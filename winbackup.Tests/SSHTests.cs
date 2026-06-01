using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using winbackup.Core.Transferencia;

namespace winbackup.Tests
{
    /// <summary>
    /// Pruebas REALES de SSH contra el servidor local Windows OpenSSH.
    /// Verifica conexión, ejecución de comandos y verificación de archivos.
    /// Servidor: 127.0.0.1 puerto 22
    /// </summary>
    [TestClass]
    public class SSHTests
    {
        private clconfiguracion _config = null!;
        private TransferenciaSSH _ssh = null!;

        [TestInitialize]
        public void Setup()
        {
            _config = clconfiguracion.Cargar("config.json");

            _ssh = new TransferenciaSSH(
                host: _config.CredencialesLocal!.SftpHost,
                puerto: 22,
                usuario: _config.CredencialesLocal!.User,
                password: _config.CredencialesLocal!.Pass
            );
        }

        // ═════════════════════════════════════════════
        // PRUEBA 1 — Conexión SSH al servidor local
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("SSH-Real")]
        public void SSH_ProbarConexion_ServidorLocal_RetornaTrue()
        {
            // ACT
            bool conectado = _ssh.ProbarConexion();

            // ASSERT
            Assert.IsTrue(conectado, "Debe conectar al servidor SSH 127.0.0.1:22");
        }

        // ═════════════════════════════════════════════
        // PRUEBA 2 — Ejecutar comando en servidor
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("SSH-Real")]
        public void SSH_EjecutarComando_ListarCarpeta_RetornaResultado()
        {
            // ACT
            string resultado = _ssh.EjecutarComando(
                "powershell -command \"Get-ChildItem 'C:\\Users\\WINDOWS\\Documents\\servidor_STP'\""
            );

            // ASSERT
            Assert.IsNotNull(resultado, "El comando SSH debe retornar una respuesta.");
        }

        // ═════════════════════════════════════════════
        // PRUEBA 3 — Verificar que la carpeta backup existe
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("SSH-Real")]
        public void SSH_EjecutarComando_CarpetaBackupExiste_RetornaTrue()
        {
            // ACT
            string resultado = _ssh.EjecutarComando(
                "powershell -command \"Test-Path 'C:\\Users\\WINDOWS\\Documents\\servidor_STP'\""
            );

            // ASSERT
            Assert.IsTrue(resultado.Trim().Contains("True"),
                "La carpeta servidor_STP debe existir en el servidor.");
        }

        // ═════════════════════════════════════════════
        // PRUEBA 4 — Subir por SFTP y verificar con SSH
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("SSH-Real")]
        public void SSH_ArchivoExiste_VerificarChunkSubidoPorSFTP()
        {
            // ARRANGE — subir archivo por SFTP
            var sftp = new TransferenciaSFTP(
                host: _config.CredencialesLocal!.SftpHost,
                puerto: 22,
                usuario: _config.CredencialesLocal!.User,
                password: _config.CredencialesLocal!.Pass,
                carpetaRemota: _config.CredencialesLocal!.SftpCarpeta
            );

            byte[] datos = System.Text.Encoding.UTF8.GetBytes(
                "archivo para verificar con SSH"
            );
            sftp.SubirChunk(datos, datos.Length, "ssh_verificacion.zip.part001");

            // ACT — verificar con SSH que el archivo llegó
            string rutaCompleta = @"C:\Users\WINDOWS\Documents\servidor_STP\ssh_verificacion.zip.part001";
            bool existe = _ssh.ArchivoExisteEnServidor(rutaCompleta);

            // ASSERT
            Assert.IsTrue(existe,
                "SSH debe confirmar que el archivo subido por SFTP existe en el servidor.");
        }

        // ═════════════════════════════════════════════
        // PRUEBA 5 — Listar archivos y mostrar en consola
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("SSH-Real")]
        public void SSH_EjecutarComando_ListarArchivosBackup_MuestraContenido()
        {
            // ACT
            string resultado = _ssh.EjecutarComando(
                "powershell -command \"Get-ChildItem 'C:\\Users\\WINDOWS\\Documents\\servidor_STP' | Select-Object Name, Length\""
            );

            // ASSERT
            Assert.IsNotNull(resultado, "El listado no debe ser nulo.");

            // Mostrar en consola de pruebas para evidencia
            Console.WriteLine("=== Archivos en servidor_STP ===");
            Console.WriteLine(resultado);
        }

        // ═════════════════════════════════════════════
        // PRUEBA 6 — Credenciales SSH cargadas
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("SSH-Real")]
        public void SSH_Credenciales_SeCarganDesdeConfigJson()
        {
            Assert.IsNotNull(_config.CredencialesLocal,
                "CREDENCIALES_LOCAL debe existir en config.json.");
            Assert.IsFalse(string.IsNullOrEmpty(_config.CredencialesLocal.User),
                "Usuario SSH no debe estar vacío.");
            Assert.IsFalse(string.IsNullOrEmpty(_config.CredencialesLocal.Pass),
                "Contraseña SSH no debe estar vacía.");
            Assert.AreEqual("127.0.0.1", _config.CredencialesLocal.SftpHost,
                "Host SSH debe ser 127.0.0.1.");
        }
    }
}