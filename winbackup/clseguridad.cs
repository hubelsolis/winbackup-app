using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace winbackup
{
    public static class clseguridad
    {
        // IMPORTANTE: Esta frase es tu llave maestra. 
        // Cámbiala por una de 32 caracteres y NO LA PIERDAS.
        private static readonly string Passphrase = "VIAFACT_SECURITY_KEY_TINGO_2026!";
        private static readonly byte[] Salt = Encoding.ASCII.GetBytes("Salt_Backup_System");

        public static string Encriptar(string textoPlano)
        {
            if (string.IsNullOrEmpty(textoPlano)) return "";

            byte[] clearBytes = Encoding.Unicode.GetBytes(textoPlano);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Passphrase, Salt, 1000, HashAlgorithmName.SHA256);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    textoPlano = Convert.ToBase64String(ms.ToArray());
                }
            }
            return textoPlano;
        }

        public static string Desencriptar(string textoCifrado)
        {
            if (string.IsNullOrEmpty(textoCifrado)) return "";

            try
            {
                // Reemplazar espacios por '+' maneja inconsistencias de transporte Base64
                byte[] cipherBytes = Convert.FromBase64String(textoCifrado.Replace(" ", "+"));

                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Passphrase, Salt, 1000, HashAlgorithmName.SHA256);
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);

                    // CORRECCIÓN: Pasamos 'cipherBytes' al MemoryStream para que tenga datos que leer
                    using (MemoryStream ms = new MemoryStream(cipherBytes))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs, Encoding.Unicode))
                            {
                                return sr.ReadToEnd(); // Lee todo el flujo descifrado limpiamente
                            }
                        }
                    }
                }
            }
            catch
            {
                // Si el token está corrupto o alterado de forma externa, devuelve vacío de forma segura
                return "";
            }
        }








    }
}