using System;
using System.Collections.Generic;
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
                return JsonSerializer.Deserialize<clconfiguracion>(jsonString) ?? new clconfiguracion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar configuración: " + ex.Message);
            }
        }

        public static void Guardar(clconfiguracion config, string rutaArchivo = "config.json")
        {
            try
            {
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(rutaArchivo, json);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar configuración: " + ex.Message);
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
        public string Pass
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PassEncriptado))
                    return string.Empty;

                string decrypted = clseguridad.Desencriptar(PassEncriptado);
                return string.IsNullOrEmpty(decrypted) ? PassEncriptado : decrypted;
            }
        }

        [JsonPropertyName("ftpBaseUrl")]
        public string FtpBaseUrl { get; set; }
    }

    public class BackupItem
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("origen")]
        public string Origen { get; set; }

        [JsonPropertyName("destino")]
        public string Destino { get; set; }
    }

    public class BackupsConfig
    {
        [JsonPropertyName("backups")]
        public string Cantidad { get; set; }

        [JsonPropertyName("frecuencia")]
        public string Frecuencia { get; set; }

        [JsonPropertyName("horaProgramada")]
        public string HoraProgramada { get; set; }

        [JsonPropertyName("items")]
        public List<BackupItem> Items { get; set; } = new List<BackupItem>();

        [JsonIgnore]
        public int CantidadValor => int.TryParse(Cantidad, out int result) ? result : 0;
    }
}