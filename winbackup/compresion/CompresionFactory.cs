namespace winbackup.compresion
{
    public static class CompresionFactory
    {
        public static ICompresion Crear(string algoritmo)
        {
            return algoritmo.ToLower() switch
            {
                "zip" => new CompresionZip(),
                "rar" => new CompresionRar(),
                "lzma" => new CompresionLzma(),
                "tar" => new CompresionTar(),
                "cab" => new CompresionCab(),
                "gzip" => new CompresionGzip(),
                _ => throw new ArgumentException($"Algoritmo '{algoritmo}' no soportado.")
            };
        }

        public static List<string> AlgoritmosDisponibles()
        {
            return new List<string> { "ZIP", "RAR", "LZMA", "TAR", "CAB", "GZIP" };
        }
    }
}