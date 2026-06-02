namespace winbackup.compresion
{
    public interface ICompresion
    {
        void Comprimir(string rutaEntrada, string rutaSalida, string contrasena = "");
        void Descomprimir(string rutaEntrada, string rutaDestino, string contrasena = "");
        List<string> ListarContenido(string rutaArchivo);
    }
}