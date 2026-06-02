using Microsoft.VisualStudio.TestTools.UnitTesting;
using winbackup.compresion;

namespace winbackup.Tests
{
    [TestClass]
    public class CompresionTests
    {
        private string carpetaTemp = "";

        [TestInitialize]
        public void Setup()
        {
            carpetaTemp = Path.Combine(Path.GetTempPath(), "TestCompresion_" + Guid.NewGuid());
            Directory.CreateDirectory(carpetaTemp);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(carpetaTemp))
                Directory.Delete(carpetaTemp, true);
        }

        // ── ZIP ──────────────────────────────────────────────────────────
        [TestMethod]
        public void ZIP_Comprimir_SinContrasena_CreaArchivo()
        {
            string archivo = Path.Combine(carpetaTemp, "prueba.txt");
            string zip = Path.Combine(carpetaTemp, "prueba.zip");
            File.WriteAllText(archivo, "Hola mundo");

            var compresor = CompresionFactory.Crear("zip");
            compresor.Comprimir(archivo, zip);

            Assert.IsTrue(File.Exists(zip));
        }

        [TestMethod]
        public void ZIP_Comprimir_ConContrasena_CreaArchivo()
        {
            string archivo = Path.Combine(carpetaTemp, "prueba.txt");
            string zip = Path.Combine(carpetaTemp, "prueba_pass.zip");
            File.WriteAllText(archivo, "Hola mundo");

            var compresor = CompresionFactory.Crear("zip");
            compresor.Comprimir(archivo, zip, "1234");

            Assert.IsTrue(File.Exists(zip));
        }

        [TestMethod]
        public void ZIP_Descomprimir_SinContrasena_ExtraeArchivo()
        {
            string archivo = Path.Combine(carpetaTemp, "prueba.txt");
            string zip = Path.Combine(carpetaTemp, "prueba.zip");
            string destino = Path.Combine(carpetaTemp, "extraido");
            File.WriteAllText(archivo, "Hola mundo");
            Directory.CreateDirectory(destino);

            var compresor = CompresionFactory.Crear("zip");
            compresor.Comprimir(archivo, zip);
            compresor.Descomprimir(zip, destino);

            Assert.IsTrue(File.Exists(Path.Combine(destino, "prueba.txt")));
        }

        [TestMethod]
        public void ZIP_ListarContenido_RetornaArchivos()
        {
            string archivo = Path.Combine(carpetaTemp, "prueba.txt");
            string zip = Path.Combine(carpetaTemp, "prueba.zip");
            File.WriteAllText(archivo, "Hola mundo");

            var compresor = CompresionFactory.Crear("zip");
            compresor.Comprimir(archivo, zip);
            var contenido = compresor.ListarContenido(zip);

            Assert.IsTrue(contenido.Count > 0);
        }

        // ── TAR ──────────────────────────────────────────────────────────
        [TestMethod]
        public void TAR_Comprimir_CreaArchivo()
        {
            string archivo = Path.Combine(carpetaTemp, "prueba.txt");
            string tar = Path.Combine(carpetaTemp, "prueba.tar.gz");
            File.WriteAllText(archivo, "Hola mundo");

            var compresor = CompresionFactory.Crear("tar");
            compresor.Comprimir(archivo, tar);

            Assert.IsTrue(File.Exists(tar));
        }

        [TestMethod]
        public void TAR_Descomprimir_ExtraeArchivo()
        {
            string archivo = Path.Combine(carpetaTemp, "prueba.txt");
            string tar = Path.Combine(carpetaTemp, "prueba.tar.gz");
            string destino = Path.Combine(carpetaTemp, "extraido");
            File.WriteAllText(archivo, "Hola mundo");
            Directory.CreateDirectory(destino);

            var compresor = CompresionFactory.Crear("tar");
            compresor.Comprimir(archivo, tar);
            compresor.Descomprimir(tar, destino);

            Assert.IsTrue(File.Exists(Path.Combine(destino, "prueba.txt")));
        }

        // ── GZIP ─────────────────────────────────────────────────────────
        [TestMethod]
        public void GZIP_Comprimir_CreaArchivo()
        {
            string archivo = Path.Combine(carpetaTemp, "prueba.txt");
            string gz = Path.Combine(carpetaTemp, "prueba.tar.gz");
            File.WriteAllText(archivo, "Hola mundo");

            var compresor = CompresionFactory.Crear("gzip");
            compresor.Comprimir(archivo, gz);

            Assert.IsTrue(File.Exists(gz));
        }

        // ── LZMA ─────────────────────────────────────────────────────────
        [TestMethod]
        public void LZMA_Comprimir_SinContrasena_CreaArchivo()
        {
            string archivo = Path.Combine(carpetaTemp, "prueba.txt");
            string lzma = Path.Combine(carpetaTemp, "prueba.zip");
            File.WriteAllText(archivo, "Hola mundo");

            var compresor = CompresionFactory.Crear("lzma");
            compresor.Comprimir(archivo, lzma);

            Assert.IsTrue(File.Exists(lzma));
        }

        [TestMethod]
        public void LZMA_Comprimir_ConContrasena_CreaArchivo()
        {
            string archivo = Path.Combine(carpetaTemp, "prueba.txt");
            string lzma = Path.Combine(carpetaTemp, "prueba_pass.zip");
            File.WriteAllText(archivo, "Hola mundo");

            var compresor = CompresionFactory.Crear("lzma");
            compresor.Comprimir(archivo, lzma, "1234");

            Assert.IsTrue(File.Exists(lzma));
        }
    }
}