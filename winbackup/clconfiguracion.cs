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

        [JsonPropertyName("DBF_CONFIG")]
        public DbfConfig Dbf { get; set; }

        [JsonPropertyName("RUTAS")]
        public RutasConfig Rutas { get; set; }

        [JsonPropertyName("SINCRONIZACION")]
        public SincronizacionConfig Sincronizacion { get; set; }

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

    public class RutaBackupConfig
    {
        [JsonPropertyName("origen")]
        public string Origen { get; set; }

        [JsonPropertyName("destino")]
        public string Destino { get; set; }

        [JsonPropertyName("patron")]
        public string Patron { get; set; }

        [JsonPropertyName("incluirSubcarpetas")]
        public bool IncluirSubcarpetas { get; set; }

        [JsonPropertyName("excluir")]
        public List<string> Excluir { get; set; }
    }

    public class RutasConfig
    {
        [JsonPropertyName("carpetas")]
        public List<RutaBackupConfig> Carpetas { get; set; }
    }

    public class SincronizacionConfig
    {
        [JsonPropertyName("habilitado")]
        public bool Habilitado { get; set; }

        [JsonPropertyName("intervaloSegundos")]
        public int IntervaloSegundos { get; set; }

        [JsonPropertyName("debounceMs")]
        public int DebounceMs { get; set; }

        [JsonPropertyName("soloSiHayCambios")]
        public bool SoloSiHayCambios { get; set; }
    }

    public class DbfConfig
    {
        [JsonPropertyName("habilitado")]
        public bool Habilitado { get; set; }

        [JsonPropertyName("tamanoMinimoBytes")]
        public long TamanoMinimoBytes { get; set; }

        [JsonPropertyName("validarCabecera")]
        public bool ValidarCabecera { get; set; }
    }
}