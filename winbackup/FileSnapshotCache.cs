using System;
using System.Collections.Generic;
using System.Linq;

namespace winbackup
{
    public class FileSnapshotCache
    {
        private Dictionary<string, DateTime> _snapshot = new(StringComparer.OrdinalIgnoreCase);
        private readonly object _lock = new();

        public void Actualizar(string ruta, DateTime ultimaModificacion)
        {
            lock (_lock)
            {
                _snapshot[ruta] = ultimaModificacion;
            }
        }

        public void ActualizarDesdeLista(List<FileScanResult> archivos)
        {
            lock (_lock)
            {
                _snapshot = archivos
                    .Where(a => a.EsAccesible)
                    .ToDictionary(a => a.RutaCompleta, a => a.UltimaModificacion, StringComparer.OrdinalIgnoreCase);
            }
        }

        public bool HaCambiado(string ruta, DateTime ultimaModificacion)
        {
            lock (_lock)
            {
                return !_snapshot.TryGetValue(ruta, out var previa) || previa != ultimaModificacion;
            }
        }

        public void Remover(string ruta)
        {
            lock (_lock)
            {
                _snapshot.Remove(ruta);
            }
        }

        public void Reiniciar()
        {
            lock (_lock)
            {
                _snapshot.Clear();
            }
        }

        public Dictionary<string, DateTime> ObtenerSnapshot()
        {
            lock (_lock)
            {
                return new Dictionary<string, DateTime>(_snapshot, StringComparer.OrdinalIgnoreCase);
            }
        }

        public List<string> ObtenerEliminados(Dictionary<string, DateTime> snapshotActual)
        {
            lock (_lock)
            {
                return _snapshot.Keys
                    .Where(k => !snapshotActual.ContainsKey(k))
                    .ToList();
            }
        }

        public CambiosDetectados CompararYDiferenciar(List<FileScanResult> archivosActuales)
        {
            var cambios = new CambiosDetectados();
            var actual = archivosActuales
                .Where(a => a.EsAccesible)
                .ToDictionary(a => a.RutaCompleta, a => a.UltimaModificacion, StringComparer.OrdinalIgnoreCase);

            lock (_lock)
            {
                foreach (var kvp in actual)
                {
                    if (!_snapshot.TryGetValue(kvp.Key, out var previa))
                    {
                        cambios.Nuevos.Add(kvp.Key);
                    }
                    else if (previa != kvp.Value)
                    {
                        cambios.Modificados.Add(kvp.Key);
                    }
                }

                cambios.Eliminados = _snapshot.Keys
                    .Where(k => !actual.ContainsKey(k))
                    .ToList();
            }

            return cambios;
        }
    }

    public class CambiosDetectados
    {
        public List<string> Nuevos { get; set; } = new();
        public List<string> Modificados { get; set; } = new();
        public List<string> Eliminados { get; set; } = new();
        public bool HayCambios => Nuevos.Count > 0 || Modificados.Count > 0 || Eliminados.Count > 0;

        public override string ToString()
        {
            var partes = new List<string>();
            if (Nuevos.Count > 0) partes.Add($"+{Nuevos.Count} nuevos");
            if (Modificados.Count > 0) partes.Add($"~{Modificados.Count} modificados");
            if (Eliminados.Count > 0) partes.Add($"-{Eliminados.Count} eliminados");
            return string.Join(", ", partes);
        }
    }
}
