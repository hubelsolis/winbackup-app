using System;

namespace winbackup
{
    public class FileScanResult
    {
        public string RutaCompleta { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public long TamañoBytes { get; set; }
        public DateTime UltimaModificacion { get; set; }
        public string CarpetaOrigen { get; set; }
        public string RutaRelativa { get; set; }
        public bool EsAccesible { get; set; }
        public string Error { get; set; }
    }
}
