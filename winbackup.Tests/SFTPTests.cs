using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.IO.Compression;
using winbackup.Core.Transferencia;

namespace winbackup.Tests
{
    /// <summary>
    /// Pruebas REALES de SFTP contra el servidor local Windows OpenSSH.
    /// Requiere que config.json tenga CREDENCIALES_LOCAL válidas.
    /// Servidor: 127.0.0.1 puerto 22
    /// </summary>
    [TestClass]
    public class SFTPTests
    {
        private clconfiguracion _config = null!;
        private TransferenciaSFTP _sftp = null!;
        private string _carpetaTemp = string.Empty;
        private string _archivoLocal = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            _config = clconfiguracion.Cargar("config.json");

            _sftp = new TransferenciaSFTP(
                host: _config.CredencialesLocal!.SftpHost,
                puerto: 22,
                usuario: _config.CredencialesLocal!.User,
                password: _config.CredencialesLocal!.Pass,
                carpetaRemota: _config.CredencialesLocal!.SftpCarpeta
            );

            // Carpeta única por prueba para evitar conflictos
            _carpetaTemp = Path.Combine(Path.GetTempPath(), "sftp_test_" + Guid.NewGuid());
            Directory.CreateDirectory(_carpetaTemp);

            _archivoLocal = Path.Combine(_carpetaTemp, "prueba_sftp.txt");
            File.WriteAllText(_archivoLocal, "Prueba SFTP real - Grupo 3 - Hans - winbackup");
        }

        [TestCleanup]
        public void TearDown()
        {
            System.Threading.Thread.Sleep(200);

            if (Directory.Exists(_carpetaTemp))
            {
                try { Directory.Delete(_carpetaTemp, recursive: true); }
                catch { /* no afecta el resultado de la prueba */ }
            }
        }

        // ═════════════════════════════════════════════
        // PRUEBA 1 — Conexión al servidor SFTP local
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("SFTP-Real")]
        public void SFTP_ProbarConexion_ServidorLocal_RetornaTrue()
        {
            // ACT
            bool conectado = _sftp.ProbarConexion();

            // ASSERT
            Assert.IsTrue(conectado,
                $"Debe conectar al servidor SFTP {_config.CredencialesLocal!.SftpHost}:22");
        }

        // ═════════════════════════════════════════════
        // PRUEBA 2 — Subir un chunk real por SFTP
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("SFTP-Real")]
        public void SFTP_SubirChunk_ArchivoReal_SubeCorrectamente()
        {
            // ARRANGE
            byte[] datos = System.Text.Encoding.UTF8.GetBytes(
                "Chunk SFTP real subido desde winbackup - Grupo 3"
            );
            string nombreRemoto = "winbackup_test_sftp.zip.part001";

            // ACT
            _sftp.SubirChunk(datos, datos.Length, nombreRemoto);

            // ASSERT
            Assert.IsTrue(true, "El chunk se subió al servidor SFTP sin errores.");
        }

        // ═════════════════════════════════════════════
        // PRUEBA 3 — Flujo completo SFTP
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("SFTP-Real")]
        public void SFTP_FlujoCompleto_ComprimirYSubir_FuncionaCorrectamente()
        {
            // ARRANGE
            string zipPath = Path.Combine(_carpetaTemp, "prueba_sftp.zip");
            int chunkSize = 2 * 1024 * 1024; // 2MB

            // PASO 1 — Comprimir
            using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(_archivoLocal, Path.GetFileName(_archivoLocal));
            }
            Assert.IsTrue(File.Exists(zipPath), "ZIP debe crearse correctamente.");

            // PASO 2 — Dividir y subir chunks
            int chunksSubidos = 0;
            using (FileStream fs = new(zipPath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[chunkSize];
                int part = 1;
                int leido;

                while ((leido = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string nombre = $"prueba_sftp.zip.part{part:D3}";
                    _sftp.SubirChunk(buffer, leido, nombre);
                    part++;
                    chunksSubidos++;
                }
            }

            // PASO 3 — Limpiar ZIP temporal
            File.Delete(zipPath);

            // ASSERT
            Assert.IsFalse(File.Exists(zipPath), "ZIP temporal debe eliminarse.");
            Assert.IsTrue(chunksSubidos > 0, "Debe haberse subido al menos 1 chunk.");
        }

        // ═════════════════════════════════════════════
        // PRUEBA 4 — Credenciales locales cargadas
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("SFTP-Real")]
        public void SFTP_Credenciales_SeCarganDesdeConfigJson()
        {
            Assert.IsNotNull(_config.CredencialesLocal,
                "CREDENCIALES_LOCAL debe existir en config.json.");
            Assert.IsFalse(string.IsNullOrEmpty(_config.CredencialesLocal.User),
                "Usuario SFTP no debe estar vacío.");
            Assert.IsFalse(string.IsNullOrEmpty(_config.CredencialesLocal.Pass),
                "Contraseña SFTP no debe estar vacía.");
            Assert.AreEqual("127.0.0.1", _config.CredencialesLocal.SftpHost,
                "Host SFTP debe ser 127.0.0.1.");
        }
    }
}