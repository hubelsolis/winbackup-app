# 🗜️ Módulo de Compresión — winbackup
# 🗜️ Compression Module — winbackup

---

## 📋 Descripción / Description

**ES:** Módulo de compresión integrado dentro del sistema de backups `winbackup`. Permite comprimir y descomprimir archivos usando 6 algoritmos diferentes, con soporte de contraseña en los formatos que lo permiten. Implementado siguiendo el patrón de diseño **Factory** para una arquitectura limpia y extensible.

**EN:** Compression module integrated into the `winbackup` backup system. Allows compressing and decompressing files using 6 different algorithms, with password support for formats that allow it. Implemented following the **Factory** design pattern for a clean and extensible architecture.

---

## 👥 Integrantes / Team Members

| Integrante | Módulo |
|---|---|
| **Checya** | ZIP, TAR, GZip, CAB, Factory Pattern, Interfaz, Pruebas Unitarias |
| **Estela** | RAR |
| **Romero** | LZMA |

---

## 🏗️ Estructura del Módulo / Module Structure

```
winbackup/
└── compresion/
    ├── ICompresion.cs          → Interfaz común para todos los algoritmos
    ├── CompresionZip.cs        → Algoritmo ZIP con contraseña
    ├── CompresionRar.cs        → Algoritmo RAR con contraseña
    ├── CompresionLzma.cs       → Algoritmo LZMA con contraseña
    ├── CompresionTar.cs        → Algoritmo TAR/GZip
    ├── CompresionCab.cs        → Algoritmo CAB (Windows)
    ├── CompresionGzip.cs       → Algoritmo GZip (Linux/Unix)
    └── CompresionFactory.cs    → Factory Pattern — selector de algoritmo
```

---

## ⚙️ Requisitos / Requirements

| Herramienta | Versión | Obligatorio |
|---|---|---|
| Visual Studio | 2022 o superior | ✅ |
| .NET SDK | 8.0 | ✅ |
| WinRAR | Cualquiera | ✅ Solo para comprimir RAR |
| SonarQube | 26.5.0 Community | ⚙️ Opcional |

---

## 📦 Paquetes NuGet / NuGet Packages

| Paquete | Versión | Uso |
|---|---|---|
| DotNetZip | 1.16.0 | ZIP y LZMA con contraseña |
| SharpCompress | 0.34.2 | RAR descomprimir, TAR, GZip |

> **Nota / Note:** Los paquetes se restauran automáticamente al compilar. / Packages restore automatically on build.

---

## 🚀 Instalación y Uso / Installation & Usage

### ES:
1. Clona el repositorio
2. Abre `winbackup.slnx` con Visual Studio
3. Presiona `Ctrl + Shift + B` para compilar
4. Usa `CompresionFactory.Crear("algoritmo")` para obtener el compresor deseado

### EN:
1. Clone the repository
2. Open `winbackup.slnx` with Visual Studio
3. Press `Ctrl + Shift + B` to build
4. Use `CompresionFactory.Crear("algoritmo")` to get the desired compressor

### Ejemplo / Example:
```csharp
// Comprimir con ZIP y contraseña / Compress with ZIP and password
var compresor = CompresionFactory.Crear("zip");
compresor.Comprimir(@"C:\archivo.txt", @"C:\archivo.zip", "miContrasena");

// Descomprimir / Decompress
compresor.Descomprimir(@"C:\archivo.zip", @"C:\destino\", "miContrasena");

// Listar contenido / List content
var archivos = compresor.ListarContenido(@"C:\archivo.zip");
```

---

## 🗜️ Algoritmos Disponibles / Available Algorithms

### ZIP ✅
- **Librería:** DotNetZip 1.16.0
- **Extensión:** `.zip`
- **Contraseña:** ✅ AES-256
- **Funciones:** Comprimir, Descomprimir, Listar contenido

### RAR ✅
- **Librería:** WinRAR.exe + SharpCompress (descomprimir)
- **Extensión:** `.rar`
- **Contraseña:** ✅ `-p` parámetro WinRAR
- **Nota:** RAR es un formato propietario de RARLab. No existe librería gratuita en .NET que pueda CREAR archivos RAR. Se usa WinRAR.exe para comprimir y SharpCompress para descomprimir.

