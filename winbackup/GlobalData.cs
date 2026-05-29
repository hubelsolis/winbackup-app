// TODO: Limpieza de codigo 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winbackup
{
    public static class GlobalData
    {
        // Contenedor global de la configuración
        public static clconfiguracion Config { get; set; }

        /// <summary>
        /// Inicializa la configuración global. 
        /// Llamar esto al inicio de la aplicación (Program.cs o Form_Load)
        /// </summary>
        public static void Inicializar()
        {
            Config = clconfiguracion.Cargar();
        }
    }
}