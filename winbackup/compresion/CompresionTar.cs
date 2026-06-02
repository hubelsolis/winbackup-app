using SharpCompress.Archives;
using SharpCompress.Archives.Tar;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;

namespace winbackup.compresion
{
    public class CompresionTar : ICompresion
    {
        public void Comprimir(string rutaEntrada, string rutaSalida, string contrasena = "")
        {
            using var stream = File.Create(rutaSalida);
            using var writer = WriterFactory.Open(stream,
                ArchiveType.Tar,
                new WriterOptions(CompressionType.GZip));

            if (File.Exists(rutaEntrada))
                writer.Write(Path.GetFileName(rutaEntrada), rutaEntrada);
            else if (Directory.Exists(rutaEntrada))
                foreach (var archivo in Directory.GetFiles(rutaEntrada, "*", SearchOption.AllDirectories))
                    writer.Write(Path.GetRelativePath(rutaEntrada, archivo), archivo);
        }

        public void Descomprimir(string rutaEntrada, string rutaDestino, string contrasena = "")
        {
            using var stream = File.OpenRead(rutaEntrada);
            using var reader = ReaderFactory.Open(stream);
            while (reader.MoveToNextEntry())
            {
                if (!reader.Entry.IsDirectory)
                    reader.WriteEntryToDirectory(rutaDestino,
                        new ExtractionOptions
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
            }
        }

        public List<string> ListarContenido(string rutaArchivo)
        {
            var lista = new List<string>();
            using var archive = TarArchive.Open(rutaArchivo);
            foreach (var entry in archive.Entries)
                if (!entry.IsDirectory)
                    lista.Add($"{entry.Key} ({entry.Size} bytes)");
            return lista;
        }
    }
}