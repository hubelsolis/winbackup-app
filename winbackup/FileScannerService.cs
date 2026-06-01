using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace winbackup
{
    public class FileScannerService : IFileScannerService
    {
        private readonly IDbfFileValidatorService _dbfValidator;
        private readonly ILogService _log;

        public FileScannerService() : this(new DbfFileValidatorService(), null)
        {
        }

        public FileScannerService(IDbfFileValidatorService dbfValidator, ILogService log = null)
        {
            _dbfValidator = dbfValidator;
            _log = log;
        }

        public List<FileScanResult> EscanearTodo()
        {
            var resultados = new List<FileScanResult>();

            if (GlobalData.Config?.Rutas?.Carpetas == null)
            {
                _log?.Info("EscanearTodo: no hay rutas configuradas.");
                return resultados;
            }

            _log?.Info($"Iniciando escaneo de {GlobalData.Config?.Rutas?.Carpetas?.Count ?? 0} rutas...");

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
            {
                _log?.Advertencia($"Carpeta no encontrada: {ruta.Origen}");
                return resultados;
            }

            _log?.Info($"Escaneando carpeta: {ruta.Origen}");

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
                        _log?.Advertencia($"Acceso denegado: {resultado.Nombre}");
                    }
                    catch (IOException ex)
                    {
                        resultado.EsAccesible = false;
                        resultado.Error = ex.Message;
                        _log?.Error($"Error de lectura: {resultado.Nombre} — {ex.Message}");
                    }

                    if (resultado.EsAccesible && resultado.Extension == ".dbf")
                    {
                        var dbfConfig = GlobalData.Config?.Dbf;
                        if (dbfConfig != null && dbfConfig.Habilitado)
                        {
                            var validacion = _dbfValidator.Validar(archivo);
                            if (!validacion.esValido)
                            {
                                resultado.EsAccesible = false;
                                resultado.Error = validacion.razon;
                                _log?.Advertencia($"DBF bloqueado: {resultado.Nombre} — {validacion.razon}");
                            }
                        }
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
