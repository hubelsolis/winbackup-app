using System;

namespace winbackup
{
    public interface ILogService
    {
        void Info(string mensaje);
        void Exito(string mensaje);
        void Advertencia(string mensaje);
        void Error(string mensaje);
    }
}
