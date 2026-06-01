# Diseño de Implementación — RF2, RF3, RF4

## Arquitectura propuesta

Se respeta la arquitectura WinForms existente y se introducen **interfaces** para desacoplar la lógica de negocio de la UI. No se agrega un contenedor DI; la inyección se hace manual en el constructor de `Form1`.

```
┌─────────────────────────────────────────────────────────┐
│                      Form1.cs (UI)                       │
│  Inyecta: IBackupEngine, IFileSystemMonitor,             │
│           IBackupScheduler, ILogService, IDbfValidator   │
│  Eventos: Engine_OnBackupComplete, Monitor_OnChange      │
└────┬────────────┬──────────────┬───────────────┬────────┘
     │            │              │               │
     ▼            ▼              ▼               ▼
┌──────────┐ ┌──────────┐ ┌────────────┐ ┌──────────────┐
│Backup    │ │FileSystem│ │Backup      │ │LogService    │
│Engine    │ │Monitor   │ │Scheduler   │ │(ILogService) │
│(IBackup  │ │(IFileS..)│ │(IBackupS..)│ │              │
│Engine)   │ │          │ │            │ │              │
└────┬─────┘ └──────────┘ └────────────┘ └──────────────┘
     │
     ▼
┌──────────┐ ┌──────────┐  ┌──────────────┐
│FtpUpload │ │DbfValid. │  │FileSnapshot  │
│Service   │ │(IDbfVal.)│  │Cache         │
│(IFtpUp..)│ │          │  │(clase interna│
└──────────┘ └──────────┘  │ de BackupEng)│
                           └──────────────┘
```

Todas las dependencias concretas se instancian en `Form1_Load` y se asignan a campos privados.

---

## Diagrama de flujo general

```
Form1_Load
  ├── clconfiguracion.Cargar("config.json")
  ├── Instantiate DbfValidator
  ├── Instantiate FtpUploadService
  ├── Instantiate LogService  (con referencia a lstRegistro)
  ├── Instantiate BackupEngine (con DbfValidator + FtpUploadService + LogService)
  ├── Instantiate FileSystemMonitor (por cada ruta)
  │     └── suscribir eventos → BackupEngine.ProcesarArchivo()
  ├── Instantiate BackupScheduler (con BackupEngine)
  │     └── si sincro.habilitado → scheduler.Start()
  └── Suscribir Engine.OnBackupComplete → ActualizarUI()

Menú "Generar Ahora"
  └── BackupEngine.EjecutarBackupCompleto()

Menú "Comprobar Cambios"
  └── BackupEngine.CompararYRespaldar()

Timer tick (BackupScheduler)
  └── BackupEngine.CompararYRespaldar()

FileSystemWatcher evento
  └── debounce → BackupEngine.ProcesarArchivo(ruta)
```

---

## 1. Interfaces

### `IDbfValidator`

```csharp
namespace winbackup
{
    public interface IDbfValidator
    {
        /// <summary>Retorna true si la extensión es .dbf (case-insensitive).</summary>
        bool EsArchivoDbf(string ruta);

        /// <summary>Verifica si el archivo está bloqueado por otro proceso.</summary>
        bool EstaBloqueado(string ruta);

        /// <summary>Verifica tamaño y firma mínima de cabecera DBF.</summary>
        bool EsValido(string ruta);

        /// <summary>Valida completa: extensión + bloqueo + cabecera.</summary>
        (bool esValido, string razon) Validar(string ruta);
    }
}
```

### `IFtpUploadService`

```csharp
namespace winbackup
{
    public interface IFtpUploadService
    {
        /// <summary>Comprime, chunkea y sube un archivo completo al FTP.</summary>
        Task<bool> SubirArchivo(string rutaLocal, string nombreRemoto, IProgress<string> progreso);

        /// <summary>Sube un único chunk (para reuso desde BackupEngine).</summary>
        void UploadChunk(byte[] data, int length, string remoteUrl);
    }
}
```

### `IBackupEngine`

