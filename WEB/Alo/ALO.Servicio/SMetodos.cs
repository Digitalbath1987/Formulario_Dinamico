using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALO.Entidades;
using ALO.Utilidades;

namespace ALO.Servicio
{
    public class SMetodos
    {



        string SISTEMA = "VENTA_FOR_PRE";



        public oSP_RETURN_STATUS SP_UPDATE_FORMULARIO_X_OBJETO(iSP_UPDATE_FORMULARIO_X_OBJETO Input)

        {
            oSP_RETURN_STATUS ObjetoRest = new oSP_RETURN_STATUS();

            try
            {
                //===========================================================
                // SERVICIO REST 
                //===========================================================
                using (SRestFul Servicio = new SRestFul())
                {


                    //=======================================================
                    // LLAMADA DEL SERVICIO  
                    //=======================================================
                    int ESTADO = Servicio.Solicitar<oSP_RETURN_STATUS>("SP_UPDATE_FORMULARIO_X_OBJETO", SISTEMA, Input, new object());


                    //=======================================================
                    // EVALUACIÓN DE CABEZERA 
                    //=======================================================
                    if (ESTADO == 1)
                    {
                        ObjetoRest = (oSP_RETURN_STATUS)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                    }
                }

                return ObjetoRest;

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// ACTUALIZA EL CAMPO DE LA GRILLA MULTICHECKED
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public oSP_RETURN_STATUS SP_UPDATE_FORMULARIO_X_OBJETO_MULTICHECKED(iSP_UPDATE_FORMULARIO_X_OBJETO_MULTICHECKED Input)

        {
            oSP_RETURN_STATUS ObjetoRest = new oSP_RETURN_STATUS();

            try
            {
                //===========================================================
                // SERVICIO REST 
                //===========================================================
                using (SRestFul Servicio = new SRestFul())
                {


                    //=======================================================
                    // LLAMADA DEL SERVICIO  
                    //=======================================================
                    int ESTADO = Servicio.Solicitar<oSP_RETURN_STATUS>("SP_UPDATE_FORMULARIO_X_OBJETO_MULTICHECKED", SISTEMA, Input, new object());


                    //=======================================================
                    // EVALUACIÓN DE CABEZERA 
                    //=======================================================
                    if (ESTADO == 1)
                    {
                        ObjetoRest = (oSP_RETURN_STATUS)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                    }
                }


                return ObjetoRest;


            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// ACTUALIZA EL CAMPO DE LA GRILLA DINAMICA
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public oSP_RETURN_STATUS SP_UPDATE_ROW_GRILLA_DINAMICA(iSP_UPDATE_ROW_GRILLA_DINAMICA Input)

        {
            oSP_RETURN_STATUS ObjetoRest = new oSP_RETURN_STATUS();

            try
            {
                //===========================================================
                // SERVICIO REST 
                //===========================================================
                using (SRestFul Servicio = new SRestFul())
                {


                    //=======================================================
                    // LLAMADA DEL SERVICIO  
                    //=======================================================
                    int ESTADO = Servicio.Solicitar<oSP_RETURN_STATUS>("SP_UPDATE_ROW_GRILLA_DINAMICA", SISTEMA, Input, new object());


                    //=======================================================
                    // EVALUACIÓN DE CABEZERA 
                    //=======================================================
                    if (ESTADO == 1)
                    {
                        ObjetoRest = (oSP_RETURN_STATUS)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                    }
                }


                return ObjetoRest;


            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<oSP_READ_PANTALLA_X_ASIGNACION> SP_READ_PANTALLA_X_ASIGNACION(iSP_READ_PANTALLA_X_ASIGNACION Input){
            List<oSP_READ_PANTALLA_X_ASIGNACION> ListaRest = new List<oSP_READ_PANTALLA_X_ASIGNACION>();

            try{
                //===========================================================
                // SERVICIO REST 
                //===========================================================
                using (SRestFul Servicio = new SRestFul()){

                    //=======================================================
                    // LLAMADA DEL SERVICIO  
                    //=======================================================
                    int ESTADO = Servicio.Solicitar<oSP_READ_PANTALLA_X_ASIGNACION>("SP_READ_PANTALLA_X_ASIGNACION", SISTEMA, Input, new object());

                    //=======================================================
                    // EVALUACIÓN DE CABEZERA 
                    //=======================================================
                    if (ESTADO == 1){
                        ListaRest = (List<oSP_READ_PANTALLA_X_ASIGNACION>)Servicio.ObjetoRest;
                    }else{
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                    }
                }
                return ListaRest;
            }catch{
                throw;
            }
        }






        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<oSP_READ_OBJETO_X_OPCION> SP_READ_OBJETO_X_OPCION(iSP_READ_OBJETO_X_OPCION Input)
        {
            List<oSP_READ_OBJETO_X_OPCION> ListaRest = new List<oSP_READ_OBJETO_X_OPCION>();

            try
            {
                //===========================================================
                // SERVICIO REST 
                //===========================================================
                using (SRestFul Servicio = new SRestFul())
                {


                    //=======================================================
                    // LLAMADA DEL SERVICIO  
                    //=======================================================
                    int ESTADO = Servicio.Solicitar<oSP_READ_OBJETO_X_OPCION>("SP_READ_OBJETO_X_OPCION", SISTEMA, Input, new object());


                    //=======================================================
                    // EVALUACIÓN DE CABEZERA 
                    //=======================================================
                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_OBJETO_X_OPCION>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                    }
                }


                return ListaRest;


            }
            catch(Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// LECTURA DE METODO 
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<oSP_READ_OBJETO_X_COMBO> SP_READ_OBJETO_X_COMBO(iSP_READ_OBJETO_X_COMBO Input)
        {
            List<oSP_READ_OBJETO_X_COMBO> ListaRest = new List<oSP_READ_OBJETO_X_COMBO>();

            try
            {
                //===========================================================
                // SERVICIO REST 
                //===========================================================
                using (SRestFul Servicio = new SRestFul())
                {


                    //=======================================================
                    // LLAMADA DEL SERVICIO  
                    //=======================================================
                    int ESTADO = Servicio.Solicitar<oSP_READ_OBJETO_X_COMBO>("SP_READ_OBJETO_X_COMBO", SISTEMA, Input, new object());


                    //=======================================================
                    // EVALUACIÓN DE CABEZERA 
                    //=======================================================
                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_OBJETO_X_COMBO>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                    }
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<oSP_READ_OBJETO_X_GRILLA> SP_READ_OBJETO_X_GRILLA(iSP_READ_OBJETO_X_GRILLA Input)
        {
            List<oSP_READ_OBJETO_X_GRILLA> ListaRest = new List<oSP_READ_OBJETO_X_GRILLA>();

            try
            {
                //===========================================================
                // SERVICIO REST 
                //===========================================================
                using (SRestFul Servicio = new SRestFul())
                {


                    //=======================================================
                    // LLAMADA DEL SERVICIO  
                    //=======================================================
                    int ESTADO = Servicio.Solicitar<oSP_READ_OBJETO_X_GRILLA>("SP_READ_OBJETO_X_GRILLA", SISTEMA, Input, new object());


                    //=======================================================
                    // EVALUACIÓN DE CABEZERA 
                    //=======================================================
                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_OBJETO_X_GRILLA>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                    }
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<oSP_READ_OBJETO_X_TEXTBOX> SP_READ_OBJETO_X_TEXTBOX(iSP_READ_OBJETO_X_TEXTBOX Input)
        {
            List<oSP_READ_OBJETO_X_TEXTBOX> ListaRest = new List<oSP_READ_OBJETO_X_TEXTBOX>();

            try
            {
                //===========================================================
                // SERVICIO REST 
                //===========================================================
                using (SRestFul Servicio = new SRestFul())
                {


                    //=======================================================
                    // LLAMADA DEL SERVICIO  
                    //=======================================================
                    int ESTADO = Servicio.Solicitar<oSP_READ_OBJETO_X_TEXTBOX>("SP_READ_OBJETO_X_TEXTBOX", SISTEMA, Input, new object());


                    //=======================================================
                    // EVALUACIÓN DE CABEZERA 
                    //=======================================================
                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_OBJETO_X_TEXTBOX>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                    }
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<oSP_READ_OBJETO_X_CHECKBOX> SP_READ_OBJETO_X_CHECKBOX(iSP_READ_OBJETO_X_CHECKBOX Input)
        {
            List<oSP_READ_OBJETO_X_CHECKBOX> ListaRest = new List<oSP_READ_OBJETO_X_CHECKBOX>();

            try
            {
                //===========================================================
                // SERVICIO REST 
                //===========================================================
                using (SRestFul Servicio = new SRestFul())
                {


                    //=======================================================
                    // LLAMADA DEL SERVICIO  
                    //=======================================================
                    int ESTADO = Servicio.Solicitar<oSP_READ_OBJETO_X_CHECKBOX>("SP_READ_OBJETO_X_CHECKBOX", SISTEMA, Input, new object());


                    //=======================================================
                    // EVALUACIÓN DE CABEZERA 
                    //=======================================================
                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_OBJETO_X_CHECKBOX>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                    }
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// LECTURA DE METODOS REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public DataTable SP_DELETE_ROW_GRILLA_DINAMICA(iSP_DELETE_ROW_GRILLA_DINAMICA Input)
        {
            DataTable ListaRest = new DataTable();

            try
            {
                //===========================================================
                // DECLARACION DE VARIABLES 
                //===========================================================
                SRestFul Servicio = new SRestFul();

                //=======================================================
                // LLAMADA DEL SERVICIO                                  
                //=======================================================
                int ESTADO = Servicio.SolicitarData("SP_DELETE_ROW_GRILLA_DINAMICA", SISTEMA, Input, new object());

                if (ESTADO == 1)
                {
                    ListaRest = (DataTable)Servicio.ObjetoRest;


                }
                else
                {

                    ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                    throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// LECTURA DE METODOS REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public oSP_RETURN_STATUS SP_CREATE_ROW_GRILLA_DINAMICA(iSP_CREATE_ROW_GRILLA_DINAMICA Input)

        {
            oSP_RETURN_STATUS ObjetoRest = new oSP_RETURN_STATUS();

            try
            {
                //===========================================================
                // SERVICIO REST 
                //===========================================================
                using (SRestFul Servicio = new SRestFul())
                {


                    //=======================================================
                    // LLAMADA DEL SERVICIO  
                    //=======================================================
                    int ESTADO = Servicio.Solicitar<oSP_RETURN_STATUS>("SP_CREATE_ROW_GRILLA_DINAMICA", SISTEMA, Input, new object());


                    //=======================================================
                    // EVALUACIÓN DE CABEZERA 
                    //=======================================================
                    if (ESTADO == 1)
                    {
                        ObjetoRest = (oSP_RETURN_STATUS)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                    }
                }


                return ObjetoRest;


            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// LECTURA DE METODOS REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public DataTable SP_READ_TABLA_PIVOT_WEB_GRV_DINAMICA_DDL(iSP_READ_TABLA_PIVOT_WEB_GRV_DINAMICA_DDL Input)
        {


            DataTable ListaRest = new DataTable();

            try
            {
                //===========================================================
                // DECLARACION DE VARIABLES 
                //===========================================================
                SRestFul Servicio = new SRestFul();



                //=======================================================
                // LLAMADA DEL SERVICIO                                  
                //=======================================================
                int ESTADO = Servicio.SolicitarData("SP_READ_TABLA_PIVOT_WEB_GRV_DINAMICA_DDL", SISTEMA, Input, new object());

                if (ESTADO == 1)
                {
                    ListaRest = (DataTable)Servicio.ObjetoRest;


                }
                else
                {

                    ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                    throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// LECTURA DE METODOS REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public DataTable SP_READ_TABLA_PIVOT_WEB(iSP_READ_TABLA_PIVOT_WEB Input)
        {


            DataTable ListaRest = new DataTable();

            try
            {
                //===========================================================
                // DECLARACION DE VARIABLES 
                //===========================================================
                SRestFul Servicio = new SRestFul();



                //=======================================================
                // LLAMADA DEL SERVICIO                                  
                //=======================================================
                int ESTADO = Servicio.SolicitarData("SP_READ_TABLA_PIVOT_WEB", SISTEMA, Input, new object());

                if (ESTADO == 1)
                {
                    ListaRest = (DataTable)Servicio.ObjetoRest;


                }
                else
                {

                    ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                    throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }

        

        public DataTable SP_READ_TABLA_PIVOT_WEB_GRV_DINAMIC(iSP_READ_TABLA_PIVOT_WEB_GRV_DINAMIC Input)
        {


            DataTable ListaRest = new DataTable();

            try
            {
                //===========================================================
                // DECLARACION DE VARIABLES 
                //===========================================================
                SRestFul Servicio = new SRestFul();



                //=======================================================
                // LLAMADA DEL SERVICIO                                  
                //=======================================================
                int ESTADO = Servicio.SolicitarData("SP_READ_TABLA_PIVOT_WEB_GRV_DINAMIC", SISTEMA, Input, new object());

                if (ESTADO == 1)
                {
                    ListaRest = (DataTable)Servicio.ObjetoRest;


                }
                else
                {

                    ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                    throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }

        public DataTable SP_READ_TABLA_PIVOT_WEB_GRV_MULTICHECKED(iSP_READ_TABLA_PIVOT_WEB_GRV_MULTICHECKED Input)
        {


            DataTable ListaRest = new DataTable();

            try
            {
                //===========================================================
                // DECLARACION DE VARIABLES 
                //===========================================================
                SRestFul Servicio = new SRestFul();



                //=======================================================
                // LLAMADA DEL SERVICIO                                  
                //=======================================================
                int ESTADO = Servicio.SolicitarData("SP_READ_TABLA_PIVOT_WEB_GRV_MULTICHECKED", SISTEMA, Input, new object());

                if (ESTADO == 1)
                {
                    ListaRest = (DataTable)Servicio.ObjetoRest;


                }
                else
                {

                    ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                    throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// LECTURA DE METODOS REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<oSP_READ_FILTRO> SP_READ_FILTRO(iSP_READ_FILTRO Input)
        {
            List<oSP_READ_FILTRO> ListaRest = new List<oSP_READ_FILTRO>();

            try
            {
                //===========================================================
                // SERVICIO REST 
                //===========================================================
                using (SRestFul Servicio = new SRestFul())
                {


                    //=======================================================
                    // LLAMADA DEL SERVICIO  
                    //=======================================================
                    int ESTADO = Servicio.Solicitar<oSP_READ_FILTRO>("SP_READ_FILTRO", SISTEMA, Input, new object());


                    //=======================================================
                    // EVALUACIÓN DE CABEZERA 
                    //=======================================================
                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_FILTRO>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                    }
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<oSP_READ_PARAMETROS_OUTPUT> SP_READ_PARAMETROS_OUTPUT(iSP_READ_PARAMETROS_OUTPUT Input)
        {
            List<oSP_READ_PARAMETROS_OUTPUT> ListaRest = new List<oSP_READ_PARAMETROS_OUTPUT>();

            try
            {
                //===========================================================
                // SERVICIO REST 
                //===========================================================
                using (SRestFul Servicio = new SRestFul())
                {


                    //=======================================================
                    // LLAMADA DEL SERVICIO  
                    //=======================================================
                    int ESTADO = Servicio.Solicitar<oSP_READ_PARAMETROS_OUTPUT>("SP_READ_PARAMETROS_OUTPUT", SISTEMA, Input, new object());


                    //=======================================================
                    // EVALUACIÓN DE CABEZERA 
                    //=======================================================
                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_PARAMETROS_OUTPUT>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                    }
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<oSP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL> SP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL(iSP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL Input)
        {
            List<oSP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL> ListaRest = new List<oSP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL>();

            try
            {
                //===========================================================
                // SERVICIO REST 
                //===========================================================
                using (SRestFul Servicio = new SRestFul())
                {


                    //=======================================================
                    // LLAMADA DEL SERVICIO  
                    //=======================================================
                    int ESTADO = Servicio.Solicitar<oSP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL>("SP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL", SISTEMA, Input, new object());


                    //=======================================================
                    // EVALUACIÓN DE CABEZERA 
                    //=======================================================
                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error));
                    }
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }

    }

}
