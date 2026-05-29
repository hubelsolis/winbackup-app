using Microsoft.VisualStudio.TestTools.UnitTesting;
using winbackup; // Esto conecta con tus clases originales

namespace winbackup.Tests
{
    [TestClass]
    public class clseguridadTests
    {
        [TestMethod]
        public void EncriptarYDesencriptar_TextoValido_RetornaTextoOriginal()
        {
            // 1. ARRANGE (Preparar el escenario con la contraseña real)
            string textoOriginal = "Sistema2024$";

            // 2. ACT (Ejecutar encriptación y desencriptación consecutivas)
            string textoCifrado = clseguridad.Encriptar(textoOriginal);
            string textoResultado = clseguridad.Desencriptar(textoCifrado);

            // 3. ASSERT (Validar que el flujo sea reversible y seguro)
            Assert.AreNotEqual(textoOriginal, textoCifrado, "El texto cifrado no debería ser legible ni igual al plano.");
            Assert.AreEqual(textoOriginal, textoResultado, "El proceso falló: el texto desencriptado no coincide con el original.");
        }

        [TestMethod]
        public void Encriptar_CadenaVacia_RetornaCadenaVacia()
        {
            // ARRANGE
            string textoVacio = "";

            // ACT
            string resultado = clseguridad.Encriptar(textoVacio);

            // ASSERT
            Assert.AreEqual("", resultado, "Si se pasa un texto vacío, debe retornar vacío sin romper el flujo.");
        }
    }
}