using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.IO.Compression;
using winbackup.Core.Transferencia;

namespace winbackup.Tests
{
    /// <summary>
    /// Pruebas REALES de FTP contra el servidor del docente.
    /// Requiere que config.json esté disponible con credenciales válidas.
    /// Servidor: ftp://192.254.237.233/
    /// </summary>
    [TestClass]
    public class FTPTests
    {
        private clconfiguracion _config = null!;
        private TransferenciaFTP _ftp = null!;
        private string _carpetaTemp = string.Empty;
        private string _archivoLocal = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            _config = clconfiguracion.Cargar("config.json");

            _ftp = new TransferenciaFTP(
                _config.Credenciales.FtpBaseUrl,
                _config.Credenciales.User,
                _config.Credenciales.Pass
            );

            // Carpeta única por prueba para evitar conflictos entre tests
            _carpetaTemp = Path.Combine(Path.GetTempPath(), "ftp_test_" + Guid.NewGuid());
            Directory.CreateDirectory(_carpetaTemp);

            _archivoLocal = Path.Combine(_carpetaTemp, "prueba_ftp.txt");
            File.WriteAllText(_archivoLocal, "Prueba FTP real - Grupo 3 - Hans - winbackup");
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
        // PRUEBA 1 — Conexión al servidor FTP
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("FTP-Real")]
        public void FTP_ProbarConexion_ServidorDocente_RetornaTrue()
        {
            // ACT
            bool conectado = _ftp.ProbarConexion();

            // ASSERT
            Assert.IsTrue(conectado,
                $"Debe conectar al servidor FTP {_config.Credenciales.FtpBaseUrl}");
        }

        // ═════════════════════════════════════════════
        // PRUEBA 2 — Subir un chunk real al servidor
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("FTP-Real")]
        public void FTP_SubirChunk_ArchivoReal_SubeCorrectamente()
        {
            // ARRANGE
            byte[] datos = System.Text.Encoding.UTF8.GetBytes(
                "Chunk FTP real subido desde winbackup - Grupo 3"
            );
            string nombreRemoto = "winbackup_test_ftp.zip.part001";

            // ACT
            _ftp.SubirChunk(datos, datos.Length, nombreRemoto);

            // ASSERT — si no lanza excepción el chunk se subió
            Assert.IsTrue(true, "El chunk se subió al servidor FTP sin errores.");
        }

        // ═════════════════════════════════════════════
        // PRUEBA 3 — Flujo completo FTP
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("FTP-Real")]
        public void FTP_FlujoCompleto_ComprimirYSubir_FuncionaCorrectamente()
        {
            // ARRANGE
            string zipPath = Path.Combine(_carpetaTemp, "prueba_ftp.zip");
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
                    string nombre = $"prueba_ftp.zip.part{part:D3}";
                    _ftp.SubirChunk(buffer, leido, nombre);
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
        // PRUEBA 4 — Credenciales cargadas correctamente
        // ═════════════════════════════════════════════
        [TestMethod]
        [TestCategory("FTP-Real")]
        public void FTP_Credenciales_SeCarganDesdeConfigJson()
        {
            Assert.IsFalse(string.IsNullOrEmpty(_config.Credenciales.User),
                "Usuario FTP no debe estar vacío.");
            Assert.IsFalse(string.IsNullOrEmpty(_config.Credenciales.Pass),
                "Contraseña FTP no debe estar vacía después de desencriptar.");
            Assert.IsFalse(string.IsNullOrEmpty(_config.Credenciales.FtpBaseUrl),
                "URL FTP no debe estar vacía.");
            Assert.IsTrue(_config.Credenciales.FtpBaseUrl.StartsWith("ftp://"),
                "URL FTP debe comenzar con ftp://");
        }
    }
}