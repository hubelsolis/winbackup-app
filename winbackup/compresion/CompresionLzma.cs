using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;

namespace winbackup.compresion
{
    public class CompresionLzma : ICompresion
    {
        public void Comprimir(string rutaEntrada, string rutaSalida, string contrasena = "")
        {
            using var zip = new Ionic.Zip.ZipFile();

            if (!string.IsNullOrEmpty(contrasena))
            {
                zip.Password = contrasena;
                zip.Encryption = Ionic.Zip.EncryptionAlgorithm.WinZipAes256;
            }

            zip.CompressionMethod = Ionic.Zip.CompressionMethod.BZip2;

            if (File.Exists(rutaEntrada))
                zip.AddFile(rutaEntrada, "");
            else if (Directory.Exists(rutaEntrada))
                zip.AddDirectory(rutaEntrada);

            zip.Save(rutaSalida);
        }

        public void Descomprimir(string rutaEntrada, string rutaDestino, string contrasena = "")
        {
            using var zip = Ionic.Zip.ZipFile.Read(rutaEntrada);

            if (!string.IsNullOrEmpty(contrasena))
                zip.Password = contrasena;

            foreach (var entry in zip)
                entry.Extract(rutaDestino,
                    Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
        }

        public List<string> ListarContenido(string rutaArchivo)
        {
            var lista = new List<string>();
            using var zip = Ionic.Zip.ZipFile.Read(rutaArchivo);
            foreach (var entry in zip)
                lista.Add($"{entry.FileName} ({entry.UncompressedSize} bytes)");
            return lista;
        }
    }
}