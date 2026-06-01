using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace winbackup
{
    public class FileScannerService : IFileScannerService
    {
        public List<FileScanResult> EscanearTodo()
        {
            var resultados = new List<FileScanResult>();

            if (GlobalData.Config?.Rutas?.Carpetas == null)
                return resultados;

            foreach (var ruta in GlobalData.Config.Rutas.Carpetas)
            {
                if (!Directory.Exists(ruta.Origen))
                    continue;

                var archivos = EscanearRuta(ruta);
                resultados.AddRange(archivos);
            }

            return resultados;
        }

        public List<FileScanResult> EscanearRuta(RutaBackupConfig ruta)
        {
            var resultados = new List<FileScanResult>();

            if (!Directory.Exists(ruta.Origen))
                return resultados;

            var opcionesBusqueda = ruta.IncluirSubcarpetas
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            try
            {
                var archivos = Directory.EnumerateFiles(
                    ruta.Origen,
                    ruta.Patron ?? "*.*",
                    opcionesBusqueda
                );

                foreach (var archivo in archivos)
                {
                    var resultado = new FileScanResult
                    {
                        RutaCompleta = archivo,
                        Nombre = Path.GetFileName(archivo),
                        Extension = Path.GetExtension(archivo)?.ToLowerInvariant(),
                        CarpetaOrigen = ruta.Origen,
                        RutaRelativa = Path.GetRelativePath(ruta.Origen, archivo)
                    };

                    if (ruta.Excluir != null && ruta.Excluir.Contains(resultado.Extension))
                        continue;

                    try
                    {
                        var info = new FileInfo(archivo);
                        resultado.TamañoBytes = info.Length;
                        resultado.UltimaModificacion = info.LastWriteTime;
                        resultado.EsAccesible = true;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        resultado.EsAccesible = false;
                        resultado.Error = "Acceso denegado";
                    }
                    catch (IOException ex)
                    {
                        resultado.EsAccesible = false;
                        resultado.Error = ex.Message;
                    }

                    resultados.Add(resultado);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }

            return resultados;
        }
    }
}
