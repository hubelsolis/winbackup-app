using System;

namespace winbackup
{
    public interface IFileSystemMonitor
    {
        void Iniciar(string ruta, string filtro = "*.*", bool incluirSubcarpetas = true);
        void Detener();
        bool EstaActivo { get; }
        event Action<FileChangeEvent> OnCambioDetectado;
    }
}