```csharp
namespace winbackup
{
    public interface IBackupEngine
    {
        /// <summary>Procesa todas las rutas configuradas, respalda archivos nuevos/modificados.</summary>
        Task<BackupResult> EjecutarBackupCompleto();

        /// <summary>Compara snapshot actual vs. anterior y respalda solo los cambiados.</summary>
        Task<BackupResult> CompararYRespaldar();

        /// <summary>Procesa un archivo individual (desde FileSystemWatcher).</summary>
        Task<BackupResult> ProcesarArchivo(string ruta);

        /// <summary>Snapshot actual de archivos (ruta → última modificación).</summary>
        Dictionary<string, DateTime> ObtenerSnapshot();

        /// <summary>Dispara cuando un backup individual termina.</summary>
        event Action<BackupResult> OnArchivoProcesado;

        /// <summary>Dispara cuando el backup completo termina.</summary>
        event Action<BackupResult> OnBackupCompleto;
    }

    public class BackupResult
    {
        public bool Exitoso { get; set; }
        public string Archivo { get; set; }
        public string RutaOrigen { get; set; }
        public string RutaDestinoFtp { get; set; }
        public long BytesSubidos { get; set; }
        public string Error { get; set; }
        public DateTime Fecha { get; set; }
        public List<string> ArchivosOmitidos { get; set; }
    }
}
```

### `IFileSystemMonitor`

```csharp
namespace winbackup
{
    public interface IFileSystemMonitor
    {
        void Iniciar(string ruta, string filtro = "*.*", bool incluirSubcarpetas = true);
        void Detener();
        bool EstaActivo { get; }

        /// <summary>Se dispara con debounce cuando un archivo cambia.</summary>
        event Action<string> OnArchivoCambiado;

        /// <summary>Se dispara cuando se crea un archivo.</summary>
        event Action<string> OnArchivoCreado;

        /// <summary>Se dispara cuando se elimina un archivo.</summary>
        event Action<string> OnArchivoEliminado;
    }
}
```

### `IBackupScheduler`

```csharp
namespace winbackup
{
    public interface IBackupScheduler
    {
        void Iniciar(int intervaloSegundos);
        void Detener();
        bool EstaActivo { get; }

        /// <summary>Se dispara en cada tick del timer.</summary>
        event Action OnTick;
    }
}
```

### `ILogService`

```csharp
namespace winbackup
{
    public interface ILogService
    {
        void Info(string mensaje);
        void Exito(string mensaje);
        void Error(string mensaje);
        void Advertencia(string mensaje);
        void Limpiar();
        List<string> ObtenerHistorial();
    }
}
```

---

## 2. Nuevas clases

### 2.1 `DbfValidator.cs` — RF2

```csharp
namespace winbackup
{
    public class DbfValidator : IDbfValidator
    {
        private readonly string[] _extensionesValidas;
        private readonly long _tamanoMinimoBytes;

        // Lee config de GlobalData.Config.Dbf
        public DbfValidator() { ... }

        public bool EsArchivoDbf(string ruta) { ... }
        public bool EstaBloqueado(string ruta) { ... }
        public bool EsValido(string ruta) { ... }
        public (bool, string) Validar(string ruta) { ... }
    }
}
```

**Algoritmo `Validar`:**
1. `EsArchivoDbf` → extensión `.dbf` case-insensitive
2. `File.Exists` + tamaño > `tamanoMinimoBytes`
3. `EstaBloqueado` → `using FileStream fs = new(ruta, FileMode.Open, FileAccess.Read, FileShare.None)` — si lanza `IOException` con `HRESULT` de sharing violation, está bloqueado
4. `EsValido` → leer primeros 4 bytes: DBF signature (`0x03` FoxBASE, `0x30` Visual FoxPro, `0x31`, `0x32`, `0xFB`)
5. Si todo OK → válido

### 2.2 `FtpUploadService.cs`

```csharp
namespace winbackup
{
    public class FtpUploadService : IFtpUploadService
    {
        private readonly string _ftpBaseUrl;
        private readonly string _user;
        private readonly string _pass;
        private const int CHUNK_SIZE = 2 * 1024 * 1024;

        // Lee credenciales de GlobalData.Config.Credenciales
        public FtpUploadService() { ... }

        public async Task<bool> SubirArchivo(string rutaLocal, string nombreRemoto, IProgress<string> progreso) { ... }
        public void UploadChunk(byte[] data, int length, string remoteUrl) { ... }
    }
}
```

**Lógica `SubirArchivo`** (refactoriza el `btnEnviar_Click` actual):
1. Crear ZIP temporal con `ZipArchive`
2. Abrir ZIP con `FileStream`
3. Leer en bloques de `CHUNK_SIZE`
4. Para cada bloque, llamar `UploadChunk` a `ftpBaseUrl + nombreRemoto.partNNN`
5. Eliminar ZIP temporal
6. Reportar progreso vía `IProgress<string>`

