# Pruebas Manuales — RF4 Sincronización Dinámica

Componentes bajo prueba:

| Componente | Archivo | Rol |
|-----------|---------|-----|
| `FileSnapshotCache` | `FileSnapshotCache.cs` | Cache `{ruta → LastWriteTime}`, método `CompararYDiferenciar()` |
| `FileSystemMonitor` | `FileSystemMonitor.cs` | `FileSystemWatcher` con debounce de 1.5s |
| `BackupScheduler` | `BackupScheduler.cs` | Timer periódico que ejecuta `Scheduler_OnTick` |
| Integración en Form1 | `Form1.cs` | Conecta scheduler + monitor + cache |

---

## Preparación del entorno

### 1. Configurar `config.json` para pruebas

```json
{
  "CREDENCIALES": { "user": "...", "pass": "...", "ftpBaseUrl": "ftp://..." },
  "BACKUPS": { "backups": "4" },
  "RUTAS": {
    "carpetas": [
      {
        "origen": "C:\\Temp\\RF4\\Fuentes",
        "destino": "respaldos/prueba",
        "patron": "*.*",
        "incluirSubcarpetas": true,
        "excluir": []
      }
    ]
  },
  "SINCRONIZACION": {
    "habilitado": true,
    "intervaloSegundos": 30,
    "debounceMs": 1500,
    "soloSiHayCambios": true
  }
}
```

> ⚠ Reducir `intervaloSegundos` a 30 para que las pruebas no demoren 5 min.

### 2. Preparar carpeta de prueba

```powershell
# Setup inicial una sola vez
Remove-Item -Path "C:\Temp\RF4" -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path "C:\Temp\RF4\Fuentes" -Force | Out-Null

# Archivo inicial (existirá cuando la app arranque)
"linea inicial" | Out-File -FilePath "C:\Temp\RF4\Fuentes\base.txt" -Encoding ascii
```

### 3. Iniciar la aplicación

1. Compilar y ejecutar `winbackup` desde Visual Studio.
2. Verificar en el `lstRegistro` (ListBox inferior) el mensaje:
   ```
   [HH:mm:ss] Sincronización iniciada cada 30s
   ```
3. La app inicia y toma un snapshot inicial de `C:\Temp\RF4\Fuentes`.

---

## Casos de prueba — Timer (snapshot por comparación)

Estos casos prueban el flujo `BackupScheduler.OnTick → FileSnapshotCache.CompararYDiferenciar()`.

---

### CP4-01: Archivo nuevo detectado por timer

**Objetivo:** Verificar que al crear un archivo nuevo, el próximo tick del timer lo detecta como "+1 nuevos".

**Precondiciones:**
- App corriendo, `intervaloSegundos = 30`.
- Snapshot inicial ya tomado (solo existe `base.txt`).

**Pasos:**
1. Anotar la hora actual.
2. Crear un archivo nuevo:
   ```powershell
   "contenido nuevo" | Out-File -FilePath "C:\Temp\RF4\Fuentes\nuevo.txt" -Encoding ascii
   ```
3. Esperar hasta el próximo tick del timer (máximo 30s).

**Resultado esperado en `lstRegistro`:**
```
[HH:mm:ss] Cambios detectados: +1 nuevos
```

**Verificación adicional:**
- El snapshot interno se actualizó: al próximo tick ya no debe reportar cambios (si no se crean más archivos).

---

### CP4-02: Archivo modificado detectado por timer

**Objetivo:** Verificar que al modificar un archivo existente, el próximo tick lo detecta como "~1 modificados".

**Precondiciones:**
- App corriendo, snapshot contiene `base.txt`.
- No hay cambios pendientes.

**Pasos:**
1. Modificar `base.txt` (cambiar contenido → cambia `LastWriteTime`):
   ```powershell
   "contenido modificado" | Out-File -FilePath "C:\Temp\RF4\Fuentes\base.txt" -Encoding ascii
   ```
2. Esperar el próximo tick del timer.

**Resultado esperado:**
```
[HH:mm:ss] Cambios detectados: ~1 modificados
```

---

### CP4-03: Archivo eliminado detectado por timer

**Objetivo:** Verificar que al eliminar un archivo, el próximo tick lo detecta como "-1 eliminados".

**Precondiciones:**
- App corriendo, snapshot contiene `base.txt` y `nuevo.txt`.

**Pasos:**
1. Eliminar `nuevo.txt`:
   ```powershell
   Remove-Item -Path "C:\Temp\RF4\Fuentes\nuevo.txt" -Force
   ```
2. Esperar el próximo tick del timer.

**Resultado esperado:**
```
[HH:mm:ss] Cambios detectados: -1 eliminados
```

