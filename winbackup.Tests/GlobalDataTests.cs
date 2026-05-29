using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using winbackup; // Conexión con tu proyecto principal

namespace winbackup.Tests
{
    [TestClass]
    public class GlobalDataTests
    {
        private string _rutaConfigJson;

        [TestInitialize]
        public void Setup()
        {
            // Ubica la carpeta bin/Debug/... del proyecto de pruebas donde corre el test
            string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;
            _rutaConfigJson = Path.Combine(directorioEjecucion, "config.json");

            // Creamos un archivo config.json idéntico al de producción pero controlado
            string jsonDePrueba = @"{
                ""CREDENCIALES"": {
                    ""user"": ""usuario_test@viafact.com"",
                    ""pass"": ""CRmBnDjy5zu0WDkfZVjcOUgdnATHDbNxOQAtCAUfCJc="",
                    ""ftpBaseUrl"": ""ftp://127.0.0.1/""
                },
                ""BACKUPS"": {
                    ""backups"": ""3""
                }
            }";

            // Forzamos la escritura física para que GlobalData lo pueda leer al inicializar
            File.WriteAllText(_rutaConfigJson, jsonDePrueba);
        }

        [TestCleanup]
        public void TearDown()
        {
            // Limpieza: Borramos el archivo config.json temporal para no interferir con otros tests
            if (File.Exists(_rutaConfigJson))
            {
                File.Delete(_rutaConfigJson);
            }
        }

        [TestMethod]
        public void Inicializar_ArchivoConfigExistente_CargaContenedorGlobalCorrectamente()
        {
            // 1. ARRANGE
            // Nos aseguramos de limpiar cualquier estado previo en memoria RAM
            GlobalData.Config = null;

            // 2. ACT
            // Ejecutamos tu método de inicialización que dispara internamente el clconfiguracion.Cargar()
            GlobalData.Inicializar();

            // 3. ASSERT
            // Comprobamos que el contenedor estático global ya no sea nulo
            Assert.IsNotNull(GlobalData.Config, "GlobalData.Config no debería ser nulo tras invocar Inicializar().");

            // Nota técnica: Una vez que me pases el código de clconfiguracion, 
            // podremos mapear propiedades aquí de la siguiente forma:
            // Assert.AreEqual(3, GlobalData.Config.backups);
        }
    }
}