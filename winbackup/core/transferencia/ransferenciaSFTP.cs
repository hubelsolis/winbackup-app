using Renci.SshNet;

namespace winbackup.Core.Transferencia
{
    /// <summary>
    /// Implementa la subida de archivos por SFTP (SSH File Transfer Protocol).
    /// Requiere el paquete NuGet: SSH.NET
    /// Más seguro que FTP porque cifra toda la transferencia.
    /// </summary>
    public class TransferenciaSFTP : ITransferenciaArchivo
    {
        private readonly string _host;
        private readonly int _puerto;
        private readonly string _usuario;
        private readonly string _password;
        private readonly string _carpetaRemota;

        /// <param name="host">IP o dominio del servidor</param>
        /// <param name="puerto">Puerto SFTP, normalmente 22</param>
        /// <param name="usuario">Usuario SSH</param>
        /// <param name="password">Contraseña SSH</param>
        /// <param name="carpetaRemota">Ruta remota donde guardar archivos, ej: "/backups/"</param>
        public TransferenciaSFTP(string host, int puerto, string usuario,
                                 string password, string carpetaRemota = "/")
        {
            _host = host;
            _puerto = puerto;
            _usuario = usuario;
            _password = password;
            _carpetaRemota = carpetaRemota.TrimEnd('/') + "/";
        }

        public void SubirChunk(byte[] datos, int longitud, string nombreArchivoRemoto)
        {
            using var sftp = new SftpClient(_host, _puerto, _usuario, _password);
            sftp.Connect();

            string rutaRemota = _carpetaRemota + nombreArchivoRemoto;

            using var stream = new MemoryStream(datos, 0, longitud);

            // CORRECCIÓN: UploadFile en SSH.NET no acepta 'overwrite' como parámetro nombrado
            // Se elimina el archivo si existe antes de subir
            if (sftp.Exists(rutaRemota))
                sftp.DeleteFile(rutaRemota);

            sftp.UploadFile(stream, rutaRemota);

            sftp.Disconnect();
        }

        public bool ProbarConexion()
        {
            try
            {
                using var sftp = new SftpClient(_host, _puerto, _usuario, _password);
                sftp.ConnectionInfo.Timeout = TimeSpan.FromSeconds(5);
                sftp.Connect();
                bool conectado = sftp.IsConnected;
                sftp.Disconnect();
                return conectado;
            }
            catch
            {
                return false;
            }
        }
    }
}