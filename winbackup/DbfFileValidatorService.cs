using System;
using System.IO;
using System.Linq;

namespace winbackup
{
    public class DbfFileValidatorService : IDbfFileValidatorService
    {
        private readonly long _tamanoMinimo;
        private readonly bool _validarCabecera;

        private static readonly byte[] FirmasValidas = {
            0x02, 0x03, 0x04, 0x05,       // FoxBASE / dBase III-IV-V sin memo
            0x30, 0x31, 0x32,             // Visual FoxPro
            0x6B, 0x7B, 0x8B, 0x8E,       // dBase IV con memo / SQL
            0x83, 0xF5, 0xFB              // FoxBASE / FoxPro con memo
        };

        public DbfFileValidatorService()
        {
            var dbfConfig = GlobalData.Config?.Dbf;
            _tamanoMinimo = dbfConfig?.TamanoMinimoBytes ?? 512;
            _validarCabecera = dbfConfig?.ValidarCabecera ?? true;
        }

        public bool EstaBloqueado(string ruta)
        {
            try
            {
                using var fs = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.None);
                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }

        public bool CabeceraValida(string ruta)
        {
            try
            {
                using var fs = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] header = new byte[1];
                int read = fs.Read(header, 0, 1);
                return read == 1 && FirmasValidas.Contains(header[0]);
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public (bool esValido, string razon) Validar(string ruta)
        {
            if (!File.Exists(ruta))
                return (false, "El archivo no existe");

            var info = new FileInfo(ruta);
            if (info.Length < _tamanoMinimo)
                return (false, $"El archivo DBF mide {info.Length} bytes, mínimo requerido: {_tamanoMinimo}");

            if (EstaBloqueado(ruta))
                return (false, "El archivo DBF está bloqueado por otro proceso (Firebird/FoxPro)");

            if (_validarCabecera && !CabeceraValida(ruta))
                return (false, "La cabecera DBF no contiene una firma válida. El archivo puede estar corrupto o no ser DBF");

            return (true, null);
        }
    }
}
