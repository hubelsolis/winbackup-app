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
            using var sftp = CrearCliente();
            sftp.Connect();

            string rutaRemota = _carpetaRemota + nombreArchivoRemoto;

            using var stream = new MemoryStream(datos, 0, longitud);

            if (sftp.Exists(rutaRemota))
                sftp.DeleteFile(rutaRemota);

            sftp.UploadFile(stream, rutaRemota);
            sftp.Disconnect();
        }

        /// <summary>
        /// Retorna true si conecta, lanza excepción con detalle si falla.
        /// El error real ahora es visible en Form1.
        /// </summary>
        public bool ProbarConexion()
        {
            using var sftp = CrearCliente();
            sftp.Connect(); // si falla lanza excepción con el motivo real
            bool conectado = sftp.IsConnected;
            sftp.Disconnect();
            return conectado;
        }

        // ─────────────────────────────────────────────
        // Crear cliente con configuración estándar
        // ─────────────────────────────────────────────
        private SftpClient CrearCliente()
        {
            var cliente = new SftpClient(_host, _puerto, _usuario, _password);
            cliente.ConnectionInfo.Timeout = TimeSpan.FromSeconds(10);
            return cliente;
        }
    }
}