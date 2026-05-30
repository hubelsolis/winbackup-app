using SharpCompress.Common;
using SharpCompress.Readers;
using System.Diagnostics;
using System.IO;

namespace WinFormsAppRAR
{
    public class EstelaRar
    {
        public static void Descomprimir(string rutaRar, string rutaDestino)
        {
            using var stream = File.OpenRead(rutaRar);
            using var reader = ReaderFactory.Open(stream);
            while (reader.MoveToNextEntry())
            {
                if (!reader.Entry.IsDirectory)
                    reader.WriteEntryToDirectory(rutaDestino, new ExtractionOptions
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
            }
        }

        public static void Comprimir(string rutaEntrada, string rutaSalida)
        {
            string winrar = @"C:\Program Files\WinRAR\WinRAR.exe";
            if (!File.Exists(winrar))
            {
                throw new Exception("WinRAR no está instalado en este equipo.");
            }
            var proceso = new Process();
            proceso.StartInfo.FileName = winrar;
            proceso.StartInfo.Arguments = $"a \"{rutaSalida}\" \"{rutaEntrada}\"";
            proceso.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proceso.Start();
            proceso.WaitForExit();
        }
    }
}