---

### CP4-04: Múltiples cambios simultáneos

**Objetivo:** Verificar que el timer detecta nuevos + modificados + eliminados en un solo tick.

**Precondiciones:**
- App corriendo, snapshot contiene solo `base.txt`.

**Pasos:**
1. Ejecutar todos estos cambios rápidamente:
   ```powershell
   # Nuevo
   "archivo a" | Out-File -FilePath "C:\Temp\RF4\Fuentes\a.txt" -Encoding ascii
   "archivo b" | Out-File -FilePath "C:\Temp\RF4\Fuentes\b.txt" -Encoding ascii

   # Modificado
   "contenido cambiado" | Out-File -FilePath "C:\Temp\RF4\Fuentes\base.txt" -Encoding ascii

   # Eliminado (ninguno, para que haya de los 3 tipos creamos uno y lo borramos)
   "temporal" | Out-File -FilePath "C:\Temp\RF4\Fuentes\temp.docx" -Encoding ascii
   Start-Sleep 1
   Remove-Item -Path "C:\Temp\RF4\Fuentes\temp.docx" -Force
   ```
2. Esperar el próximo tick.

**Resultado esperado:**
```
[HH:mm:ss] Cambios detectados: +2 nuevos, ~1 modificados, -1 eliminados
```

---

### CP4-05: Sin cambios (ejecución repetida)

**Objetivo:** Verificar que cuando no hay cambios, el timer NO escribe nada (no hay mensaje de cambios).

**Precondiciones:**
- App corriendo, snapshot actualizado tras el último cambio.
- No se ha creado, modificado ni eliminado ningún archivo.

**Pasos:**
1. Esperar 2 ticks completos del timer sin tocar la carpeta.

**Resultado esperado:**
- `lstRegistro` **NO** muestra ningún mensaje de "Cambios detectados".
- La app sigue funcionando sin errores.

**Verificación adicional:**
- Se puede confirmar inyectando `Console.WriteLine` o revisando que no hay excepciones silenciosas.

---

### CP4-06: Archivo excluido por extensión no se detecta

**Objetivo:** Verificar que archivos con extensión excluida no son trackeados.

**Precondiciones:**
- Config: `"excluir": [".tmp", ".log"]`.
- App corriendo, snapshot inicial tomado.

**Pasos:**
1. Crear archivos con extensión excluida:
   ```powershell
   "log temporal" | Out-File -FilePath "C:\Temp\RF4\Fuentes\debug.log" -Encoding ascii
   "temp data" | Out-File -FilePath "C:\Temp\RF4\Fuentes\cache.tmp" -Encoding ascii
   ```
2. Esperar el próximo tick del timer.

**Resultado esperado:**
- No hay mensaje de cambios (los archivos excluidos no se enumeran → no entran al snapshot → no generan diff).

---

## Casos de prueba — FileSystemWatcher (tiempo real)

Estos casos prueban el flujo `FileSystemMonitor → evento OnCambioDetectado → Monitor_OnCambioDetectado`.

---

### CP4-07: Archivo nuevo detectado por watcher

**Objetivo:** Verificar que crear un archivo genera evento `+` en el log.

**Precondiciones:**
- App corriendo, `SINCRONIZACION.habilitado = true`, watchers activos por ruta.

**Pasos:**
1. Anotar hora.
2. Crear archivo:
   ```powershell
   "nuevo watcher" | Out-File -FilePath "C:\Temp\RF4\Fuentes\watcher_test.txt" -Encoding ascii
   ```
3. Esperar ~1.5s (debounce).

**Resultado esperado en `lstRegistro`:**
```
[HH:mm:ss] + watcher_test.txt
```

---

### CP4-08: Archivo modificado detectado por watcher

**Pasos:**
1. Modificar `watcher_test.txt`:
   ```powershell
   "contenido actualizado" | Out-File -FilePath "C:\Temp\RF4\Fuentes\watcher_test.txt" -Encoding ascii
   ```
2. Esperar ~1.5s.

**Resultado esperado:**
```
[HH:mm:ss] ~ watcher_test.txt
```

---

### CP4-09: Archivo eliminado detectado por watcher

**Pasos:**
1. Eliminar `watcher_test.txt`:
   ```powershell
   Remove-Item -Path "C:\Temp\RF4\Fuentes\watcher_test.txt" -Force
   ```
2. Esperar ~0.5s (eliminado no tiene debounce).

**Resultado esperado:**
```
[HH:mm:ss] - watcher_test.txt
```

---

### CP4-10: Debounce agrupa eventos rápidos

