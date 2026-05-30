using System.IO.Compression;

namespace zipChecya
{
    public class CompresorZip
    {
        public static void Comprimir(string rutaEntrada, string rutaSalida)
        {
            if (Directory.Exists(rutaEntrada))
                ZipFile.CreateFromDirectory(rutaEntrada, rutaSalida);
            else if (File.Exists(rutaEntrada))
            {
                using var zip = ZipFile.Open(rutaSalida, ZipArchiveMode.Create);
                zip.CreateEntryFromFile(rutaEntrada, Path.GetFileName(rutaEntrada));
            }
        }

        public static void Descomprimir(string rutaZip, string rutaDestino)
        {
            ZipFile.ExtractToDirectory(rutaZip, rutaDestino, overwriteFiles: true);
        }

        public static List<string> ListarContenido(string rutaZip)
        {
            var lista = new List<string>();
            using var zip = ZipFile.OpenRead(rutaZip);
            foreach (var entry in zip.Entries)
                lista.Add($"{entry.FullName}  ({entry.Length} bytes)");
            return lista;
        }
    }
}