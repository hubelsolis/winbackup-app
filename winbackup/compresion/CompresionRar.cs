using System.Diagnostics;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace winbackup.compresion
{
    public class CompresionRar : ICompresion
    {
        private const string RutaWinRar = @"C:\Program Files\WinRAR\WinRAR.exe";

        public void Comprimir(string rutaEntrada, string rutaSalida, string contrasena = "")
        {
            if (!File.Exists(RutaWinRar))
                throw new Exception("WinRAR no está instalado en este equipo.");

            string args = string.IsNullOrEmpty(contrasena)
                ? $"a \"{rutaSalida}\" \"{rutaEntrada}\""
                : $"a -p{contrasena} -hp{contrasena} \"{rutaSalida}\" \"{rutaEntrada}\"";

            var proceso = new Process();
            proceso.StartInfo.FileName = RutaWinRar;
            proceso.StartInfo.Arguments = args;
            proceso.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proceso.Start();
            proceso.WaitForExit();
        }

        public void Descomprimir(string rutaEntrada, string rutaDestino, string contrasena = "")
        {
            if (!File.Exists(RutaWinRar))
                throw new Exception("WinRAR no está instalado en este equipo.");

            string args = string.IsNullOrEmpty(contrasena)
                ? $"x \"{rutaEntrada}\" \"{rutaDestino}\\\""
                : $"x -p{contrasena} \"{rutaEntrada}\" \"{rutaDestino}\\\"";

            var proceso = new Process();
            proceso.StartInfo.FileName = RutaWinRar;
            proceso.StartInfo.Arguments = args;
            proceso.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proceso.Start();
            proceso.WaitForExit();
        }

        public List<string> ListarContenido(string rutaArchivo)
        {
            var lista = new List<string>();
            using var stream = File.OpenRead(rutaArchivo);
            using var reader = ReaderFactory.Open(stream);
            while (reader.MoveToNextEntry())
                lista.Add($"{reader.Entry.Key} ({reader.Entry.Size} bytes)");
            return lista;
        }
    }
}