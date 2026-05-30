namespace winbackup.Core.Transferencia
{
    /// <summary>
    /// Contrato común para cualquier método de transferencia de archivos.
    /// </summary>
    public interface ITransferenciaArchivo
    {
        /// <summary>
        /// Sube un chunk de bytes al destino remoto.
        /// </summary>
        /// <param name="datos">Bytes a subir</param>
        /// <param name="longitud">Cantidad real de bytes válidos en el array</param>
        /// <param name="nombreArchivoRemoto">Nombre del archivo en el servidor</param>
        void SubirChunk(byte[] datos, int longitud, string nombreArchivoRemoto);

        /// <summary>
        /// Verifica que la conexión al servidor funciona antes de iniciar.
        /// </summary>
        /// <returns>true si la conexión es exitosa</returns>
        bool ProbarConexion();
    }
}