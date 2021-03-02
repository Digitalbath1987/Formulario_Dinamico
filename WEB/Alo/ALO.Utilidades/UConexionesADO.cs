using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALO.Utilidades
{
    /// <summary>
    /// CLASE QUE PERMITE CREAR LAS CADENAS DE CONECCION POR MEDIO
    /// DE LA INFORMACIÓN QUE SE PREVEE EN EL WEBCONFIG DEL APLICATIVO WEB
    /// </summary>
    public class UConexionesADO
    {




        /*-------------------------------------------------------------------------*/
        /* SERVIDOR DATOS DE CONEXIONES                                            */
        /*-------------------------------------------------------------------------*/
        public string Servidor_A    = System.Configuration.ConfigurationSettings.AppSettings["DB_SERVIDOR_G"];
        public string Usuario_A     = System.Configuration.ConfigurationSettings.AppSettings["DB_USUARIO_G"];
        public string Password_A    = System.Configuration.ConfigurationSettings.AppSettings["DB_PASSWORD_G"];
        public string BaseDatos_A   = System.Configuration.ConfigurationSettings.AppSettings["DB_BASEDATOS_G"];
        public string ASHX_POST_ALO = System.Configuration.ConfigurationSettings.AppSettings["ASHX_POST_ALO"];


        /// <summary>
        /// RETORNO DE URL POST
        /// </summary>
        /// <returns></returns>
        public string URLPost()
        {
            string URL = "";

            try
            {

                //===========================================================
                // KEY DE WEBCONFIG                               
                //===========================================================
                URL = this.ASHX_POST_ALO;
                return URL;


            }
            catch
            {
                return "";
            }

        }



        /// <summary>
        /// RETORNO DE CONEXION BASES DE DATOS 
        /// </summary>
        /// <returns></returns>
        public string ConexionCargas()
        {
            string Retorno = "";
            try
            {
                //===========================================================
                // SE EXTRAEN REGISTROS DE CONEXION A BASE DE DATOS                      
                //===========================================================
                string Usuario = this.Usuario_A;
                string Password = this.Password_A;
                string BaseDatos = this.BaseDatos_A;
                string Servidor = this.Servidor_A;

                //===========================================================
                // RETORNA CADENA DE CONEXION                      
                //===========================================================
                return CadenaConexion(Usuario, Password, BaseDatos, Servidor);


            }
            catch
            {

                return Retorno;
            }

        }

        /// <summary>
        /// CADENA DE CONEXION
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="Paswword"></param>
        /// <param name="BaseDatos"></param>
        /// <param name="Servidor"></param>
        /// <returns></returns>
        private string CadenaConexion(string Usuario
                                           , string Paswword
                                           , string BaseDatos
                                           , string Servidor)
        {

            string Retorno = "";
            try
            {

                //===========================================================
                // CADENA DE CONEXION                     
                //===========================================================
                Retorno = @"User ID=" + Usuario + ";"
                        + "Password =" + Paswword + ";"
                        + "Initial Catalog =" + BaseDatos + ";"
                        + "Data Source=" + Servidor + ";"
                        + "Persist Security Info=True;"
                        + "Pooling=False;"
                        + "Connection Lifetime=5;"
                        + "Application Name= RESTFUL MIT";


                return Retorno;

            }
            catch
            {
                return Retorno;
            }


        }






    }
}
