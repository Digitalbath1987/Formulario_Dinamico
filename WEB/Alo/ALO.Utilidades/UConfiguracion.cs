using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALO.Utilidades
{
    public class UConfiguracion
    {

        //=====================================================================
        // CONFIGURACION RESCATADAS DESDE EL WEBCONFIG
        //=====================================================================
        public static string ServidorUrl_GET = System.Configuration.ConfigurationSettings.AppSettings["URL_RESTful_GET"];
        public static string ServidorUrl_POST = System.Configuration.ConfigurationSettings.AppSettings["URL_RESTful_POST"];
        public static string ServidorRest_Opcion = System.Configuration.ConfigurationSettings.AppSettings["URL_RESTful_OPCION"];
        public static string ServidorWCF = System.Configuration.ConfigurationSettings.AppSettings["URL_WCF"];
        public static string RutaLearning = System.Configuration.ConfigurationSettings.AppSettings["RUTA_LEARNING"];
    }
}
