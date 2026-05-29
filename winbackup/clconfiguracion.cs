using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace winbackup
{
    public class clconfiguracion
    {
        [JsonPropertyName("CREDENCIALES")]
        public CredencialesConfig Credenciales { get; set; }

        [JsonPropertyName("BACKUPS")]
        public BackupsConfig Backups { get; set; }

        public static clconfiguracion Cargar(string rutaArchivo = "config.json")
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                    throw new FileNotFoundException("No existe config.json");

                string jsonString = File.ReadAllText(rutaArchivo);
                return JsonSerializer.Deserialize<clconfiguracion>(jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar configuración: " + ex.Message);
            }
        }
    }

    public class CredencialesConfig
    {
        [JsonPropertyName("user")]
        public string User { get; set; }

        // Guardamos el valor encriptado que viene del JSON
        [JsonPropertyName("pass")]
        public string PassEncriptado { get; set; }

        // Esta es la propiedad que usarás en tu código de backup
        // Al pedirla, se desencripta automáticamente "al vuelo"
        [JsonIgnore]
        public string Pass => clseguridad.Desencriptar(PassEncriptado);

        [JsonPropertyName("ftpBaseUrl")]
        public string FtpBaseUrl { get; set; }
    }

    public class BackupsConfig
    {
        [JsonPropertyName("backups")]
        public string Cantidad { get; set; }
    }
}