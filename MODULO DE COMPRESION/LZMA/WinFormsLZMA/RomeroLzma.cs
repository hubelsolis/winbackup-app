using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;

namespace WinFormsLZMA;

public static class RomeroLzma
{
    public static (long original, long comprimido, double porcentaje)
        Comprimir(string rutaEntrada, string rutaSalida)
    {
        long tamOriginal = new FileInfo(rutaEntrada).Length;

        using (var salida = File.Create(rutaSalida))
        using (var writer = WriterFactory.Open(
                   salida,
                   ArchiveType.Zip,
                   new WriterOptions(CompressionType.LZMA)))
        {
            writer.Write(Path.GetFileName(rutaEntrada), rutaEntrada);
        }

        long tamComprimido = new FileInfo(rutaSalida).Length;
        double pct = tamOriginal > 0
            ? (1.0 - (double)tamComprimido / tamOriginal) * 100.0
            : 0;

        return (tamOriginal, tamComprimido, pct);
    }

    public static void Descomprimir(string ruta7z, string carpetaDestino)
    {
        using var stream = File.OpenRead(ruta7z);
        using var reader = ReaderFactory.Open(stream);
        while (reader.MoveToNextEntry())
        {
            if (!reader.Entry.IsDirectory)
                reader.WriteEntryToDirectory(carpetaDestino,
                    new ExtractionOptions
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
        }
    }

    public static string FormatBytes(long bytes)
    {
        if (bytes >= 1_048_576) return $"{bytes / 1_048_576.0:F2} MB";
        if (bytes >= 1_024) return $"{bytes / 1_024.0:F2} KB";
        return $"{bytes} bytes";
    }
}