### LZMA ✅
- **Librería:** DotNetZip 1.16.0
- **Extensión:** `.zip` (con compresión LZMA)
- **Contraseña:** ✅ AES-256
- **Funciones:** Comprimir, Descomprimir, Listar contenido

### TAR ✅
- **Librería:** SharpCompress 0.34.2
- **Extensión:** `.tar.gz`
- **Contraseña:** ❌ TAR es un formato antiguo sin cifrado nativo
- **Funciones:** Comprimir, Descomprimir, Listar contenido

### CAB ✅
- **Librería:** `makecab.exe` y `expand.exe` incluidos en Windows
- **Extensión:** `.cab`
- **Contraseña:** ❌ CAB no soporta contraseña nativamente
- **Nota:** No requiere instalar nada extra — usa herramientas del sistema operativo Windows.

### GZip ✅
- **Librería:** SharpCompress 0.34.2
- **Extensión:** `.tar.gz`
- **Contraseña:** ❌ GZip es un algoritmo estándar de Linux/Unix sin cifrado
- **Funciones:** Comprimir, Descomprimir, Listar contenido

---

## 🏭 Factory Pattern / Patrón Fábrica

```csharp
// Algoritmos disponibles / Available algorithms
CompresionFactory.AlgoritmosDisponibles();
// → ["ZIP", "RAR", "LZMA", "TAR", "CAB", "GZIP"]

// Crear compresor / Create compressor
ICompresion compresor = CompresionFactory.Crear("zip");
```

---

## 🧪 Pruebas Unitarias / Unit Tests

**Archivo:** `winbackup.Tests/CompresionTests.cs`

| Prueba | Algoritmo | Resultado |
|---|---|---|
| `ZIP_Comprimir_SinContrasena_CreaArchivo` | ZIP | ✅ Passed |
| `ZIP_Comprimir_ConContrasena_CreaArchivo` | ZIP | ✅ Passed |
| `ZIP_Descomprimir_SinContrasena_ExtraeArchivo` | ZIP | ✅ Passed |
| `ZIP_ListarContenido_RetornaArchivos` | ZIP | ✅ Passed |
| `TAR_Comprimir_CreaArchivo` | TAR | ✅ Passed |
| `TAR_Descomprimir_ExtraeArchivo` | TAR | ✅ Passed |
| `GZIP_Comprimir_CreaArchivo` | GZip | ✅ Passed |
| `LZMA_Comprimir_SinContrasena_CreaArchivo` | LZMA | ✅ Passed |
| `LZMA_Comprimir_ConContrasena_CreaArchivo` | LZMA | ✅ Passed |

**Total: 9 pruebas — 9 Correcta ✅**

Para ejecutar / To run:
```
Prueba → Ejecutar todas las pruebas
```

---

## 📊 Análisis de Calidad — SonarQube / Quality Analysis

| Métrica | Resultado |
|---|---|
| **Quality Gate** | ✅ Passed |
| **Security** | ⚠️ D — 3 issues |
| **Reliability** | ✅ A — 0 issues |
| **Maintainability** | ✅ A — 38 code smells |
| **Duplications** | 9.5% |
| **Lines of Code** | 571 |

**Versión SonarQube:** Community Build v26.5.0.122743

Para ejecutar el análisis / To run the analysis:
```bash
dotnet sonarscanner begin /k:"winbackup-modulo-compresion" /d:sonar.host.url="http://localhost:9000" /d:sonar.token="TU_TOKEN"
dotnet build "winbackup\winbackup.slnx"
dotnet sonarscanner end /d:sonar.token="TU_TOKEN"
```

---

## 📝 Notas Importantes / Important Notes

- **RAR:** Requiere WinRAR instalado en `C:\Program Files\WinRAR\`. Si no está instalado, el sistema lanza una excepción clara. / Requires WinRAR installed at `C:\Program Files\WinRAR\`. If not installed, the system throws a clear exception.
- **TAR y GZip:** No soportan contraseña — son formatos sin cifrado nativo. / TAR and GZip don't support passwords — they are formats without native encryption.
- **CAB:** Usa herramientas nativas de Windows (`makecab.exe`, `expand.exe`). No requiere instalación adicional. / Uses native Windows tools. No additional installation required.

---

## 📅 Fecha / Date
Junio 2026 / June 2026

