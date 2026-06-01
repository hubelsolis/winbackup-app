using System;

namespace winbackup
{
    public class FileChangeEvent
    {
        public string Ruta { get; set; }
        public TipoCambio Tipo { get; set; }
        public DateTime Timestamp { get; set; }
        public string Nombre => System.IO.Path.GetFileName(Ruta);
        public string Extension => System.IO.Path.GetExtension(Ruta)?.ToLowerInvariant();
    }
}
