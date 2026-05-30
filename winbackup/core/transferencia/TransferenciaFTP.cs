using FluentFTP;
using System.Net;

namespace winbackup.Core.Transferencia
{
    /// <summary>
    /// Implementa la subida de archivos por FTP usando FluentFTP.
    /// Reemplaza WebRequest obsoleto en .NET 8+
    /// Instalar: dotnet add package FluentFTP
    /// </summary>
    public class TransferenciaFTP : ITransferenciaArchivo
    {
        private readonly string _host;
        private readonly string _usuario;
        private readonly string _password;
        private readonly string _carpetaRemota;

        /// <param name="ftpBaseUrl">URL completa: ftp://162.241.194.172/</param>
        /// <param name="usuario">Usuario FTP</param>
        /// <param name="password">Contraseña FTP</param>
        public TransferenciaFTP(string ftpBaseUrl, string usuario, string password)
        {
            // Extraer solo el host de la URL: "ftp://162.241.194.172/" → "162.241.194.172"
            _host = new Uri(ftpBaseUrl).Host;
            _usuario = usuario;
            _password = password;
            _carpetaRemota = new Uri(ftpBaseUrl).AbsolutePath.TrimEnd('/') + "/";
        }

        public void SubirChunk(byte[] datos, int longitud, string nombreArchivoRemoto)
        {
            using var cliente = CrearCliente();
            cliente.Connect();

            string rutaRemota = _carpetaRemota + nombreArchivoRemoto;

            using var stream = new MemoryStream(datos, 0, longitud);
            cliente.UploadStream(stream, rutaRemota, FtpRemoteExists.Overwrite, createRemoteDir: true);

            cliente.Disconnect();
        }

        public bool ProbarConexion()
        {
            try
            {
                using var cliente = CrearCliente();
                cliente.Connect();
                bool conectado = cliente.IsConnected;
                cliente.Disconnect();
                return conectado;
            }
            catch
            {
                return false;
            }
        }

        // ─────────────────────────────────────────────
        // Crea y configura el cliente FTP
        // ─────────────────────────────────────────────
        private FtpClient CrearCliente()
        {
            return new FtpClient(_host, _usuario, _password)
            {
                Config =
                {
                    ConnectTimeout  = 5000,
                    ReadTimeout     = 10000,
                    DataConnectionType = FtpDataConnectionType.PASV
                }
            };
        }
    }
}