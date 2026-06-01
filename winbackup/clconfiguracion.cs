using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace winbackup
{
    public class clconfiguracion
    {
        [JsonPropertyName("CREDENCIALES")]
        public CredencialesConfig Credenciales { get; set; } = new();

        [JsonPropertyName("CREDENCIALES_LOCAL")]
        public CredencialesLocalConfig? CredencialesLocal { get; set; }

        [JsonPropertyName("BACKUPS")]
        public BackupsConfig Backups { get; set; } = new();

        public static clconfiguracion Cargar(string rutaArchivo = "config.json")
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                    throw new FileNotFoundException("No existe config.json");

                string jsonString = File.ReadAllText(rutaArchivo);
                return JsonSerializer.Deserialize<clconfiguracion>(jsonString)
                    ?? throw new Exception("El archivo config.json está vacío o mal formado.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar configuración: " + ex.Message);
            }
        }
    }

    // ─────────────────────────────────────────────
    // Credenciales servidor del docente (FTP)
    // ─────────────────────────────────────────────
    public class CredencialesConfig
    {
        [JsonPropertyName("user")]
        public string User { get; set; } = string.Empty;

        [JsonPropertyName("pass")]
        public string PassEncriptado { get; set; } = string.Empty;

        [JsonIgnore]
        public string Pass => clseguridad.Desencriptar(PassEncriptado);

        [JsonPropertyName("ftpBaseUrl")]
        public string FtpBaseUrl { get; set; } = string.Empty;
    }

    // ─────────────────────────────────────────────
    // Credenciales servidor local (SFTP/SSH)
    // ─────────────────────────────────────────────
    public class CredencialesLocalConfig
    {
        [JsonPropertyName("user")]
        public string User { get; set; } = string.Empty;

        [JsonPropertyName("pass")]
        public string Pass { get; set; } = string.Empty;  // directo, sin descifrar

        [JsonPropertyName("sftpHost")]
        public string SftpHost { get; set; } = string.Empty;

        [JsonPropertyName("sftpCarpeta")]
        public string SftpCarpeta { get; set; } = string.Empty;
    }

    // ─────────────────────────────────────────────
    // Configuración de backups
    // ─────────────────────────────────────────────
    public class BackupsConfig
    {
        [JsonPropertyName("backups")]
        public string Cantidad { get; set; } = string.Empty;
    }
}