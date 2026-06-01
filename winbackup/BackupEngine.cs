using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace winbackup
{
    public class BackupEngine : IBackupEngine
    {
        private readonly ILogService _log;

        public BackupEngine(ILogService log)
        {
            _log = log;
        }

        public void ProcesarCambios(List<FileScanResult> archivosActuales, CambiosDetectados cambios)
        {
            var rutas = GlobalData.Config?.Rutas?.Carpetas;
            if (rutas == null || rutas.Count == 0)
            {
                _log?.Advertencia("BackupEngine: no hay rutas configuradas.");
                return;
            }

            var archivosACopiar = archivosActuales
                .Where(a => a.EsAccesible)
                .Where(a => cambios.Nuevos.Contains(a.RutaCompleta) || cambios.Modificados.Contains(a.RutaCompleta))
                .ToList();

            if (archivosACopiar.Count == 0)
            {
                _log?.Info("BackupEngine: no hay archivos nuevos o modificados para copiar.");
                return;
            }

            foreach (var archivo in archivosACopiar)
            {
                var rutaConfig = rutas.FirstOrDefault(r =>
                    string.Equals(r.Origen, archivo.CarpetaOrigen, StringComparison.OrdinalIgnoreCase));

                if (rutaConfig == null)
                {
                    _log?.Advertencia($"BackupEngine: no se encontró configuración para origen {archivo.CarpetaOrigen}");
                    continue;
                }

                var destRelativo = rutaConfig.Destino?.Replace("/", "\\") ?? "";
                var destBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destRelativo);
                var destPath = Path.Combine(destBase, archivo.RutaRelativa);

                try
                {
                    var destDir = Path.GetDirectoryName(destPath);
                    if (!string.IsNullOrEmpty(destDir))
                        Directory.CreateDirectory(destDir);

                    File.Copy(archivo.RutaCompleta, destPath, overwrite: true);
                    _log.Exito($"Backup: {archivo.Nombre} → {destPath}");
                }
                catch (Exception ex)
                {
                    _log.Error($"Backup: error al copiar {archivo.Nombre}: {ex.Message}");
                }
            }
        }

        public void ProcesarArchivoIndividual(FileScanResult archivo)
        {
            var rutas = GlobalData.Config?.Rutas?.Carpetas;
            if (rutas == null) return;

            var rutaConfig = rutas.FirstOrDefault(r =>
                string.Equals(r.Origen, archivo.CarpetaOrigen, StringComparison.OrdinalIgnoreCase));

            if (rutaConfig == null) return;

            var destRelativo = rutaConfig.Destino?.Replace("/", "\\") ?? "";
            var destBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destRelativo);
            var destPath = Path.Combine(destBase, archivo.RutaRelativa);

            try
            {
                var destDir = Path.GetDirectoryName(destPath);
                if (!string.IsNullOrEmpty(destDir))
                    Directory.CreateDirectory(destDir);

                File.Copy(archivo.RutaCompleta, destPath, overwrite: true);
                _log.Exito($"Backup: {archivo.Nombre} → {destPath}");
            }
            catch (Exception ex)
            {
                _log.Error($"Backup: error al copiar {archivo.Nombre}: {ex.Message}");
            }
        }
    }
}
