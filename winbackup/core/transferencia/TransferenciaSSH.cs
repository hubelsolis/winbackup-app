using Renci.SshNet;

namespace winbackup.Core.Transferencia
{
    /// <summary>
    /// Ejecuta comandos remotos por SSH (útil para verificar integridad,
    /// descomprimir en el servidor o disparar scripts post-backup).
    /// Requiere el paquete NuGet: SSH.NET
    /// </summary>
    public class TransferenciaSSH
    {
        private readonly string _host;
        private readonly int _puerto;
        private readonly string _usuario;
        private readonly string _password;

        public TransferenciaSSH(string host, int puerto, string usuario, string password)
        {
            _host = host;
            _puerto = puerto;
            _usuario = usuario;
            _password = password;
        }

        /// <summary>
        /// Ejecuta un comando en el servidor remoto y devuelve la salida.
        /// Ejemplo: EjecutarComando("ls /backups/") 
        /// </summary>
        public string EjecutarComando(string comando)
        {
            using var ssh = new SshClient(_host, _puerto, _usuario, _password);
            ssh.Connect();

            using SshCommand resultado = ssh.RunCommand(comando);
            string salida = resultado.Result;

            ssh.Disconnect();
            return salida;
        }

        /// <summary>
        /// Verifica si un archivo existe en el servidor remoto.
        /// </summary>
        public bool ArchivoExisteEnServidor(string rutaCompleta)
        {
            // El comando test devuelve código 0 si existe, 1 si no
            string salida = EjecutarComando($"test -f \"{rutaCompleta}\" && echo SI || echo NO");
            return salida.Trim() == "SI";
        }

        public bool ProbarConexion()
        {
            try
            {
                using var ssh = new SshClient(_host, _puerto, _usuario, _password);
                ssh.ConnectionInfo.Timeout = TimeSpan.FromSeconds(5);
                ssh.Connect();
                bool conectado = ssh.IsConnected;
                ssh.Disconnect();
                return conectado;
            }
            catch
            {
                return false;
            }
        }
    }
}