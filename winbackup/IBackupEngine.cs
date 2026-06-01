using System.Collections.Generic;

namespace winbackup
{
    public interface IBackupEngine
    {
        void ProcesarCambios(List<FileScanResult> archivosActuales, CambiosDetectados cambios);
        void ProcesarArchivoIndividual(FileScanResult archivo);
    }
}