### 2.3 `BackupEngine.cs` — RF3

```csharp
namespace winbackup
{
    public class BackupEngine : IBackupEngine
    {
        private readonly IDbfValidator _dbfValidator;
        private readonly IFtpUploadService _ftpService;
        private readonly ILogService _log;
        private readonly FileSnapshotCache _snapshot;

        public event Action<BackupResult> OnArchivoProcesado;
        public event Action<BackupResult> OnBackupCompleto;

        public BackupEngine(IDbfValidator dbfVal, IFtpUploadService ftp, ILogService log) { ... }

        public async Task<BackupResult> EjecutarBackupCompleto() { ... }
        public async Task<BackupResult> CompararYRespaldar() { ... }
        public async Task<BackupResult> ProcesarArchivo(string ruta) { ... }
        public Dictionary<string, DateTime> ObtenerSnapshot() { ... }
    }
}
```

**Algoritmo `EjecutarBackupCompleto`:**
1. Para cada `RutaBackupConfig` en `GlobalData.Config.Rutas`:
   a. Enumerar archivos con `Directory.EnumerateFiles(origen, patron, subcarpetas)`
   b. Para cada archivo:
      - Si es DBF → `DbfValidator.Validar()` — si inválido, log + omitir
      - Si no es DBF → pasar directo
      - `FtpUploadService.SubirArchivo()`
      - Actualizar `_snapshot`
      - Disparar `OnArchivoProcesado`
   c. Disparar `OnBackupCompleto`

**Algoritmo `CompararYRespaldar`:**
1. Tomar snapshot actual de todos los archivos
2. Comparar con `_snapshot` anterior
3. Solo procesar: archivos nuevos (no estaban en snapshot) + archivos modificados (`LastWriteTime` distinto)
4. Actualizar `_snapshot` al final
5. Log de archivos sin cambios (omitidos)

### 2.4 `FileSnapshotCache.cs`

```csharp
namespace winbackup
{
    public class FileSnapshotCache
    {
        private Dictionary<string, DateTime> _cache = new();

        public void Actualizar(string ruta, DateTime ultimaModificacion) { ... }
        public bool HaCambiado(string ruta, DateTime ultimaModificacion) { ... }
        public Dictionary<string, DateTime> ObtenerSnapshot() { ... }
        public void Remover(string ruta) { ... }
        public void Reiniciar() { ... }
    }
}
```

### 2.5 `FileSystemMonitor.cs` — RF4

```csharp
namespace winbackup
{
    public class FileSystemMonitor : IFileSystemMonitor
    {
        private FileSystemWatcher _watcher;
        private readonly int _debounceMs;
        private CancellationTokenSource _debounceCts;

        public event Action<string> OnArchivoCambiado;
        public event Action<string> OnArchivoCreado;
        public event Action<string> OnArchivoEliminado;

        public FileSystemMonitor(int debounceMs)
        {
            _debounceMs = debounceMs;
        }

        public void Iniciar(string ruta, string filtro = "*.*", bool incluirSubcarpetas = true)
        {
            _watcher = new FileSystemWatcher(ruta, filtro);
            _watcher.IncludeSubdirectories = incluirSubcarpetas;
            _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            _watcher.Created += (s, e) => AplicarDebounce(e.FullPath, OnArchivoCreado);
            _watcher.Changed += (s, e) => AplicarDebounce(e.FullPath, OnArchivoCambiado);
            _watcher.Deleted += (s, e) => AplicarDebounce(e.FullPath, OnArchivoEliminado);
            _watcher.Renamed += (s, e) => AplicarDebounce(e.FullPath, OnArchivoCambiado);
            _watcher.EnableRaisingEvents = true;
        }

        public void Detener()
        {
            _watcher?.Dispose();
        }

        public bool EstaActivo => _watcher?.EnableRaisingEvents ?? false;

        private void AplicarDebounce(string ruta, Action<string> evento)
        {
            // Cancela debounce anterior para esta ruta
            // Espera _debounceMs antes de disparar el evento
            // Evita duplicados cuando un mismo archivo genera Created+Changed
        }
    }
}
```

### 2.6 `BackupScheduler.cs` — RF4

