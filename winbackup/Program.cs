namespace winbackup
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());

            var config = clconfiguracion.Cargar("config.json");
            MessageBox.Show(
                $"LocalUser: '{config.CredencialesLocal?.User}'\n" +
                $"LocalPass: '{config.CredencialesLocal?.Pass}'\n" +
                $"LocalHost: '{config.CredencialesLocal?.SftpHost}'",
                "Debug bin config"
            );

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

    }


}