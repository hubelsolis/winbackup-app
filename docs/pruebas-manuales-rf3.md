# Pruebas Manuales — RF3 Backup de archivos locales

Clase bajo prueba: `FileScannerService`  
Interfaz: `IFileScannerService`  
Métodos: `EscanearTodo()` y `EscanearRuta(RutaBackupConfig)`

---

## Preparación del entorno de pruebas

1. Abrir `winbackup-app` en Visual Studio 2022+.
2. Compilar la solución: `Build > Build Solution` (Ctrl+Shift+B).
3. Asegurarse de que la aplicación **no esté ejecutándose** (revisar bandeja del sistema).
4. El archivo `config.json` debe estar presente en `winbackup\bin\Debug\net8.0-windows\` (se copia automáticamente al compilar).
5. Crear las carpetas temporales indicadas en cada caso.

---

## Configuración de prueba base

Editar `config.json` según cada caso. Los valores sombreados son los que cambian por escenario.

```json
{
  "CREDENCIALES": { /* sin cambios */ },
  "BACKUPS": { "backups": "4" },
  "RUTAS": {
    "carpetas": [
      {
        "origen": "C:\\Temp\\RF3\\Fuentes",
        "destino": "respaldos/contabilidad",
        "patron": "*.*",
        "incluirSubcarpetas": true,
        "excluir": [".tmp", ".log"]
      }
    ]
  }
}
```

---

## Casos de prueba

### CP-01: Escaneo exitoso — carpeta con archivos variados

**Objetivo:** Verificar que `EscanearTodo()` retorna todos los archivos con metadatos correctos.

**Precondiciones:**
```powershell
# Crear estructura
New-Item -ItemType Directory -Path "C:\Temp\RF3\Fuentes" -Force
New-Item -ItemType Directory -Path "C:\Temp\RF3\Fuentes\Sub" -Force