```csharp
namespace winbackup
{
    public class BackupScheduler : IBackupScheduler
    {
        private System.Timers.Timer _timer;

        public event Action OnTick;

        public void Iniciar(int intervaloSegundos)
        {
            _timer = new System.Timers.Timer(intervaloSegundos * 1000);
            _timer.AutoReset = true;
            _timer.Elapsed += (s, e) => OnTick?.Invoke();
            _timer.Start();
        }

        public void Detener()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }

        public bool EstaActivo => _timer?.Enabled ?? false;
    }
}
```

### 2.7 `LogService.cs`

```csharp
namespace winbackup
{
    public class LogService : ILogService
    {
        private readonly ListBox _listBox;   // referencia a lstRegistro
        private readonly string _rutaArchivoLog;
        private readonly List<string> _historial = new();
        private readonly object _lock = new();

        public LogService(ListBox listBox = null)
        {
            _listBox = listBox;
            _rutaArchivoLog = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "logs",
                $"backup_{DateTime.Now:yyyyMMdd}.log"
            );
            Directory.CreateDirectory(Path.GetDirectoryName(_rutaArchivoLog));
        }

        public void Info(string msg) => Escribir("INFO", msg);
        public void Exito(string msg) => Escribir("OK", msg);
        public void Error(string msg) => Escribir("ERROR", msg);
        public void Advertencia(string msg) => Escribir("WARN", msg);

        private void Escribir(string nivel, string mensaje)
        {
            string linea = $"[{DateTime.Now:HH:mm:ss}] {nivel}: {mensaje}";
            lock (_lock)
            {
                _historial.Add(linea);
                File.AppendAllText(_rutaArchivoLog, linea + Environment.NewLine);
            }
            // Actualizar ListBox en el hilo de UI
            if (_listBox != null && !_listBox.IsDisposed)
            {
                _listBox.Invoke(() =>
                {
                    _listBox.Items.Insert(0, linea);
                    if (_listBox.Items.Count > 500) _listBox.Items.RemoveAt(_listBox.Items.Count - 1);
                });
            }
        }

        public List<string> ObtenerHistorial() { ... }
        public void Limpiar() { ... }
    }
}
```

---

## 3. Modelos (se agregan a `clconfiguracion.cs`)

### Nuevas secciones en `clconfiguracion`

```csharp
public class clconfiguracion
{
    [JsonPropertyName("CREDENCIALES")]
    public CredencialesConfig Credenciales { get; set; }

    [JsonPropertyName("BACKUPS")]
    public BackupsConfig Backups { get; set; }

    // ===== NUEVAS SECCIONES =====

    [JsonPropertyName("DBF_CONFIG")]
    public DbfConfig Dbf { get; set; }

    [JsonPropertyName("RUTAS")]
    public RutasConfig Rutas { get; set; }

    [JsonPropertyName("SINCRONIZACION")]
    public SincronizacionConfig Sincronizacion { get; set; }
}

public class DbfConfig
{
    [JsonPropertyName("habilitado")]
    public bool Habilitado { get; set; }

    [JsonPropertyName("extensiones")]
    public List<string> Extensiones { get; set; }

    [JsonPropertyName("tamanoMinimoBytes")]
    public long TamanoMinimoBytes { get; set; }

    [JsonPropertyName("validarCabecera")]
    public bool ValidarCabecera { get; set; }
}

public class RutaBackupConfig
{
    [JsonPropertyName("origen")]
    public string Origen { get; set; }

    [JsonPropertyName("destino")]
    public string Destino { get; set; }

    [JsonPropertyName("patron")]
    public string Patron { get; set; }

    [JsonPropertyName("incluirSubcarpetas")]
    public bool IncluirSubcarpetas { get; set; }

    [JsonPropertyName("excluir")]
    public List<string> Excluir { get; set; }
}

public class RutasConfig
{
    [JsonPropertyName("carpetas")]
    public List<RutaBackupConfig> Carpetas { get; set; }
}

public class SincronizacionConfig
{
    [JsonPropertyName("habilitado")]
    public bool Habilitado { get; set; }

    [JsonPropertyName("intervaloSegundos")]
    public int IntervaloSegundos { get; set; }

    [JsonPropertyName("debounceMs")]
    public int DebounceMs { get; set; }

    [JsonPropertyName("soloSiHayCambios")]
    public bool SoloSiHayCambios { get; set; }
}
```

---

## 4. `config.json` final

