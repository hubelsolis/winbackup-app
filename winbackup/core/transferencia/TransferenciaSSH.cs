using Renci.SshNet;

namespace winbackup.Core.Transferencia
{
    /// <summary>
    /// Ejecuta comandos remotos por SSH.
    /// Útil para verificar integridad, listar archivos
    /// o disparar scripts post-backup en el servidor.
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
        /// </summary>
        public string EjecutarComando(string comando)
        {
            using var ssh = CrearCliente();
            ssh.Connect();

            using SshCommand resultado = ssh.RunCommand(comando);
            string salida = resultado.Result;

            ssh.Disconnect();
            return salida;
        }

        /// <summary>
        /// Verifica si un archivo existe en el servidor remoto.
        /// Usa PowerShell Test-Path para compatibilidad con Windows.
        /// </summary>
        public bool ArchivoExisteEnServidor(string rutaCompleta)
        {
            // Escapar backslashes para PowerShell
            string rutaEscapada = rutaCompleta.Replace("\\", "\\\\");
            string comando = $"powershell -command \"Test-Path '{rutaEscapada}'\"";
            string salida = EjecutarComando(comando);
            return salida.Trim().Contains("True");
        }

        /// <summary>
        /// Verifica que la conexión SSH funciona.
        /// </summary>
        public bool ProbarConexion()
        {
            try
            {
                using var ssh = CrearCliente();
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

        // ─────────────────────────────────────────────
        // Crear cliente SSH con configuración estándar
        // ─────────────────────────────────────────────
        private SshClient CrearCliente()
        {
            var cliente = new SshClient(_host, _puerto, _usuario, _password);
            cliente.ConnectionInfo.Timeout = TimeSpan.FromSeconds(10);
            return cliente;
        }
    }
}