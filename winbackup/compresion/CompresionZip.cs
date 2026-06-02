using Ionic.Zip;

namespace winbackup.compresion
{
    public class CompresionZip : ICompresion
    {
        public void Comprimir(string rutaEntrada, string rutaSalida, string contrasena = "")
        {
            using var zip = new ZipFile();

            if (!string.IsNullOrEmpty(contrasena))
            {
                zip.Password = contrasena;
                zip.Encryption = EncryptionAlgorithm.WinZipAes256;
            }

            if (Directory.Exists(rutaEntrada))
                zip.AddDirectory(rutaEntrada);
            else if (File.Exists(rutaEntrada))
                zip.AddFile(rutaEntrada, "");

            zip.Save(rutaSalida);
        }

        public void Descomprimir(string rutaEntrada, string rutaDestino, string contrasena = "")
        {
            using var zip = ZipFile.Read(rutaEntrada);

            if (!string.IsNullOrEmpty(contrasena))
                zip.Password = contrasena;

            foreach (var entry in zip)
                entry.Extract(rutaDestino, ExtractExistingFileAction.OverwriteSilently);
        }

        public List<string> ListarContenido(string rutaArchivo)
        {
            var lista = new List<string>();
            using var zip = ZipFile.Read(rutaArchivo);
            foreach (var entry in zip)
                lista.Add($"{entry.FileName} ({entry.UncompressedSize} bytes)");
            return lista;
        }
    }
}