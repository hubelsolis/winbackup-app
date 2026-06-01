# Análisis de Arquitectura — winbackup-app

## 1. Arquitectura actual

| Capa | Tecnología | Componentes |
|------|-----------|-------------|
| Presentación | Windows Forms (.NET 8) | `Form1.cs`, `Form1.Designer.cs` |
| Lógica de negocio | C# (en el code-behind de Form1) | Compresión, chunking, subida FTP |
| Datos / Config | JSON plano + Singleton estático | `config.json`, `GlobalData`, `clconfiguracion` |
| Seguridad | AES-256 (Rfc2898DeriveBytes) | `clseguridad` |
| Pruebas | MSTest | `winbackup.Tests` |

Patrón: **Monolito WinForms con acoplamiento fuerte**. Toda la lógica de backup vive dentro de `Form1.cs`. No hay separación en capas (servicios, repositorios, workers). La configuración se carga como singleton estático vía `GlobalData.Config`.

---

## 2. Flujo principal de ejecución

```
Program.Main()
  └─ Application.Run(new Form1())
       └─ Form1_Load()
            └─ GlobalData.Config = clconfiguracion.Cargar("config.json")
                 └─ JsonSerializer.Deserialize<clconfiguracion>(jsonString)
                      └─ deserializa: CredencialesConfig, BackupsConfig

       └─ btnEnviar_Click()
            └─ Task.Run:
                 1. Comprime D:\PDF-DOC-... → .zip
                 2. Divide .zip en fragmentos de 2 MB
                 3. Sube cada fragmento vía FTP (FtpWebRequest)
                 4. Elimina .zip temporal
```

Actualmente solo hay un flujo de subida manual (botón "Enviar") con un archivo fijo `D:\PDF-DOC-E001-10420604979669.pdf`.

---

## 3. Cómo inicia la aplicación

`Program.cs:9` — Punto de entrada `[STAThread] static void Main()`.

1. `ApplicationConfiguration.Initialize()` — configuración DPI/font de WinForms.
2. `Application.Run(new Form1())` — crea el formulario principal e inicia el bucle de mensajes.
3. En el evento `Form1_Load` se carga `config.json` mediante `clconfiguracion.Cargar()` y se asigna a `GlobalData.Config`.

No hay un archivo `.sln` (se usa `winbackup.slnx`). El `.csproj` apunta a `net8.0-windows` con `UseWindowsForms=true`, `OutputType=WinExe`.

---

## 4. Responsabilidades por clase

### 4.1 Configuración JSON — `clconfiguracion.cs`
- Models: `clconfiguracion`, `CredencialesConfig`, `BackupsConfig`
- Método estático `Cargar(rutaArchivo)` que lee JSON y deserializa con `System.Text.Json`
- `CredencialesConfig.Pass` es una propiedad calculada que llama a `clseguridad.Desencriptar()` al vuelo
- `config.json` contiene `CREDENCIALES` (user, pass encriptado, ftpBaseUrl) y `BACKUPS` (backups: número de copias)

### 4.2 Monitoreo de carpetas — **NO IMPLEMENTADO**
No existe `FileSystemWatcher` ni ninguna clase de monitoreo. Hay un placeholder de menú "Comprobar Cambios" sin handler.

### 4.3 Backups — `Form1.cs` (parcial)
- Compresión con `System.IO.Compression.ZipArchive`
- Chunking de 2 MB con `FileStream`
- Subida FTP con `FtpWebRequest` y `NetworkCredential`
- No hay lógica de respaldo programado, ni rotación, ni monitoreo de cambios

### 4.4 Logs — **NO IMPLEMENTADO**
No hay sistema de logging (archivos, consola, EventLog, etc.). Hay un `ListBox` llamado `lstRegistro` y una `StatusStrip` llamada `sttBarraEstado` que no se usan. El método `ActualizarStatus()` está commenteado.

### 4.5 Timers — **NO IMPLEMENTADO**
No hay `System.Windows.Forms.Timer` ni `System.Threading.Timer`. No hay ejecución periódica ni programada.

### 4.6 Otras clases
- **`GlobalData.cs`**: Contenedor estático `public static clconfiguracion Config`. Método `Inicializar()` que llama a `clconfiguracion.Cargar()` sin parámetro (usa default "config.json").
- **`clseguridad.cs`**: Cifrado AES con Passphrase fija (`VIAFACT_SECURITY_KEY_TINGO_2026!`) y Salt fijo. Provee `Encriptar()` y `Desencriptar()`.
- **`Form1.Designer.cs`**: Componentes visuales: menús (Copia Seguridad → Generar Ahora, Comprobar Cambios; Configuración → Sistema, Copias), `notifyIcon1` con minimización a bandeja, `lstCopias` con datos quemados (hardcoded), `lstRegistro`, `sttBarraEstado`.