# Crear archivos
"contenido a" | Out-File -FilePath "C:\Temp\RF3\Fuentes\factura.pdf" -Encoding ascii
"contenido b" | Out-File -FilePath "C:\Temp\RF3\Fuentes\nota.txt" -Encoding ascii
"contenido c" | Out-File -FilePath "C:\Temp\RF3\Fuentes\Sub\datos.csv" -Encoding ascii
```

**Pasos:**
1. Colocar el `config.json` con `origen: "C:\\Temp\\RF3\\Fuentes"`, `patron: "*.*"`, `incluirSubcarpetas: true`.
2. Abrir `Packages Manager Console` y ejecutar:
```powershell
cd winbackup.Tests
dotnet test --filter "FileScannerServiceTests_CP01"
```
O bien ejecutar la prueba manual con código de verificación:
```csharp
var scanner = new FileScannerService();
var resultado = scanner.EscanearTodo();
// Validar count = 3
// Validar que factura.pdf, nota.txt y Sub\datos.csv existen
// Validar TamañoBytes > 0 en cada uno
```

**Resultado esperado:**
- `resultado.Count == 3`
- `resultado[0].Nombre == "factura.pdf"`
- `resultado[1].RutaRelativa == "nota.txt"`
- `resultado[2].RutaRelativa == "Sub\datos.csv"`
- `resultado.All(r => r.EsAccesible == true)`
- `resultado.All(r => r.TamañoBytes > 0)`

**Limpieza:**
```powershell
Remove-Item -Path "C:\Temp\RF3" -Recurse -Force
```

---

### CP-02: Carpeta con un solo archivo

**Objetivo:** Verificar que retorna 1 resultado.

**Precondiciones:**
```powershell
New-Item -ItemType Directory -Path "C:\Temp\RF3\Unica" -Force
"unico archivo" | Out-File -FilePath "C:\Temp\RF3\Unica\reporte.docx" -Encoding ascii
```

**Config:** `origen: "C:\\Temp\\RF3\\Unica"`, `patron: "*.*"`, `incluirSubcarpetas: false`.

**Verificación:**
- `resultado.Count == 1`
- `resultado[0].Nombre == "reporte.docx"`
- `resultado[0].RutaRelativa == "reporte.docx"`

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

### CP-03: Ruta inexistente

**Objetivo:** Verificar que no lanza excepción, retorna lista vacía.

**Config:** `origen: "C:\\Temp\\RF3\\NoExiste"`.

**Pasos:**
1. NO crear la carpeta.
2. Llamar `EscanearTodo()`.

**Resultado esperado:**
- No lanza `DirectoryNotFoundException`
- `resultado.Count == 0`

---

### CP-04: Carpeta vacía

**Objetivo:** Verificar que retorna lista vacía.

**Precondiciones:**
```powershell
New-Item -ItemType Directory -Path "C:\Temp\RF3\Vacia" -Force
```

**Config:** `origen: "C:\\Temp\\RF3\\Vacia"`.

**Verificación:**
- `resultado.Count == 0`

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

### CP-05: Múltiples carpetas configuradas

**Objetivo:** Verificar que `EscanearTodo()` combina resultados de todas las rutas.

**Precondiciones:**
```powershell
New-Item -ItemType Directory -Path "C:\Temp\RF3\A" -Force
New-Item -ItemType Directory -Path "C:\Temp\RF3\B" -Force
"archivo a1.txt" | Out-File -FilePath "C:\Temp\RF3\A\a1.txt" -Encoding ascii
"archivo b1.txt" | Out-File -FilePath "C:\Temp\RF3\B\b1.txt" -Encoding ascii
```

**Config:**
```json
"RUTAS": {
  "carpetas": [
    { "origen": "C:\\Temp\\RF3\\A", "destino": "dest/a", "patron": "*.*", "incluirSubcarpetas": false, "excluir": [] },
    { "origen": "C:\\Temp\\RF3\\B", "destino": "dest/b", "patron": "*.*", "incluirSubcarpetas": false, "excluir": [] }
  ]
}
```

**Verificación:**
- `resultado.Count == 2`
- `resultado.Any(r => r.CarpetaOrigen == @"C:\Temp\RF3\A")`
- `resultado.Any(r => r.CarpetaOrigen == @"C:\Temp\RF3\B")`
- `resultado.Any(r => r.Nombre == "a1.txt")`
- `resultado.Any(r => r.Nombre == "b1.txt")`

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

### CP-06: Filtro por patrón

**Objetivo:** Verificar que solo retorna archivos que coinciden con el patrón.

**Precondiciones:**
```powershell
New-Item -ItemType Directory -Path "C:\Temp\RF3\Patron" -Force
"texto" | Out-File -FilePath "C:\Temp\RF3\Patron\nota.txt" -Encoding ascii
"datos" | Out-File -FilePath "C:\Temp\RF3\Patron\datos.csv" -Encoding ascii
"pdf" | Out-File -FilePath "C:\Temp\RF3\Patron\informe.pdf" -Encoding ascii
```

**Config:** `origen: "C:\\Temp\\RF3\\Patron"`, `patron: "*.txt"`, `incluirSubcarpetas: false`.

**Verificación:**
- `resultado.Count == 1`
- `resultado[0].Nombre == "nota.txt"`
- `resultado[0].Extension == ".txt"`

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

### CP-07: Exclusión por extensión

**Objetivo:** Verificar que archivos con extensiones en `excluir` se omiten.

**Precondiciones:**
```powershell
New-Item -ItemType Directory -Path "C:\Temp\RF3\Excluir" -Force
"log" | Out-File -FilePath "C:\Temp\RF3\Excluir\debug.log" -Encoding ascii
"temp" | Out-File -FilePath "C:\Temp\RF3\Excluir\temp.tmp" -Encoding ascii
"datos" | Out-File -FilePath "C:\Temp\RF3\Excluir\datos.csv" -Encoding ascii
```

**Config:**
```json
"origen": "C:\\Temp\\RF3\\Excluir",
"patron": "*.*",
"incluirSubcarpetas": false,
"excluir": [".log", ".tmp"]
```

**Verificación:**
- `resultado.Count == 1`
- `resultado[0].Nombre == "datos.csv"`
- `resultado.All(r => r.Extension != ".log" && r.Extension != ".tmp")`

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

### CP-08: Sin exclusión configurada

**Objetivo:** Verificar que si `excluir` es null o vacío, no se filtra nada.

**Precondiciones:**
```powershell
New-Item -ItemType Directory -Path "C:\Temp\RF3\NoExcl" -Force
"log" | Out-File -FilePath "C:\Temp\RF3\NoExcl\debug.log" -Encoding ascii
"datos" | Out-File -FilePath "C:\Temp\RF3\NoExcl\datos.csv" -Encoding ascii
```

**Config:** `"excluir": []`.

**Verificación:**
- `resultado.Count == 2`

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

### CP-09: Sin subcarpetas (TopDirectoryOnly)

**Objetivo:** Verificar que con `incluirSubcarpetas: false` no se recorren subdirectorios.

**Precondiciones:**
```powershell
New-Item -ItemType Directory -Path "C:\Temp\RF3\Top" -Force
New-Item -ItemType Directory -Path "C:\Temp\RF3\Top\Sub" -Force
"raiz.txt" | Out-File -FilePath "C:\Temp\RF3\Top\raiz.txt" -Encoding ascii
"sub.txt" | Out-File -FilePath "C:\Temp\RF3\Top\Sub\sub.txt" -Encoding ascii
```

**Config:** `origen: "C:\\Temp\\RF3\\Top"`, `incluirSubcarpetas: false`.

**Verificación:**
- `resultado.Count == 1`
- `resultado[0].Nombre == "raiz.txt"`

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

### CP-10: Con subcarpetas (AllDirectories)

**Objetivo:** Verificar que con `incluirSubcarpetas: true` se recorren recursivamente.

**Precondiciones:** (misma estructura del CP-09)

**Config:** `origen: "C:\\Temp\\RF3\\Top"`, `incluirSubcarpetas: true`.

**Verificación:**
- `resultado.Count == 2`
- `resultado.Any(r => r.RutaRelativa == "raiz.txt")`
- `resultado.Any(r => r.RutaRelativa == @"Sub\sub.txt")`

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

### CP-11: Archivo bloqueado (en uso por otro proceso)

**Objetivo:** Verificar que un archivo abierto por otro proceso se reporta como `EsAccesible = false`.

**Precondiciones:**
```powershell
New-Item -ItemType Directory -Path "C:\Temp\RF3\Bloqueado" -Force
"datos bloqueados" | Out-File -FilePath "C:\Temp\RF3\Bloqueado\store.fdb" -Encoding ascii
```

**Pasos:**
1. Bloquear el archivo manteniendo un handle abierto:
   ```powershell
   $fs = [System.IO.File]::Open("C:\Temp\RF3\Bloqueado\store.fdb", [System.IO.FileMode]::Open, [System.IO.FileAccess]::ReadWrite, [System.IO.FileShare]::None)
   ```
2. Ejecutar `EscanearTodo()`.
3. Liberar el handle: `$fs.Close()`

**Verificación:**
- `resultado.Count == 1`
- `resultado[0].EsAccesible == false`
- `resultado[0].Error` contiene mensaje de error (sharing violation)

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

### CP-12: Ruta con solo subcarpetas vacías

**Objetivo:** Verificar que no retorna archivos si solo hay subdirectorios.

**Precondiciones:**
```powershell
New-Item -ItemType Directory -Path "C:\Temp\RF3\SoloSub" -Force
New-Item -ItemType Directory -Path "C:\Temp\RF3\SoloSub\V1" -Force
New-Item -ItemType Directory -Path "C:\Temp\RF3\SoloSub\V2" -Force
```

**Config:** `origen: "C:\\Temp\\RF3\\SoloSub"`, `incluirSubcarpetas: false`.

**Verificación:**
- `resultado.Count == 0`

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

### CP-13: `GlobalData.Config.Rutas` es null

**Objetivo:** Verificar que no lanza `NullReferenceException`.

**Pasos:**
```csharp
GlobalData.Config = new clconfiguracion { Rutas = null };
var scanner = new FileScannerService();
var resultado = scanner.EscanearTodo();
```

**Verificación:**
- No lanza excepción
- `resultado.Count == 0`

---

### CP-14: `GlobalData.Config` es null

**Objetivo:** Verificar que no lanza excepción si Config nunca se inicializó.

**Pasos:**
```csharp
GlobalData.Config = null;
var scanner = new FileScannerService();
var resultado = scanner.EscanearTodo();
```

**Verificación:**
- No lanza `NullReferenceException`
- `resultado.Count == 0`

---

### CP-15: Archivos con nombres Unicode / espacios

**Objetivo:** Verificar que soporta nombres de archivo con caracteres especiales.

**Precondiciones:**
```powershell
New-Item -ItemType Directory -Path "C:\Temp\RF3\Unicode" -Force
"contenido ñ" | Out-File -FilePath "C:\Temp\RF3\Unicode\factura ñoña 2024.pdf" -Encoding utf8
"contenido esp" | Out-File -FilePath "C:\Temp\RF3\Unicode\mi archivo.txt" -Encoding ascii
```

**Config:** `origen: "C:\\Temp\\RF3\\Unicode"`, `patron: "*.*"`.

**Verificación:**
- `resultado.Count == 2`
- `resultado.Any(r => r.Nombre.Contains("ñoña"))`
- `resultado.Any(r => r.Nombre == "mi archivo.txt")`

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

### CP-16: Archivo de 0 bytes

**Objetivo:** Verificar que un archivo vacío se escanea correctamente.

**Precondiciones:**
```powershell
New-Item -ItemType Directory -Path "C:\Temp\RF3\Vacio" -Force
New-Item -ItemType File -Path "C:\Temp\RF3\Vacio\vacio.txt" -Force
```

**Config:** `origen: "C:\\Temp\\RF3\\Vacio"`.

**Verificación:**
- `resultado.Count == 1`
- `resultado[0].Nombre == "vacio.txt"`
- `resultado[0].TamañoBytes == 0`
- `resultado[0].EsAccesible == true`

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

### CP-17: Una ruta existe y otra no (mixto)

**Objetivo:** Verificar que `EscanearTodo()` procesa las rutas existentes y salta las inexistentes.

**Precondiciones:**
```powershell
New-Item -ItemType Directory -Path "C:\Temp\RF3\Existe" -Force
"presente.txt" | Out-File -FilePath "C:\Temp\RF3\Existe\presente.txt" -Encoding ascii
```

**Config:**
```json
"RUTAS": {
  "carpetas": [
    { "origen": "C:\\Temp\\RF3\\Existe", "destino": "dest/a", "patron": "*.*", "incluirSubcarpetas": false, "excluir": [] },
    { "origen": "C:\\Temp\\RF3\\NoExiste", "destino": "dest/b", "patron": "*.*", "incluirSubcarpetas": false, "excluir": [] }
  ]
}
```

**Verificación:**
- `resultado.Count == 1`
- `resultado[0].Nombre == "presente.txt"`

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

### CP-18: Volumen masivo (stress test)

**Objetivo:** Verificar rendimiento con 500+ archivos.

**Precondiciones:**
```powershell
New-Item -ItemType Directory -Path "C:\Temp\RF3\Stress" -Force
1..500 | ForEach-Object { "contenido $_" | Out-File -FilePath "C:\Temp\RF3\Stress\archivo_$_.txt" -Encoding ascii }
```

**Config:** `origen: "C:\\Temp\\RF3\\Stress"`, `patron: "*.*"`.

**Verificación:**
- `resultado.Count == 500`
- Tiempo de ejecución < 2 segundos

**Limpieza:** `Remove-Item -Path "C:\Temp\RF3" -Recurse -Force`

---

## Checklist QA — RF3

| # | Ítem | Estado | Observaciones |
|---|------|--------|---------------|
| 1 | `EscanearTodo()` retorna archivos de todas las rutas configuradas | ☐ | |
| 2 | `EscanearRuta()` retorna archivos de una ruta específica | ☐ | |
| 3 | Ruta inexistente → lista vacía (sin excepción) | ☐ | |
| 4 | Carpeta vacía → lista vacía | ☐ | |
| 5 | Múltiples carpetas → resultados combinados | ☐ | |
| 6 | Filtro por patrón (`*.txt`) → solo archivos .txt | ☐ | |
| 7 | Exclusión por extensión (`.tmp`, `.log`) → se omiten | ☐ | |
| 8 | Sin exclusiones → no se filtra nada | ☐ | |
| 9 | `incluirSubcarpetas: false` → solo archivos raíz | ☐ | |
| 10 | `incluirSubcarpetas: true` → incluye subcarpetas | ☐ | |
| 11 | Archivo bloqueado → `EsAccesible = false` + `Error` | ☐ | |
| 12 | Solo directorios (sin archivos) → lista vacía | ☐ | |
| 13 | `GlobalData.Config.Rutas = null` → sin error | ☐ | |
| 14 | `GlobalData.Config = null` → sin error | ☐ | |
| 15 | Nombres Unicode/espacios → se escanean correctamente | ☐ | |
| 16 | Archivo 0 bytes → `TamañoBytes == 0`, `EsAccesible == true` | ☐ | |
| 17 | Rutas mixtas (existe + no existe) → procesa las válidas | ☐ | |
| 18 | Stress test 500 archivos → completo en < 2s | ☐ | |
| 19 | `FileScanResult.CarpetaOrigen` coincide con la ruta configurada | ☐ | |
| 20 | `FileScanResult.RutaRelativa` es el path relativo correcto | ☐ | |

---

## Código helper para pruebas rápidas (PowerShell)

```powershell
# Preparar entorno RF3 completo
function New-RF3TestEnv {
    param(
        [string[]]$Archivos = @("factura.pdf", "nota.txt", "datos.csv")
    )
    Remove-Item -Path "C:\Temp\RF3" -Recurse -Force -ErrorAction SilentlyContinue
    New-Item -ItemType Directory -Path "C:\Temp\RF3\Fuentes" -Force | Out-Null
    $Archivos | ForEach-Object {
        "contenido $_" | Out-File -FilePath "C:\Temp\RF3\Fuentes\$_" -Encoding ascii
    }
    Write-Host "✔ Entorno RF3 creado en C:\Temp\RF3\Fuentes con $($Archivos.Count) archivo(s)"
}

# Limpiar entorno RF3
function Remove-RF3TestEnv {
    Remove-Item -Path "C:\Temp\RF3" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "✔ Entorno RF3 limpiado"
}
```
