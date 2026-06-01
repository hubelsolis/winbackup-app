using System;

namespace winbackup
{
    public interface IBackupScheduler
    {
        void Iniciar(int intervaloSegundos);
        void Detener();
        bool EstaActivo { get; }
        event Action OnTick;
    }
}
