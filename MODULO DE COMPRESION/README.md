# 🗜️ Módulo de Compresión — Sistema de Backups
# 🗜️ Compression Module — Backup System

---

## 📋 Descripción / Description

**ES:** Módulo de compresión de archivos desarrollado en C# con Windows Forms (.NET 10), que forma parte de un sistema de backups. Permite comprimir y descomprimir archivos usando tres algoritmos: ZIP, RAR y LZMA. Cuenta con una interfaz principal que redirige a cada módulo según el algoritmo seleccionado.

**EN:** File compression module developed in C# with Windows Forms (.NET 10), part of a backup system. It allows compressing and decompressing files using three algorithms: ZIP, RAR, and LZMA. It features a main interface that redirects to each module based on the selected algorithm.

---

## 👥 Integrantes / Team Members

| Integrante | Módulo |
|---|---|
| **Checya** | ZIP — Interfaz Principal |
| **Estela** | RAR |
| **Romero** | LZMA |

---

## 🏗️ Estructura del Proyecto / Project Structure

```
MODULO DE COMPRESION/
├── ModuloCompresion/        → Interfaz principal (menú selector)
├── ZIP/
│   └── zipChecya/           → Módulo ZIP (Checya)
├── RAR/
│   └── WinFormsAppRAR/      → Módulo RAR (Estela)
├── LZMA/
│   └── WinFormsLZMA/        → Módulo LZMA (Romero)
└── zipChecya.Tests/         → Pruebas unitarias del módulo ZIP
```

---

## ⚙️ Requisitos / Requirements

| Herramienta | Versión | Obligatorio |
|---|---|---|
| Visual Studio | 2022 o superior | ✅ |
| .NET SDK | 10.0 | ✅ |
| WinRAR | Cualquiera | ✅ (solo para comprimir RAR) |
| SonarQube | 26.5.0 (Community) | ⚙️ Opcional |

---

## 📦 Paquetes NuGet / NuGet Packages

| Proyecto | Paquete | Versión |
|---|---|---|
| WinFormsAppRAR | SharpCompress | 0.34.2 |
| WinFormsLZMA | SharpCompress | 0.32.2 |
| zipChecya.Tests | MSTest | 4.0.2 |

> **Nota / Note:** Los paquetes se restauran automáticamente al compilar. No es necesario instalarlos manualmente. / Packages are automatically restored on build. No manual installation required.

---

## 🚀 Instalación y Uso / Installation & Usage

### ES:
1. Clona el repositorio
2. Abre `ModuloCompresion.slnx` con Visual Studio
3. Presiona `Ctrl + Shift + B` para compilar toda la solución
4. Presiona `F5` para ejecutar
5. En la interfaz principal selecciona ZIP, RAR o LZMA y presiona **Usar**

### EN:
1. Clone the repository
2. Open `ModuloCompresion.slnx` with Visual Studio
3. Press `Ctrl + Shift + B` to build the entire solution
4. Press `F5` to run
5. In the main interface select ZIP, RAR or LZMA and press **Usar**

---

## 🗜️ Módulos / Modules

### ZIP (Checya)
- **Clase principal:** `CompresorZip.cs`
- **Librería:** `System.IO.Compression` (incluida en .NET)
- **Funciones:**
  - ✅ Comprimir archivos sueltos o carpetas completas → `.zip`
  - ✅ Descomprimir archivos `.zip`
  - ✅ Listar contenido del ZIP sin descomprimir

### RAR (Estela)
- **Clase principal:** `EstelaRar.cs`
- **Librería:** `SharpCompress 0.34.2`
- **Funciones:**
  - ✅ Comprimir usando WinRAR instalado en el sistema
  - ✅ Descomprimir archivos `.rar`
  - ⚠️ Requiere WinRAR instalado en `C:\Program Files\WinRAR\`

### LZMA (Romero)
- **Clase principal:** `RomeroLzma.cs`
- **Librería:** `SharpCompress 0.32.2`
- **Funciones:**
  - ✅ Comprimir archivos con algoritmo LZMA → `.zip`
  - ✅ Descomprimir archivos comprimidos
  - ✅ Muestra tamaño original vs comprimido y % de reducción

---

## 🧪 Pruebas Unitarias / Unit Tests

**Proyecto:** `zipChecya.Tests` (MSTest, .NET 10)

| Prueba | Descripción | Resultado |
|---|---|---|
| `Comprimir_ArchivoExiste_CreaZip` | Verifica que al comprimir un archivo se genera el .zip | ✅ Passed |
| `Descomprimir_ZipValido_ExtraeArchivo` | Verifica que al descomprimir se extrae el archivo correctamente | ✅ Passed |
| `ListarContenido_ZipValido_RetornaArchivos` | Verifica que se listan los archivos dentro del ZIP sin extraerlos | ✅ Passed |

Para ejecutar las pruebas / To run the tests:
```
Prueba → Ejecutar todas las pruebas
```

---

## 📊 Análisis de Calidad — SonarQube / Quality Analysis

| Métrica | Resultado |
|---|---|
| **Quality Gate** | ✅ Passed |
| **Security** | ✅ A — 0 issues |
| **Reliability** | ✅ A — 0 issues |
| **Maintainability** | ✅ A — 9 code smells menores |
| **Duplications** | ✅ 0.0% |
| **Security Hotspot** | ⚠️ 1 (descompresión ZIP — revisado) |

**Versión SonarQube:** Community Build v26.5.0.122743

Para ejecutar el análisis / To run the analysis:
```bash
dotnet sonarscanner begin /k:"ModuloCompresion" /d:sonar.host.url="http://localhost:9000" /d:sonar.token="TU_TOKEN"
dotnet build "ModuloCompresion\ModuloCompresion.slnx"
dotnet sonarscanner end /d:sonar.token="TU_TOKEN"
```

---

## 🖥️ Interfaz Principal / Main Interface

La interfaz principal (`ModuloCompresion`) presenta:
- Diseño azul oscuro profesional (`#12203A`)
- Ícono de compresión
- ListBox con las 3 opciones: ZIP, RAR, LZMA
- Botón **Usar** que abre la interfaz del algoritmo seleccionado

The main interface (`ModuloCompresion`) features:
- Professional dark blue design (`#12203A`)
- Compression icon
- ListBox with 3 options: ZIP, RAR, LZMA
- **Usar** button that opens the selected algorithm's interface

---

## 📝 Notas Importantes / Important Notes

- **ES:** El módulo RAR requiere WinRAR instalado. Si no está instalado, el sistema mostrará un mensaje de error claro al intentar comprimir.
- **EN:** The RAR module requires WinRAR to be installed. If not installed, the system will display a clear error message when attempting to compress.

---

## 📅 Fecha / Date
Mayo 2026 / May 2026

