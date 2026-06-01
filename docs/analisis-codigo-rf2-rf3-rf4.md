# Reporte de Análisis de Código — RF2, RF3, RF4

## Índice
1. [Metodología](#1-metodología)
2. [RF2 — Soporte Legacy DBF/Fox](#2-rf2--soporte-legacy-dbffox)
3. [RF3 — Backup de archivos locales](#3-rf3--backup-de-archivos-locales)
4. [RF4 — Sincronización dinámica](#4-rf4--sincronización-dinámica)
5. [Hallazgos generales de arquitectura](#5-hallazgos-generales-de-arquitectura)
6. [Resumen de severidad](#6-resumen-de-severidad)
7. [Recomendaciones priorizadas](#7-recomendaciones-priorizadas)
8. [Correcciones realizadas](#8-correcciones-realizadas)

---

## 1. Metodología

- **Revisión manual** de los 24 archivos fuente (23 `.cs` + `config.json`)
- Framework: .NET 8.0 Windows Forms (WinExe)
- Validación contra los requerimientos funcionales RF2, RF3, RF4
- Identificación de: errores potenciales, code smells, duplicación, mejoras
- Estado a fecha: 31/05/2026 (post-correcciones)

---

## 2. RF2 — Soporte Legacy (DBF/Fox)

### Archivos involucrados
| Archivo | Rol | Funciones clave |
|---------|-----|-----------------|
| `winbackup\DbfFileValidatorService.cs` | Validación de archivos DBF | `Validar()`, `EstaBloqueado()`, `CabeceraValida()` |
| `winbackup\IDbfFileValidatorService.cs` | Interfaz del validador | `Validar(string)` |
| `winbackup\FileScannerService.cs` (líneas 105-118) | Integración del validador en el escaneo | `EscanearRuta()` — bloque DBF |
| `winbackup\Form1.cs` (línea 189) | Inyección del validador | `Form1_Load()` |
| `winbackup\clconfiguracion.cs` (líneas 111-121) | Modelo `DbfConfig` | Propiedades `Habilitado`, `TamanoMinimoBytes`, `ValidarCabecera` |
| `winbackup\config.json` (líneas 10-14) | Configuración `DBF_CONFIG` | Valores: `habilitado: true`, `tamanoMinimoBytes: 512`, `validarCabecera: true` |
| `winbackup\FileScanResult.cs` | DTO con `EsAccesible` y `Error` | Almacena resultado del escaneo por archivo |

### Mapa de funciones RF2

```
clconfiguracion.Cargar("config.json")
  → deserializa DBF_CONFIG en DbfConfig
  → almacena en GlobalData.Config.Dbf

Form1_Load:
  → new DbfFileValidatorService()  ← lee GlobalData.Config?.Dbf en constructor
  → new FileScannerService(dbfValidator, log)

FileScannerService.EscanearRuta(ruta):
  por cada archivo .dbf:
    1. resultado.Extension == ".dbf" (detección, línea 105)
    2. dbfConfig = GlobalData.Config?.Dbf (config, línea 107)
    3. if dbfConfig.Habilitado (línea 108)
    4.   _dbfValidator.Validar(archivo) (línea 110):
          a. File.Exists(ruta)
          b. info.Length >= _tamanoMinimo (512 bytes)
          c. EstaBloqueado(): FileStream(FileShare.None) → IOException = bloqueado
          d. CabeceraValida(): leer byte 0 → FirmasValidas.Contains(header[0])
    5. resultado.EsAccesible = false + resultado.Error = razon (si falla)
```

### Requerimientos vs Implementación

| # | Requerimiento | Estado | Archivo/Línea | Detalle |
|---|--------------|--------|---------------|---------|
| RF2.1 | Detectar archivos .dbf | ✅ OK | `FileScannerService.cs:105` | `resultado.Extension == ".dbf"` con `ToLowerInvariant` |
| RF2.2 | Verificar si están bloqueados | ✅ OK | `DbfFileValidatorService.cs:26-37` | `FileShare.None` + catch `IOException` |
| RF2.3 | Evitar copiar corruptos | ⚠️ Parcial | `DbfFileValidatorService.cs:39-52` | Solo lee 1 byte de cabecera (firma). Débil contra corrupción real |

### Errores encontrados

| ID | Severidad | Archivo | Línea | Problema | Corregido |
|----|-----------|---------|-------|----------|-----------|
| B1 | 🔴 Alto | `DbfFileValidatorService.cs` | 48-51 | `catch { }` vacío en `CabeceraValida` — traga toda excepción | ✅ **SÍ** |
| B2 | 🟡 Medio | `FileScannerService.cs` | 115 | Mensaje `"DBF bloqueado: "` siempre dice "bloqueado" aunque la falla sea por firma inválida | ❌ No |
| B3 | 🟡 Medio | `FileScannerService.cs` | 113 | Marca `EsAccesible = false` cuando el DBF es inválido → semántica incorrecta | ❌ No |
| B4 | 🔴 Alto | `FileScannerService.cs` | 107 | `GlobalData.Config?.Dbf` dentro del `foreach` (no cacheado) | ❌ No |
| B5 | 🟡 Medio | `DbfFileValidatorService.cs` | 19-24 | Constructor sin parámetros lee `GlobalData.Config` (acoplamiento estático) | ❌ No |
| B6 | ⚪ Bajo | `DbfFileValidatorService.cs` | 22 | `_tamanoMinimo = 512` — número mágico sin constante nombrada | ❌ No |

### Code Smells
- **Service Locator**: `DbfFileValidatorService` depende de `GlobalData.Config` en el constructor
- **Validación débil**: solo 1 byte de firma contra 13 conocidos; no valida fecha (bytes 1-3) ni conteo de registros (bytes 4-7)
- **Mezcla de responsabilidades**: `FileScannerService` tiene lógica de DBF incrustada en lugar de ser un filtro plugueable

### Mejoras sugeridas (pendientes)
1. Agregar constructor parametrizado que reciba `DbfConfig` directamente
2. ~~Reemplazar `catch { }` con captura específica (`IOException`)~~ ✅ Hecho
3. Agregar `EsValido` a `FileScanResult` (separado de `EsAccesible`)
4. Subir la validación de cabecera a mínimo 8 bytes (firma + fecha + conteo de registros)
5. Cachear `dbfConfig` fuera del `foreach` en `EscanearRuta`

---

## 3. RF3 — Backup de archivos locales

### Archivos involucrados
| Archivo | Rol | Funciones clave |
|---------|-----|-----------------|
| `winbackup\FileScannerService.cs` | Escaneo de archivos | `EscanearTodo()`, `EscanearRuta()` |
| `winbackup\IFileScannerService.cs` | Interfaz del escáner | `EscanearTodo()`, `EscanearRuta()` |
| `winbackup\FileScanResult.cs` | Resultado del escaneo | Propiedades: `RutaCompleta`, `CarpetaOrigen`, `RutaRelativa`, `EsAccesible`, `UltimaModificacion`, etc. |
| `winbackup\clconfiguracion.cs` (líneas 72-93) | Modelos `RutaBackupConfig`, `RutasConfig` | `Origen`, `Destino`, `Patron`, `IncluirSubcarpetas`, `Excluir` |
| `winbackup\config.json` (líneas 15-32) | Configuración `RUTAS` | 2 rutas: `C:\Backup\Fuentes` y `C:\Sistema\Data` |
| `winbackup\Form1.cs` (líneas 186-193) | Integración del escáner | `Form1_Load()` crea `FileScannerService` |
| `winbackup\GlobalData.cs` | Contenedor estático de configuración | `Config` property |
| `winbackup\LogService.cs` | Servicio de logging | `Info()`, `Exito()`, `Advertencia()`, `Error()` |
| `winbackup\clconfiguracion.cs` (líneas 25-45) | Carga de configuración | `Cargar(rutaArchivo)` |

### Mapa de funciones RF3

```
Form1_Load:
  → clconfiguracion.Cargar("config.json")  ← resuelve ruta contra BaseDirectory
  → GlobalData.Config = resultado
  → new FileScannerService(dbfValidator, log)
  → _fileScanner.EscanearTodo()  ← escaneo inicial

FileScannerService.EscanearTodo():
  por cada ruta en GlobalData.Config.Rutas.Carpetas:
    1. if !Directory.Exists(ruta.Origen) → log warning + continue
    2. archivos = EscanearRuta(ruta)      ← delega el escaneo

FileScannerService.EscanearRuta(ruta):
  1. Directory.EnumerateFiles(ruta.Origen, ruta.Patron, SearchOption)
  2. por cada archivo:
     a. Crear FileScanResult (RutaCompleta, Nombre, Extension, CarpetaOrigen, RutaRelativa)
     b. if Extensión in ruta.Excluir → continue
     c. new FileInfo(archivo) → TamañoBytes, UltimaModificacion, EsAccesible
     d. Si .dbf → validación DBF (RF2)
     e. Agregar a resultados
  3. catch (UnauthorizedAccessException) → log warning
     catch (DirectoryNotFoundException) → log warning
  4. return resultados
```

### Requerimientos vs Implementación

| # | Requerimiento | Estado | Archivo/Línea | Detalle |
|---|--------------|--------|---------------|---------|
| RF3.1 | Registrar rutas/carpetas | ✅ OK | `clconfiguracion.cs:25-39`, `config.json:15-32` | Carga desde JSON con `JsonPropertyName`. Ruta resuelta contra `AppDomain.CurrentDomain.BaseDirectory` |
| RF3.2 | Leer archivos de carpetas | ✅ OK | `FileScannerService.cs:65-68` | `Directory.EnumerateFiles` con filtro y búsqueda recursiva |
| RF3.3 | Procesar múltiples rutas | ✅ OK | `FileScannerService.cs:35-42` | Itera `GlobalData.Config.Rutas.Carpetas` |

### Errores encontrados

| ID | Severidad | Archivo | Línea | Problema | Corregido |
|----|-----------|---------|-------|----------|-----------|
| C1 | 🔴 Crítico | `FileScannerService.cs` | 33 | `GlobalData.Config.Rutas.Carpetas.Count` — sin null-conditional (`?.`) | ✅ **SÍ** |
| C2 | 🔴 Alto | `FileScannerService.cs` | 123-128 | catch vacíos para `UnauthorizedAccessException` y `DirectoryNotFoundException` | ✅ **SÍ** |
| C3 | 🔴 Alto | `FileScannerService.cs` | 37-38 | `continue` sin logging cuando `Directory.Exists` es false en `EscanearTodo` | ✅ **SÍ** |
| C4 | 🟡 Medio | `GlobalData.cs` | 19-22 | `Inicializar()` nunca llamado (código muerto) | ❌ No |
| C5 | 🟡 Medio | `Form1.cs` | 185 | Ruta relativa `"config.json"` — ya corregida en `clconfiguracion.cs:29-33` | ✅ **SÍ** |
| C6 | 🟡 Medio | `clconfiguracion.cs` | 35-38 | `catch (Exception ex) { throw new Exception(...); }` — pierde stack trace original | ❌ No |
| C7 | 🟡 Medio | `clconfiguracion.cs` | 33 | `JsonSerializer.Deserialize<>` puede devolver null | ❌ No |
| C8 | ⚪ Bajo | `clconfiguracion.cs` | 63 | `BackupsConfig.Cantidad` es `string` (debería ser `int`) | ❌ No |

### Code Smells
- **Duplicación de `Directory.Exists`**: en `EscanearTodo` (línea 37-41) y en `EscanearRuta` (línea 51)
- **LogService duplicado**: las ramas `if`/`else` del `InvokeRequired` tienen código idéntico
- **Sin límite de archivos**: búsqueda recursiva sin `MaxResults` ni límite de profundidad

### Mejoras sugeridas (pendientes)
1. ~~Agregar null-conditional en `FileScannerService.cs:33`~~ ✅ Hecho
2. ~~Agregar logging en todos los `catch` vacíos~~ ✅ Hecho
3. ~~Usar `AppDomain.CurrentDomain.BaseDirectory` para localizar `config.json`~~ ✅ Hecho
4. Agregar validación post-deserialización de `clconfiguracion`
5. Preservar inner exception en `clconfiguracion.cs:35-38`

---

## 4. RF4 — Sincronización dinámica

### Archivos involucrados
| Archivo | Rol | Funciones clave |
|---------|-----|-----------------|
| `winbackup\FileSnapshotCache.cs` | Caché y detección de cambios | `CompararYDiferenciar()`, `ActualizarDesdeLista()`, `Remover()` |
| `winbackup\FileSystemMonitor.cs` | Monitor de cambios en tiempo real | `Iniciar()`, `Detener()`, `AplicarDebounce()` |
| `winbackup\IFileSystemMonitor.cs` | Interfaz del monitor | `Iniciar()`, `Detener()` |
| `winbackup\BackupScheduler.cs` | Timer periódico | `Iniciar()`, `Detener()`, `Timer_Elapsed()` |
| `winbackup\IBackupScheduler.cs` | Interfaz del scheduler | `Iniciar()`, `Detener()` |
| `winbackup\IBackupEngine.cs` | Interfaz del motor de backup | `ProcesarCambios()`, `ProcesarArchivoIndividual()` |
| `winbackup\BackupEngine.cs` | Motor de backup (copia local) | `ProcesarCambios()`, `ProcesarArchivoIndividual()` |
| `winbackup\FileChangeEvent.cs` | DTO de evento de cambio | Propiedades: `Ruta`, `Tipo`, `Timestamp`, `Nombre`, `Extension` |
| `winbackup\TipoCambio.cs` | Enum de tipos de cambio | `Creado`, `Modificado`, `Eliminado`, `Renombrado` |
| `winbackup\Form1.cs` (líneas 196-215, 228-294) | Integración | `Form1_Load()`, `Scheduler_OnTick()`, `Monitor_OnCambioDetectado()`, `CrearResultadoDesdeRuta()` |
| `winbackup\clconfiguracion.cs` (líneas 96-109) | Modelo `SincronizacionConfig` | `Habilitado`, `IntervaloSegundos`, `DebounceMs` |
| `winbackup\config.json` (líneas 33-37) | Configuración `SINCRONIZACION` | Valores: `habilitado: true`, `intervaloSegundos: 300`, `debounceMs: 1500` |

### Mapa de funciones RF4

```
Form1_Load:
  → new FileSnapshotCache()
  → new BackupScheduler()
  → new BackupEngine(log)
  → _scheduler.OnTick += Scheduler_OnTick
  → _scheduler.Iniciar(intervaloSegundos)   ← AutoReset = false
  → por cada ruta:
      new FileSystemMonitor(debounceMs, log)
      monitor.OnCambioDetectado += Monitor_OnCambioDetectado
      monitor.Iniciar(ruta.Origen, patron, incluirSubcarpetas)

--- FLUJO PERIÓDICO (scheduler) ---
BackupScheduler.Timer_Elapsed:
  1. Monitor.TryEnter(_lockTick) → si no puede, return (evita concurrencia)
  2. OnTick?.Invoke()
  3. _timer.Start()  ← reinicia timer (AutoReset=false)
  
Scheduler_OnTick (en Form1):
  1. _fileScanner.EscanearTodo()
  2. _snapshotCache.CompararYDiferenciar(archivosActuales):
     - Nuevos: path existe ahora pero no antes
     - Modificados: LastWriteTime difiere
     - Eliminados: path existía antes pero no ahora
  3. if HayCambios:
     a. Loggear cada cambio (+ nombre, ~ nombre, - nombre)
     b. _backupEngine.ProcesarCambios(archivosActuales, cambios)
        → por cada archivo nuevo/modificado:
           1. Buscar RutaBackupConfig por CarpetaOrigen
           2. Construir destPath = BaseDirectory + Destino + RutaRelativa
           3. Directory.CreateDirectory(destDir)
           4. File.Copy(origen, destPath, overwrite: true)
           5. Log éxito/error
     c. _snapshotCache.ActualizarDesdeLista(archivosActuales)

--- FLUJO EN TIEMPO REAL (watcher) ---
FileSystemMonitor:
  FileSystemWatcher.Created → AplicarDebounce(path, Creado)
  FileSystemWatcher.Changed → AplicarDebounce(path, Modificado)
  FileSystemWatcher.Deleted → DispararEvento(path, Eliminado)  ← sin debounce
  FileSystemWatcher.Renamed → DispararEvento(path, Renombrado) ← sin debounce
  FileSystemWatcher.Error   → _log.Error(msg)                  ← nuevo!

AplicarDebounce(ruta, tipo):
  1. Cancelar CTS anterior para esta ruta (si existe)
  2. Task.Delay(debounceMs).ContinueWith:
     → DispararEvento(ruta, tipo)

Monitor_OnCambioDetectado (en Form1):
  1. Loggear cambio (+/-/~/> nombre) en lstRegistro (vía Invoke)
  2. Si Eliminado → _snapshotCache.Remover(ruta)
  3. Si Creado o Modificado:
     a. Crear FileScanResult desde la ruta (CrearResultadoDesdeRuta)
     b. _backupEngine.ProcesarArchivoIndividual(resultado)
        → Copia el archivo al destino inmediatamente
```

### Requerimientos vs Implementación

| # | Requerimiento | Estado | Archivo/Línea | Detalle |
|---|--------------|--------|---------------|---------|
| RF4.1 | Detectar archivos nuevos | ✅ OK | `FileSnapshotCache.cs:81-85` | path en scan actual pero no en snapshot |
| RF4.2 | Detectar archivos modificados | ✅ OK | `FileSnapshotCache.cs:87-90` | Compara `LastWriteTime` |
| RF4.3 | Detectar archivos eliminados | ✅ OK | `FileSnapshotCache.cs:93-95` | Keys en snapshot que no están en escaneo actual |
| RF4.4 | Ejecutar automáticamente con timer | ✅ OK | `BackupScheduler.cs:17-24,37-48` | `System.Timers.Timer` con `AutoReset = false` + guarda de concurrencia `Monitor.TryEnter` |
| RF4.5 | Solo copiar si hubo cambios | ✅ OK | `Form1.cs:234-244`, `BackupEngine.cs:19-52` | Verifica `cambios.HayCambios`, luego ejecuta `BackupEngine.ProcesarCambios()` |

### Errores encontrados

| ID | Severidad | Archivo | Línea | Problema | Corregido |
|----|-----------|---------|-------|----------|-----------|
| D1 | 🔴 **Crítico** | `Form1.cs:234-244` | `Scheduler_OnTick` solo loggea cambios, no ejecuta backup | ✅ **SÍ** (BackupEngine + copia local) |
| D2 | 🔴 **Crítico** | `clconfiguracion.cs:107-108` | `SoloSiHayCambios` definido pero nunca leído | ✅ **SÍ** (eliminado) |
| D3 | 🔴 Alto | `BackupScheduler.cs:17-23` | `AutoReset = true` puede causar ticks concurrentes | ✅ **SÍ** (AutoReset=false + Monitor.TryEnter) |
| D4 | 🔴 Alto | `FileSystemMonitor.cs:30-35` | No suscribe al evento `Error` del FileSystemWatcher | ✅ **SÍ** |
| D5 | 🟡 Medio | `FileSystemMonitor.cs:60-78` | Race en `AplicarDebounce`: `TryGetValue` + asignación no atómicos | ❌ No |
| D6 | 🟡 Medio | `FileSystemMonitor.cs:71-78` | `ContinueWith` puede ejecutarse después de `Detener` | ❌ No |
| D7 | 🟡 Medio | `FileSystemMonitor.cs:39-40` | Eventos `Deleted` y `Renamed` sin debounce | ❌ No |
| D8 | 🟡 Medio | `FileSystemMonitor.cs:40` | Rename solo usa `e.FullPath`, ignora `e.OldFullPath` | ❌ No |
| D9 | ⚪ Bajo | `Form1.cs:252-269` | Antes: solo loggeaba. Ahora: también ejecuta backup vía `ProcesarArchivoIndividual` | ✅ **SÍ** |
| D10 | ⚪ Bajo | `FileSnapshotCache.cs:30-69` | `HaCambiado()`, `ObtenerEliminados()`, `ObtenerSnapshot()` — código muerto | ❌ No |

### Code Smells
- **Snapshot y Watcher inconsistentes**: el monitor en tiempo real y el scheduler periódico comparten `_snapshotCache` pero no hay una orquestación unificada
- **Missing `IDisposable`**: `FileSystemMonitor` no implementa `IDisposable`, aunque contiene `FileSystemWatcher` (IDisposable)

### Mejoras sugeridas (pendientes)
1. ~~Implementar la acción de backup en `Scheduler_OnTick` (copia local)~~ ✅ Hecho
2. ~~Cambiar `AutoReset = false` y reiniciar el timer manualmente~~ ✅ Hecho
3. ~~Suscribir al evento `Error` del `FileSystemWatcher`~~ ✅ Hecho
4. ~~Usar `Monitor.TryEnter` en `Scheduler_OnTick` para evitar concurrencia~~ ✅ Hecho (en BackupScheduler)
5. Aplicar debounce consistente a todos los tipos de eventos
6. Remover código muerto (`HaCambiado`, `ObtenerEliminados`, `ObtenerSnapshot`)
7. Agregar retry logic al backup (File.Copy puede fallar por archivos bloqueados temporalmente)

---

## 5. Hallazgos generales de arquitectura

### 5.1 Seguridad (🔴 Crítico)
*(Sin cambios — no corresponde a RF2/RF3/RF4)*

| ID | Archivo | Línea | Problema |
|----|---------|-------|----------|
| S1 | `clseguridad.cs` | 12 | **Passphrase hardcodeada**: `"VIAFACT_SECURITY_KEY_TINGO_2026!"` en source code. Cualquiera con el binario puede desencriptar todas las contraseñas |
| S2 | `clseguridad.cs` | 13 | **Salt estático**: `"Salt_Backup_System"` — debería ser aleatorio por operación |
| S3 | `clseguridad.cs` | 22 | **Solo 1000 iteraciones PBKDF2** (OWASP recomienda ≥600,000, NIST ≥1,200,000) |
| S4 | `clseguridad.cs` | 10-13 | Material criptográfico en campos `static readonly` → extraíble por reflexión/decompilación |
| S5 | `config.json` | 5 | **FTP sin SSL**: `ftp://` transmite credenciales en texto claro |
| S6 | `config.json` | 3-6 | **Credenciales de producción en git** → exposición permanente |
| S7 | `config.json` | 18,25 | **Rutas de producción en git** → información de infraestructura expuesta |

### 5.2 Concurrencia (🔴 Alto)

| ID | Archivo | Línea | Problema | Estado |
|----|---------|-------|----------|--------|
| T1 | `LogService.cs` | 38 vs 42-49 | **TOCTOU race**: se verifica `_listBox.IsDisposed` y luego se llama `Invoke` | ❌ No corregido |
| T2 | `BackupScheduler.cs` | 15-23 | ~~Ticks concurrentes si escaneo supera el intervalo~~ | ✅ **Corregido** (AutoReset=false + Monitor.TryEnter) |
| T3 | `Form1.cs` | 229-243 | `Scheduler_OnTick` (ThreadPool) y `Monitor_OnCambioDetectado` (ThreadPool) acceden `_snapshotCache` concurrentemente | ❌ No corregido |
| T4 | `GlobalData.cs` | 13 | `Config` sin sincronización — lecturas/escrituras desde múltiples threads | ❌ No corregido |

### 5.3 Null Safety (🔴 Alto)

| ID | Archivo | Línea | Problema | Estado |
|----|---------|-------|----------|--------|
| N1 | `FileScannerService.cs` | 33 | ~~GlobalData.Config.Rutas.Carpetas.Count sin null-conditional~~ | ✅ **Corregido** |
| N2 | `clconfiguracion.cs` | 33 | `JsonSerializer.Deserialize<>` puede devolver null | ❌ No corregido |
| N3 | `clconfiguracion.cs` | 29 | `File.Exists(null)` si `rutaArchivo` es null | ❌ No corregido |

### 5.4 Nombramiento y Organización

| ID | Archivo | Línea | Problema |
|----|---------|-------|----------|
| O1 | `clconfiguracion.cs` | 8 | Prefijo húngaro `cl` → `Configuracion` o `AppConfig` |
| O2 | `clseguridad.cs` | 8 | Prefijo húngaro `cl` → `SecurityHelper` o `CryptoService` |
| O3 | Múltiples archivos | Varias | Métodos en español (`EscanearTodo`, `Validar`, `CabeceraValida`, etc.) mezclados con interfaces en inglés |
| O4 | `Form1.cs` | 17-21 | **God class**: Form1 orquesta todo (escáner, scheduler, monitores, log, backup engine) violando SRP |

### 5.5 Código Muerto

| ID | Archivo | Línea | Código | Estado |
|----|---------|-------|--------|--------|
| M1 | `GlobalData.cs` | 19-22 | `Inicializar()` nunca invocado | ❌ No corregido |
| M2 | `Form1.cs` | 135-138 | `contextMenuStrip1_Opening` handler vacío | ❌ No corregido |
| M3 | `Form1.cs` | 222-225 | `btnEnviar_Click_1` handler vacío | ❌ No corregido |
| M4 | `FileSnapshotCache.cs` | 30-69 | `HaCambiado`, `ObtenerEliminados`, `ObtenerSnapshot` | ❌ No corregido |
| M5 | `clconfiguracion.cs` | 107-108 | `SoloSiHayCambios` nunca leído | ✅ **Corregido** (eliminado) |

### 5.6 Acoplamiento

| ID | Archivo | Línea | Problema |
|----|---------|-------|----------|
| C1 | `GlobalData.cs` | 13 | Estado global estático mutable — anti-patrón Service Locator |
| C2 | `LogService.cs` | 10-11 | Acoplado a `ListBox` de WinForms — no se puede loguear a archivo |
| C3 | `FileScannerService.cs` | 13-14 | Constructor default crea dependencias concretas (`new DbfFileValidatorService()`) |
| C4 | `Form1.cs` | 28-99 | Método monolítico `btnEnviar_Click` mezcla compresión, FTP, UI |

---

## 6. Resumen de severidad

| Severidad | Conteo original | Conteo actual | Cambio |
|-----------|----------------|---------------|--------|
| 🔴 **Crítico** | 5 | 3 | Se corrigieron RF4 D1 (BackupEngine) y D2 (SoloSiHayCambios) |
| 🔴 **Alto** | 12 | 9 | Se corrigieron B1, C1, C2, C3, D3, D4 |
| 🟡 **Medio** | 14 | 14 | Sin cambios |
| ⚪ **Bajo** | 8 | 8 | Sin cambios |

---

## 7. Recomendaciones priorizadas

### Inmediatas (Semana 1) — Pendientes
1. **Seguridad**: Reemplazar cifrado casero por DPAPI (`ProtectedData.Protect`/`Unprotect`) o remover credenciales del repositorio
2. **Null safety**: Agregar validación post-deserialización en `clconfiguracion` (C7)
3. **TOCTOU race**: Corregir `LogService.cs:38-49` (T1)

### Corto plazo (Semana 2-3) — Pendientes
4. Agregar `EsValido` a `FileScanResult` (B3)
5. Cachear `dbfConfig` fuera del `foreach` en `EscanearRuta` (B4)
6. Aplicar debounce consistente a todos los tipos de eventos en `FileSystemMonitor` (D7)
7. Remover código muerto restante (M1, M2, M3, M4)
8. Extraer lógica de FTP/backup de `Form1.cs` a servicios dedicados
9. Agregar retry logic al `File.Copy` en `BackupEngine`

### Mediano plazo (Mes 1-2)
10. Reemplazar `GlobalData` estático por inyección de dependencias
11. Agregar pruebas unitarias para `DbfFileValidatorService`, `FileSnapshotCache`, `FileScannerService`
12. Estandarizar nomenclatura (todo en español o todo en inglés)
13. Agregar validación de firma DBF a mínimo 8 bytes (RF2.3)

---

## 8. Correcciones realizadas

| # | ID | Hash | Fecha | Archivos | Descripción |
|---|----|------|-------|----------|-------------|
| 1 | B1 | `22ee3a3` | 2026-05-31 | `DbfFileValidatorService.cs` | Reemplazar `catch { }` en `CabeceraValida` por captura específica de `IOException` y `UnauthorizedAccessException` |
| 2 | C1 | `1ecca01` | 2026-05-31 | `FileScannerService.cs` | Agregar null-conditional (`?.`) en acceso a `GlobalData.Config.Rutas.Carpetas.Count` |
| 3 | C2 | `a0239d2` | 2026-05-31 | `FileScannerService.cs` | Agregar logging (`_log.Advertencia`) en los catch de `UnauthorizedAccessException` y `DirectoryNotFoundException` en `EscanearRuta` |
| 4 | C3 | `61dc7f1` | 2026-05-31 | `FileScannerService.cs` | Agregar `_log.Advertencia()` cuando `Directory.Exists` es false en `EscanearTodo` (antes: `continue` silencioso) |
| 5 | D2 | `ebf25d5` | 2026-05-31 | `clconfiguracion.cs`, `config.json` | Eliminar propiedad `SoloSiHayCambios` del modelo `SincronizacionConfig` y del JSON (código muerto) |
| 6 | D3 | `a5f38a9` | 2026-05-31 | `BackupScheduler.cs` | Cambiar `AutoReset = true` a `false` + agregar `Monitor.TryEnter` para evitar ticks concurrentes. El timer se reinicia manualmente tras cada tick |
| 7 | D4 | `fc566d1` | 2026-05-31 | `FileSystemMonitor.cs`, `Form1.cs` | Agregar `ILogService` al constructor de `FileSystemMonitor`. Suscribir al evento `Error` del `FileSystemWatcher` para detectar desborde de buffer |
| 8 | D1 | `4f186f5` | 2026-05-31 | `IBackupEngine.cs`, `BackupEngine.cs`, `Form1.cs` | **Crear `BackupEngine`**: copia archivos nuevos/modificados al directorio destino local (`BaseDirectory + Destino + RutaRelativa`). Integrar en `Scheduler_OnTick` y `Monitor_OnCambioDetectado`. Agregar método auxiliar `CrearResultadoDesdeRuta()` |

---

## Archivos del proyecto

| Archivo | Líneas | Función principal | RF |
|---------|--------|-------------------|----|
| `winbackup\Program.cs` | ? | Punto de entrada | — |
| `winbackup\Form1.cs` | ~294 | UI principal, orquestación de servicios (escáner, scheduler, monitores, backup engine, log) | RF2, RF3, RF4 |
| `winbackup\Form1.Designer.cs` | ? | Diseñador de Form1 | — |
| `winbackup\GlobalData.cs` | 24 | Contenedor estático de configuración | RF3 |
| `winbackup\clconfiguracion.cs` | ~116 | Modelos y carga de configuración JSON | RF2, RF3, RF4 |
| `winbackup\clseguridad.cs` | 81 | Cifrado AES de contraseñas | — |
| `winbackup\IFileScannerService.cs` | 10 | Interfaz del escáner | RF2, RF3 |
| `winbackup\FileScannerService.cs` | ~137 | **RF2, RF3**: Escaneo de archivos con validación DBF | RF2, RF3 |
| `winbackup\FileScanResult.cs` | 17 | DTO de resultado de escaneo | RF2, RF3 |
| `winbackup\IDbfFileValidatorService.cs` | 10 | Interfaz del validador DBF | RF2 |
| `winbackup\DbfFileValidatorService.cs` | ~72 | **RF2**: Validación de archivos DBF (bloqueo via `FileShare.None`, firma de 1 byte, tamaño mínimo) | RF2 |
| `winbackup\IFileSystemMonitor.cs` | 12 | Interfaz del monitor de cambios | RF4 |
| `winbackup\FileSystemMonitor.cs` | ~93 | **RF4**: Monitor de cambios en tiempo real (FileSystemWatcher + debounce 1.5s + error event) | RF4 |
| `winbackup\FileChangeEvent.cs` | 13 | DTO de evento de cambio | RF4 |
| `winbackup\TipoCambio.cs` | 10 | Enum de tipos de cambio | RF4 |
| `winbackup\IBackupScheduler.cs` | 12 | Interfaz del scheduler | RF4 |
| `winbackup\BackupScheduler.cs` | ~48 | **RF4**: Timer periódico (`AutoReset=false`, `Monitor.TryEnter` para evitar concurrencia) | RF4 |
| `winbackup\FileSnapshotCache.cs` | 118 | **RF4**: Caché de snapshots y detección de cambios (diff por LastWriteTime) | RF4 |
| `winbackup\IBackupEngine.cs` | 14 | Interfaz del motor de backup | RF4 |
| `winbackup\BackupEngine.cs` | ~90 | **RF4**: Motor de backup — copia local de archivos nuevos/modificados al directorio destino | RF4 |
| `winbackup\ILogService.cs` | 14 | Interfaz de logging | RF2, RF3, RF4 |
| `winbackup\LogService.cs` | 59 | Implementación de logging a ListBox (`lstRegistro`) | RF2, RF3, RF4 |
| `winbackup\config.json` | ~41 | Configuración de la aplicación (CREDENCIALES, BACKUPS, DBF_CONFIG, RUTAS, SINCRONIZACION) | RF2, RF3, RF4 |
| `winbackup\winbackup.csproj` | ~14 | Archivo de proyecto .NET 8.0-windows (copias config.json al output) | — |

---

*Reporte generado el 2026-05-31 — Versión con correcciones aplicadas (8 commits)*
