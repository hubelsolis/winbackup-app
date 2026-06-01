using System.Collections.Generic;

namespace winbackup
{
    public interface IFileScannerService
    {
        List<FileScanResult> EscanearTodo();
        List<FileScanResult> EscanearRuta(RutaBackupConfig ruta);
    }
}
