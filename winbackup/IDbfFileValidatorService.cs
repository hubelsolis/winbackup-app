namespace winbackup
{
    public interface IDbfFileValidatorService
    {
        bool EstaBloqueado(string ruta);
        bool CabeceraValida(string ruta);
        (bool esValido, string razon) Validar(string ruta);
    }
}
