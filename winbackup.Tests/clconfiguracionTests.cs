using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using winbackup; // Conexión con tu proyecto principal

namespace winbackup.Tests
{
    [TestClass]
    public class clconfiguracionTests
    {
        private string _rutaArchivoTemporal;

        [TestInitialize]
        public void Setup()
        {
            // Se ejecuta ANTES de cada prueba. Crea un archivo de configuración de prueba aislado.
            _rutaArchivoTemporal = Path.Combine(Path.GetTempPath(), "config_test.json");

            string jsonDePrueba = @"{
                ""CREDENCIALES"": {
                    ""user"": ""test_user@viafact.com"",
                    ""pass"": ""CRmBnDjy5zu0WDkfZVjcOUgdnATHDbNxOQAtCAUfCJc="",
                    ""ftpBaseUrl"": ""ftp://127.0.0.1/""
                },
                ""BACKUPS"": {
                    ""backups"": 5
                }
            }";

            File.WriteAllText(_rutaArchivoTemporal, jsonDePrueba);
        }

        [TestCleanup]
        public void TearDown()
        {
            // Se ejecuta DESPUÉS de cada prueba. Borra el archivo temporal para no dejar basura en el disco.
            if (File.Exists(_rutaArchivoTemporal))
            {
                File.Delete(_rutaArchivoTemporal);
            }
        }

        [TestMethod]
        public void LeerConfiguracion_ArchivoValido_MapeaPropiedadesCorrectamente()
        {
            // 1. ARRANGE & ACT 
            // Instanciamos tu clase pasándole la ruta del archivo de prueba controlado
            var configuracion = new clconfiguracion();

            // NOTA: Si tu método de lectura requiere la ruta por parámetro, se la pasas aquí.
            // Ejemplo hipotético: configuracion.CargarDesdeRuta(_rutaArchivoTemporal);

            // 2. ASSERT (Validar que los datos cargados en memoria sean los correctos)
            // Reemplaza estas propiedades por los nombres reales que tengan dentro de tu clase:
            Assert.IsNotNull(configuracion, "La configuración no debería ser nula tras leer el archivo.");

            // Descomenta y ajusta estas líneas según los campos reales de tu clase clconfiguracion:
            // Assert.AreEqual("test_user@viafact.com", configuracion.User, "El usuario no se mapeó correctamente.");
            // Assert.AreEqual(5, configuracion.BackupsCount, "La cantidad de copias de seguridad leídas es incorrecta.");
        }
    }
}