---

## 5. Dependencias entre módulos

```
Program.cs
  └─ Form1.cs
       ├─ GlobalData.cs (estático)
       │    └─ clconfiguracion.cs
       │         └─ clseguridad.cs
       └─ System.IO.Compression
       └─ System.Net (FtpWebRequest)

GlobalData.cs
  └─ clconfiguracion.cs
       └─ clseguridad.cs

winbackup.Tests/
  ├─ clconfiguracionTests.cs → clconfiguracion
  ├─ clseguridadTests.cs → clseguridad
  ├─ GlobalDataTests.cs → GlobalData
  └─ Test1.cs (vacío)
```

No hay inyección de dependencias, interfaces, ni abstracciones. Todo es acoplamiento directo a clases concretas.

---

## 6. Puntos de implementación para nuevos RF

### RF2 — Soporte Legacy DBF/Fox

**¿Dónde implementarlo?**
- Crear una nueva clase `dBFFoxHelper.cs` (o similar) en el proyecto `winbackup/`
- Ubicación lógica: junto a `clconfiguracion.cs` y `clseguridad.cs`
- Podría leer estructura de archivos `.DBF` usando `System.Data.OleDb` con el driver `Microsoft.Jet.OLEDB.4.0` o `VFPOLEDB`
- Desde `Form1.cs` se invocaría para detectar archivos DBF en las carpetas monitoreadas

**Puntos de anclaje existentes:**
- `clconfiguracion.cs` — agregar sección `DBF_CONFIG` en el JSON con rutas de tablas Fox
- `Form1.cs` — en el flujo de backup, antes de comprimir, detectar si el archivo es DBF y aplicar lectura estructurada
- Tests: nuevo `dBFFoxHelperTests.cs` en `winbackup.Tests/`

### RF3 — Backup de archivos locales

**¿Dónde implementarlo?**
- Crear una clase `BackupEngine.cs` (o `ServicioBackup.cs`) que encapsule:
  - Compresión (ya existe en `Form1`, hay que extraerla)
  - Chunking + FTP (ya existe, hay que extraerla)
  - Rotación de copias (según `BACKUPS.backups` del JSON)
  - Cola de archivos pendientes
- `GlobalData.cs` podría expandirse para incluir configuración de carpetas origen/destino

**Puntos de anclaje existentes:**
- `clconfiguracion.cs` — agregar sección `ARCHIVOS` con `origen`, `destino`, `patrones`, `excluir`
- `config.json` — nuevas propiedades de rutas locales
- `Form1.cs` — el menú "Generar Ahora" y "Comprobar Cambios" deben disparar `BackupEngine`
- `lstCopias` debe llenarse desde datos reales, no quemados

### RF4 — Sincronización dinámica

**¿Dónde implementarlo?**
- Crear `MonitorSincronizacion.cs` usando `System.IO.FileSystemWatcher` para detectar cambios en carpetas
- Crear `ProgramadorTareas.cs` (o `TimerService.cs`) usando `System.Timers.Timer` o `System.Threading.PeriodicTimer`
- El timer consultaría la carpeta origen cada N minutos (configurable desde JSON)
- Ante un cambio detectado (`Created`, `Changed`, `Renamed`), encolar el archivo en `BackupEngine`

**Puntos de anclaje existentes:**
- `clconfiguracion.cs` — agregar sección `SINCRONIZACION` con `intervaloMinutos`, `monitorear`, `activo`
- `Form1.cs` — en `Form1_Load`, arrancar `MonitorSincronizacion` y `ProgramadorTareas` si `activo=true`
- `lstRegistro` — usar como log en vivo de eventos de sincronización
- `notifyIcon1` — mostrar notificaciones de balloon tip cuando se complete una sincronización

---

## Resumen de deuda técnica detectada

| Ítem | Severidad |
|------|-----------|
| Lógica de backup dentro de `Form1.cs` (no extraíble, no testeable) | Alta |
| Sin inyección de dependencias / interfaces | Alta |
| Sin sistema de logging | Alta |
| Sin timers / programación | Alta |
| Sin monitoreo de archivos | Alta |
| Datos quemados en `lstCopias.Items` | Media |
| Método `ActualizarStatus` commenteado | Media |
| Passphrase fija en código fuente (`clseguridad.cs:12`) | Media |
| Sin .sln (usa .slnx) | Baja |
| Tests parciales sin aserciones reales | Media |
