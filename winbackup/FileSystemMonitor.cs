using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace winbackup
{
    public class FileSystemMonitor : IFileSystemMonitor
    {
        private FileSystemWatcher _watcher;
        private readonly int _debounceMs;
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _debounceTokens = new(StringComparer.OrdinalIgnoreCase);

        public event Action<FileChangeEvent> OnCambioDetectado;
        public bool EstaActivo => _watcher?.EnableRaisingEvents ?? false;

        public FileSystemMonitor(int debounceMs = 1500)
        {
            _debounceMs = debounceMs;
        }

        public void Iniciar(string ruta, string filtro = "*.*", bool incluirSubcarpetas = true)
        {
            Detener();

            if (!Directory.Exists(ruta))
                return;

            _watcher = new FileSystemWatcher(ruta, filtro)
            {
                IncludeSubdirectories = incluirSubcarpetas,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
                EnableRaisingEvents = true
            };

            _watcher.Created += (s, e) => AplicarDebounce(e.FullPath, TipoCambio.Creado);
            _watcher.Changed += (s, e) => AplicarDebounce(e.FullPath, TipoCambio.Modificado);
            _watcher.Deleted += (s, e) => DispararEvento(e.FullPath, TipoCambio.Eliminado);
            _watcher.Renamed += (s, e) => DispararEvento(e.FullPath, TipoCambio.Renombrado);
        }

        public void Detener()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
                _watcher = null;
            }

            foreach (var kvp in _debounceTokens)
            {
                kvp.Value?.Cancel();
                kvp.Value?.Dispose();
            }
            _debounceTokens.Clear();
        }

        private void AplicarDebounce(string ruta, TipoCambio tipo)
        {
            if (_debounceTokens.TryGetValue(ruta, out var ctsAnterior))
            {
                ctsAnterior.Cancel();
                ctsAnterior.Dispose();
            }

            var cts = new CancellationTokenSource();
            _debounceTokens[ruta] = cts;

            Task.Delay(_debounceMs, cts.Token).ContinueWith(t =>
            {
                if (!t.IsCanceled)
                {
                    _debounceTokens.TryRemove(ruta, out _);
                    DispararEvento(ruta, tipo);
                }
            }, TaskScheduler.Default);
        }

        private void DispararEvento(string ruta, TipoCambio tipo)
        {
            OnCambioDetectado?.Invoke(new FileChangeEvent
            {
                Ruta = ruta,
                Tipo = tipo,
                Timestamp = DateTime.Now
            });
        }
    }
}