```json
{
  "CREDENCIALES": {
    "user": "userexamen@ylh.ywu.temporary.site",
    "pass": "CRmBnDjy5zu0WDkfZVjcOUgdnATHDbNxOQAtCAUfCJc=",
    "ftpBaseUrl": "ftp://162.241.194.172/"
  },
  "BACKUPS": {
    "backups": "4"
  },
  "DBF_CONFIG": {
    "habilitado": true,
    "extensiones": [".dbf", ".DBF"],
    "tamanoMinimoBytes": 512,
    "validarCabecera": true
  },
  "RUTAS": {
    "carpetas": [
      {
        "origen": "C:\\Backup\\Fuentes",
        "destino": "respaldos/contabilidad",
        "patron": "*.*",
        "incluirSubcarpetas": true,
        "excluir": [".tmp", ".log"]
      },
      {
        "origen": "D:\\Sistema\\Data",
        "destino": "respaldos/dbf",
        "patron": "*.dbf",
        "incluirSubcarpetas": false,
        "excluir": []
      }
    ]
  },
  "SINCRONIZACION": {
    "habilitado": true,
    "intervaloSegundos": 300,
    "debounceMs": 1500,
    "soloSiHayCambios": true
  }
}
```

---

## 5. Cambios en `Form1.cs`

### 5.1 Nuevos campos (inyección manual)

```csharp
private IDbfValidator _dbfValidator;
private IFtpUploadService _ftpService;
private IBackupEngine _backupEngine;
private IFileSystemMonitor _monitor;
private IBackupScheduler _scheduler;
private ILogService _log;
```

### 5.2 `Form1_Load` modificado

```csharp
private void Form1_Load(object sender, EventArgs e)
{
    try
    {
        GlobalData.Config = clconfiguracion.Cargar("config.json");

        _dbfValidator = new DbfValidator();
        _ftpService = new FtpUploadService();
        _log = new LogService(lstRegistro);
        _backupEngine = new BackupEngine(_dbfValidator, _ftpService, _log);
        _backupEngine.OnArchivoProcesado += BackupEngine_OnArchivoProcesado;
        _backupEngine.OnBackupCompleto += BackupEngine_OnBackupCompleto;

        if (GlobalData.Config.Sincronizacion.Habilitado)
        {
            _scheduler = new BackupScheduler();
            _scheduler.OnTick += Scheduler_OnTick;
            _scheduler.Iniciar(GlobalData.Config.Sincronizacion.IntervaloSegundos);

            foreach (var ruta in GlobalData.Config.Rutas.Carpetas)
            {
                if (Directory.Exists(ruta.Origen))
                {
                    var monitor = new FileSystemMonitor(GlobalData.Config.Sincronizacion.DebounceMs);
                    monitor.OnArchivoCambiado += m => _backupEngine.ProcesarArchivo(m);
                    monitor.OnArchivoCreado += m => _backupEngine.ProcesarArchivo(m);
                    monitor.OnArchivoEliminado += m => _log.Info($"Archivo eliminado: {m}");
                    monitor.Iniciar(ruta.Origen, ruta.Patron, ruta.IncluirSubcarpetas);
                    _log.Info($"Monitor iniciado: {ruta.Origen}");
                }
            }
        }

        _log.Info("Aplicación iniciada correctamente");
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error fatal: " + ex.Message);
    }
}
```

### 5.3 Handlers de menú

```csharp
private void generarAhoraToolStripMenuItem_Click(object sender, EventArgs e)
{
    _ = _backupEngine.EjecutarBackupCompleto();
}

private void comprobarCambiosToolStripMenuItem_Click(object sender, EventArgs e)
{
    _ = _backupEngine.CompararYRespaldar();
}
```

### 5.4 `Form1_FormClosing` — detener servicios

```csharp
private void Form1_FormClosing(object sender, FormClosingEventArgs e)
{
    if (e.CloseReason == CloseReason.UserClosing)
    {
        e.Cancel = true;
        this.Hide();
        return;
    }
    _scheduler?.Detener();
    _monitor?.Detener();
}
```

---

## 6. `Form1.Designer.cs` — eventos que deben asignarse

Los siguientes items del menú no tienen handler asignado. Deben enlazarse en `InitializeComponent()`:

```csharp
// --- Menú "Copia Seguridad" ---
generarAhoraToolStripMenuItem.Click += generarAhoraToolStripMenuItem_Click;
comprobarCambiosToolStripMenuItem.Click += comprobarCambiosToolStripMenuItem_Click;
```

---

## 7. Asignación por RF

