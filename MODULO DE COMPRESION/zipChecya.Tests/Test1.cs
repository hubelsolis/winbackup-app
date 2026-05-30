using zipChecya;

namespace zipChecya.Tests
{
    [TestClass]
    public sealed class Test1
    {
        private string carpetaTemp = "";

        [TestInitialize]
        public void Setup()
        {
            carpetaTemp = Path.Combine(Path.GetTempPath(), "TestZip_" + Guid.NewGuid());
            Directory.CreateDirectory(carpetaTemp);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(carpetaTemp))
                Directory.Delete(carpetaTemp, true);
        }

        [TestMethod]
        public void Comprimir_ArchivoExiste_CreaZip()
        {
            // Arrange
            string archivo = Path.Combine(carpetaTemp, "prueba.txt");
            string zip = Path.Combine(carpetaTemp, "prueba.zip");
            File.WriteAllText(archivo, "Hola mundo");

            // Act
            CompresorZip.Comprimir(archivo, zip);

            // Assert
            Assert.IsTrue(File.Exists(zip), "El ZIP debe existir");
        }

        [TestMethod]
        public void Descomprimir_ZipValido_ExtraeArchivo()
        {
            // Arrange
            string archivo = Path.Combine(carpetaTemp, "prueba.txt");
            string zip = Path.Combine(carpetaTemp, "prueba.zip");
            string destino = Path.Combine(carpetaTemp, "extraido");
            File.WriteAllText(archivo, "Hola mundo");
            CompresorZip.Comprimir(archivo, zip);
            Directory.CreateDirectory(destino);

            // Act
            CompresorZip.Descomprimir(zip, destino);

            // Assert
            Assert.IsTrue(File.Exists(Path.Combine(destino, "prueba.txt")), "El archivo debe extraerse");
        }

        [TestMethod]
        public void ListarContenido_ZipValido_RetornaArchivos()
        {
            // Arrange
            string archivo = Path.Combine(carpetaTemp, "prueba.txt");
            string zip = Path.Combine(carpetaTemp, "prueba.zip");
            File.WriteAllText(archivo, "Hola mundo");
            CompresorZip.Comprimir(archivo, zip);

            // Act
            var contenido = CompresorZip.ListarContenido(zip);

            // Assert
            Assert.IsTrue(contenido.Count > 0, "Debe listar al menos un archivo");
        }
    }
}