**Objetivo:** Verificar que múltiples writes rápidos a un mismo archivo solo generan **un** evento.

**Precondiciones:**
- App corriendo.

**Pasos:**
1. Ejecutar writes rápidos en un loop:
   ```powershell
   1..5 | ForEach-Object {
       "escritura $_" | Out-File -FilePath "C:\Temp\RF4\Fuentes\debounce_test.txt" -Encoding ascii
       Start-Sleep -Milliseconds 200
   }
   ```
2. Esperar ~2s.

**Resultado esperado:**
- Solo aparece **una** línea en el log:
  ```
  [HH:mm:ss] ~ debounce_test.txt
  ```
- No hay 5 líneas repetidas.

---

### CP4-11: Watcher no se activa con archivos excluidos

**Objetivo:** Verificar que el watcher ignora archivos con extensión excluida.

**Precondiciones:**
- Config: `"excluir": [".log"]`.
- App corriendo con watchers activos.

**Pasos:**
1. Crear archivo con extensión excluida:
   ```powershell
   "info log" | Out-File -FilePath "C:\Temp\RF4\Fuentes\app.log" -Encoding ascii
   ```
2. Esperar ~2s.

**Resultado esperado:**
- `lstRegistro` **SÍ** muestra `+ app.log` (el watcher ve todos los archivos a nivel de sistema, la exclusión es del `FileScannerService`).
- El snapshot NO incluye `app.log` (porque `FileScannerService` lo excluye).

> ⚠ Diferencia importante: `FileSystemWatcher` captura **todos** los eventos del sistema de archivos; el filtro por extensión ocurre después en `FileScannerService`. El log del watcher muestra la creación, pero el timer no la considerará un cambio porque `EscaneoRuta()` no devuelve ese archivo.

---

## Casos de prueba — Borde y esquina

---

### CP4-12: Directorio raíz no existe

**Objetivo:** Verificar que si una ruta configurada no existe al iniciar, no lanza excepción.

**Config:** Cambiar `origen` a `"C:\\Temp\\RF4\\NoExiste"`.

**Pasos:**
1. No crear la carpeta.
2. Iniciar la aplicación.

**Resultado esperado:**
- App inicia sin error.
- `lstRegistro` muestra: `Sincronización iniciada cada 30s`.
- `FileSystemMonitor.Iniciar()` retorna silenciosamente porque `Directory.Exists(ruta) == false`.

---

### CP4-13: `SINCRONIZACION.habilitado = false`

**Objetivo:** Verificar que con sincronización deshabilitada no se inician ni scheduler ni watchers.

**Config:** `"habilitado": false`.

**Pasos:**
1. Iniciar la aplicación.
2. Crear archivo en carpeta de prueba.
3. Esperar 30s.

**Resultado esperado:**
- `lstRegistro` NO muestra "Sincronización iniciada".
- No hay mensajes de cambios (ni timer ni watcher están activos).
- Al hacer clic en menú **Comprobar Cambios** sí funciona (es manual).

---

### CP4-14: Menú "Comprobar Cambios" manual

**Objetivo:** Verificar que el menú ejecuta el mismo flujo que el timer.

**Precondiciones:**
- App corriendo (con o sin scheduler).

**Pasos:**
1. Crear archivo nuevo:
   ```powershell
   "manual test" | Out-File -FilePath "C:\Temp\RF4\Fuentes\manual.txt" -Encoding ascii
   ```
2. Ir al menú **Copia Seguridad → Comprobar Cambios**.
3. Hacer clic.

**Resultado esperado:**
- Aparece MessageBox: "Verificación completada."
- `lstRegistro` muestra:
  ```
  [HH:mm:ss] Cambios detectados: +1 nuevos
  ```

---

### CP4-15: Renombrar archivo

**Objetivo:** Verificar que el watcher detecta renombrados.

**Pasos:**
1. Renombrar archivo:
   ```powershell
   Rename-Item -Path "C:\Temp\RF4\Fuentes\base.txt" -NewName "base_renombrado.txt"
   ```
2. Esperar ~1.5s.

**Resultado esperado en `lstRegistro`:**
```
[HH:mm:ss] > base_renombrado.txt
```

---

### CP4-16: Cerrar aplicación desde menú "Salir"

**Objetivo:** Verificar que al cerrar realmente (no minimizar), los servicios se detienen limpiamente.

**Pasos:**
1. Abrir aplicación.
2. Ir a **Copia Seguridad → Salir** (el `salirToolStripMenuItem1` llama a `Application.Exit()`).
3. Verificar que el proceso termina en el Administrador de Tareas.

**Resultado esperado:**
- El proceso `winbackup.exe` desaparece del Administrador de Tareas.
- No quedan hilos colgados (timer detenido, watchers disposed).

