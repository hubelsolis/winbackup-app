using System.Diagnostics;

namespace winbackup.compresion
{
    public class CompresionCab : ICompresion
    {
        public void Comprimir(string rutaEntrada, string rutaSalida, string contrasena = "")
        {
            // CAB usa makecab.exe que viene incluido en Windows
            string ddf = Path.Combine(Path.GetTempPath(), "makecab.ddf");

            File.WriteAllText(ddf,
                $".OPTION EXPLICIT\n" +
                $".Set CabinetNameTemplate={Path.GetFileName(rutaSalida)}\n" +
                $".Set DiskDirectoryTemplate={Path.GetDirectoryName(rutaSalida)}\n" +
                $".Set CompressionType=MSZIP\n" +
                $"\"{rutaEntrada}\"\n");

            var proceso = new Process();
            proceso.StartInfo.FileName = "makecab";
            proceso.StartInfo.Arguments = $"/F \"{ddf}\"";
            proceso.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proceso.StartInfo.CreateNoWindow = true;
            proceso.Start();
            proceso.WaitForExit();

            if (File.Exists(ddf))
                File.Delete(ddf);
        }

        public void Descomprimir(string rutaEntrada, string rutaDestino, string contrasena = "")
        {
            // expand.exe viene incluido en Windows
            var proceso = new Process();
            proceso.StartInfo.FileName = "expand";
            proceso.StartInfo.Arguments = $"\"{rutaEntrada}\" -F:* \"{rutaDestino}\"";
            proceso.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proceso.StartInfo.CreateNoWindow = true;
            proceso.Start();
            proceso.WaitForExit();
        }

        public List<string> ListarContenido(string rutaArchivo)
        {
            var lista = new List<string>();
            var proceso = new Process();
            proceso.StartInfo.FileName = "expand";
            proceso.StartInfo.Arguments = $"\"{rutaArchivo}\" -D";
            proceso.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proceso.StartInfo.CreateNoWindow = true;
            proceso.StartInfo.RedirectStandardOutput = true;
            proceso.StartInfo.UseShellExecute = false;
            proceso.Start();
            string output = proceso.StandardOutput.ReadToEnd();
            proceso.WaitForExit();
            foreach (var linea in output.Split('\n'))
                if (!string.IsNullOrWhiteSpace(linea))
                    lista.Add(linea.Trim());
            return lista;
        }
    }
}