| RF | Clase nueva | Interfaces | Archivos modificados |
|----|-------------|------------|----------------------|
| **RF2** | `DbfValidator.cs` | `IDbfValidator` | `clconfiguracion.cs` (nuevos modelos), `config.json` (sección DBF), `BackupEngine.cs` (usa el validador) |
| **RF3** | `BackupEngine.cs`, `FtpUploadService.cs`, `LogService.cs`, `FileSnapshotCache.cs` | `IBackupEngine`, `IFtpUploadService`, `ILogService` | `clconfiguracion.cs` (modelo Rutas), `config.json` (sección RUTAS), `Form1.cs` (inyectar engine, conectar menús) |
| **RF4** | `FileSystemMonitor.cs`, `BackupScheduler.cs` | `IFileSystemMonitor`, `IBackupScheduler` | `clconfiguracion.cs` (modelo Sincronizacion), `config.json` (sección SINCRONIZACION), `Form1.cs` (arrancar/parar en Load/Close) |

---

## 8. Árbol de archivos final

```
winbackup/
├── Program.cs
├── Form1.cs              ← modificado (inyección, handlers)
├── Form1.Designer.cs     ← modificado (eventos de menú)
├── clconfiguracion.cs    ← modificado (nuevos modelos)
├── clseguridad.cs
├── GlobalData.cs
├── config.json           ← modificado (3 nuevas secciones)
├── winbackup.csproj
│
├── Interfaces/
│   ├── IDbfValidator.cs      (nuevo)
│   ├── IFtpUploadService.cs  (nuevo)
│   ├── IBackupEngine.cs      (nuevo)
│   ├── IFileSystemMonitor.cs (nuevo)
│   ├── IBackupScheduler.cs   (nuevo)
│   └── ILogService.cs        (nuevo)
│
├── Services/
│   ├── DbfValidator.cs       (nuevo)
│   ├── FtpUploadService.cs   (nuevo)
│   ├── BackupEngine.cs       (nuevo)
│   ├── FileSystemMonitor.cs  (nuevo)
│   ├── BackupScheduler.cs    (nuevo)
│   ├── LogService.cs         (nuevo)
│   └── FileSnapshotCache.cs  (nuevo)
│
└── Properties/
```

---

## 9. Diagrama de secuencia — Backup completo

```
Form1                         BackupEngine              DbfValidator    FtpUploadService    FileSnapshotCache
  │                              │                          │                 │                   │
  │─ generarAhora_Click()        │                          │                 │                   │
  │───► EjecutarBackupCompleto() │                          │                 │                   │
  │                              │                          │                 │                   │
  │    for each ruta in Rutas:   │                          │                 │                   │
  │    for each archivo:         │                          │                 │                   │
  │                              │──► Validar(ruta) ───────►│                 │                   │
  │                              │◄── (bool, razon) ────────│                 │                   │
  │                              │                          │                 │                   │
  │    [if es DBF y es inválido] │                          │                 │                   │
  │    log.Warn + continue       │                          │                 │                   │
  │                              │                          │                 │                   │
  │    [if HaCambiado(ruta)]     │                          │                 │                   │
  │                              │──► SubirArchivo() ───────│──► FTP ───────►│                   │
  │                              │◄── true/false ───────────│◄───────────────│                   │
  │                              │                          │                 │                   │
  │                              │──► Actualizar(ruta, dt) ─│────────────────│──► actualiza cache │
  │                              │                          │                 │                   │
  │    dispatchea OnArchivoProcesado(result)                │                 │                   │
  │◄── actualiza lstCopias ──────│                          │                 │                   │
  │                              │                          │                 │                   │
  │    dispatchea OnBackupCompleto(result)                  │                 │                   │
  │◄── actualiza UI ─────────────│                          │                 │                   │
```

---

## 10. Resumen de principios de diseño

| Principio | Aplicación |
|-----------|-----------|
| **Interface Segregation** | Cada servicio tiene su propia interfaz pequeña y enfocada |
| **Dependency Injection (manual)** | Form1 recibe las dependencias en el constructor o las instancia en Load |
| **Single Responsibility** | BackupEngine orquesta, FtpUploadService sube, DbfValidator valida, LogService registra |
| **Observer** | Eventos `OnArchivoProcesado`, `OnBackupCompleto`, `OnTick` para desacoplar UI de lógica |
| **Debounce** | FileSystemMonitor agrupa eventos rápidos para evitar backups múltiples del mismo archivo |
| **Snapshot diff** | FileSnapshotCache almacena `{ruta → LastWriteTime}` para evitar respaldar archivos sin cambios |