---

## Checklist QA — RF4

| # | Ítem | Método | Estado | Obs. |
|---|------|--------|--------|------|
| | **Timer (snapshot)** | | | |
| 1 | Archivo nuevo → `+1 nuevos` | CP4-01 | ☐ | |
| 2 | Archivo modificado → `~1 modificados` | CP4-02 | ☐ | |
| 3 | Archivo eliminado → `-1 eliminados` | CP4-03 | ☐ | |
| 4 | Múltiples cambios combinados | CP4-04 | ☐ | |
| 5 | Sin cambios → sin mensaje | CP4-05 | ☐ | |
| 6 | Archivo con extensión excluida ignorado | CP4-06 | ☐ | |
| | **Watcher (tiempo real)** | | | |
| 7 | Creación → `+ nombre` | CP4-07 | ☐ | |
| 8 | Modificación → `~ nombre` | CP4-08 | ☐ | |
| 9 | Eliminación → `- nombre` | CP4-09 | ☐ | |
| 10 | Debounce: eventos rápidos = 1 sola línea | CP4-10 | ☐ | |
| 11 | Watcher + exclusión (diferencia con timer) | CP4-11 | ☐ | |
| 12 | Renombrado → `> nombre` | CP4-15 | ☐ | |
| | **Borde / Config** | | | |
| 13 | Ruta inexistente al iniciar → sin error | CP4-12 | ☐ | |
| 14 | `habilitado = false` → nada se inicia | CP4-13 | ☐ | |
| 15 | Menú "Comprobar Cambios" manual funciona | CP4-14 | ☐ | |
| 16 | Cierre de app detiene servicios sin leaks | CP4-16 | ☐ | |
| | **Snapshot Cache (unidad)** | | | |
| 17 | `Actualizar()` + `HaCambiado()` → true/false | — | ☐ | |
| 18 | `ActualizarDesdeLista()` reemplaza snapshot | — | ☐ | |
| 19 | `Remover()` elimina entrada | — | ☐ | |
| 20 | `Reiniciar()` limpia todo | — | ☐ | |
| 21 | `ObtenerEliminados()` contra snapshot actual | — | ☐ | |

---

## Código helper PowerShell

```powershell
# ---- Setup ----
function New-RF4Env {
    Remove-Item -Path "C:\Temp\RF4" -Recurse -Force -ErrorAction SilentlyContinue
    New-Item -ItemType Directory -Path "C:\Temp\RF4\Fuentes" -Force | Out-Null
    "linea inicial" | Out-File -FilePath "C:\Temp\RF4\Fuentes\base.txt" -Encoding ascii
    Write-Host "✔ RF4: Entorno creado con base.txt"
}

# ---- Nuevo archivo ----
function Add-File($nombre) {
    "contenido $nombre" | Out-File -FilePath "C:\Temp\RF4\Fuentes\$nombre" -Encoding ascii
    Write-Host "✔ Creado: $nombre"
}

# ---- Modificar archivo ----
function Edit-File($nombre) {
    "modificado $(Get-Date -Format HH:mm:ss)" | Out-File -FilePath "C:\Temp\RF4\Fuentes\$nombre" -Encoding ascii
    Write-Host "✔ Modificado: $nombre"
}

# ---- Eliminar archivo ----
function Remove-File($nombre) {
    Remove-Item -Path "C:\Temp\RF4\Fuentes\$nombre" -Force -ErrorAction SilentlyContinue
    Write-Host "✔ Eliminado: $nombre"
}

# ---- Múltiples cambios simultáneos ----
function Invoke-MultiChange {
    Add-File "a.txt"
    Add-File "b.txt"
    Edit-File "base.txt"
    Add-File "temp.docx"
    Start-Sleep 1
    Remove-File "temp.docx"
    Write-Host "✔ Cambios múltiples aplicados (+2, ~1, -1)"
}

# ---- Stress: 50 archivos ----
function Invoke-Stress {
    1..50 | ForEach-Object {
        "contenido $_" | Out-File -FilePath "C:\Temp\RF4\Fuentes\archivo_$_.txt" -Encoding ascii
    }
    Write-Host "✔ 50 archivos creados"
}

# ---- Limpiar ----
function Remove-RF4Env {
    Remove-Item -Path "C:\Temp\RF4" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "✔ RF4: Entorno limpiado"
}

# ---- Ver snapshot actual (solo consola) ----
function Show-Snapshot {
    Get-ChildItem -Path "C:\Temp\RF4\Fuentes" -Recurse -File | Select-Object FullName, LastWriteTime
}
```
