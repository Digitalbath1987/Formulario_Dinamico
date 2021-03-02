using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.DynamicData;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ALO.Entidades;
using ALO.Entidades.Form;
using ALO.Servicio;
using ALO.Utilidades;
using Microsoft.AspNet.SignalR.Hubs;

namespace ALO.WebSite.Formularios
{



    public partial class Formularios : System.Web.UI.Page {



        protected void Page_PreRender(object sender, EventArgs e){
            //===========================================================
            // MANTENER LA POSICION ACTUAL DEL SCROLL                        
            //=========================================================== 
            MaintainScrollPositionOnPostBack = true;

           if (IsPostBack){
                //===========================================================
                // ACTUALIZA LA VARIABLE GLOBAL                         
                //===========================================================
                GUARDAR_VALOR_OBJETO_V_GLOBAL();
            
                TabActivoAjax();
            }
        }

        /// <summary>
        /// AL CARGAR LA PAGINA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
  
                //===========================================================
                // POSTBACK                                               
                //===========================================================
                if (!this.IsPostBack) {

                    TabActivoAjax();

                    Establecer_Globales();

                    BuscarElementosPOSTEnviados();

                    //===========================================================
                    // CREACION DE PANTALLAS                      
                    //===========================================================
                    CrearPantallas(PNL_DINAMICO);

                } if (IsPostBack) {

                    //===========================================================
                    // CREACION DE PANTALLAS                      
                    //===========================================================
                    CrearPantallas(PNL_DINAMICO);

                    //===========================================================
                    // DIBUJAR OBJETOS                    
                    //===========================================================
                    LEER_OBJETO_NO_POSTBACK();

                }

            } catch (EServiceRestFulException svr) {

                MensajeLOG(svr.Message, "ERRORES DE SERVICIO");

            } catch (System.Exception ex) {

                MensajeLOG(UThrowError.MensajeThrow(ex), "ERRORES DE APLICACIÓN");

            }
        }



        /// <summary>
        /// VALIRAR OBLIGATORIEDAD DE OBJETOS
        /// </summary>
        /// <returns></returns>
        private bool VALIDAR_OBJETO(String PANTALLA){
            try{
                string script;
                //===========================================================
                // RECORRO TODOS LOS CONTROLES
                //===========================================================
                foreach (var OBJ in V_Global().LST_CONTROLES.Where(p => p.PANTALLA == int.Parse(PANTALLA))){

                    //===========================================================
                    // QUITO LAS GRILLAS DE LA LISTA 
                    //===========================================================
                    if (OBJ.ID_TIPO_OBJETO != 2) {

                        //===========================================================
                        // VALIDO QUE TENGAN DATOS GUARDADOS
                        //===========================================================
                        if (OBJ.VALOR == "" && OBJ.OBLIGATORIO == true){

                            //===========================================================
                            // MENSAJE DE ALERTA
                            //===========================================================
                            script = @"<script type='text/javascript'> alert(' Campo :  " + OBJ.DESCRIPCION + " , ES OBLIGATORIO, Favor rellenar los datos'); </script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);



                            return false;

                        }
                    }

                    //===========================================================
                    // VALIDO EL LARGO DE LOS TEXBOX
                    //===========================================================
                    if (OBJ.ID_TIPO_OBJETO == 3){
                        OBJETO_TEXTBOX OBJ_TEXTBOX = new OBJETO_TEXTBOX();
                        OBJ_TEXTBOX = (OBJETO_TEXTBOX)OBJ.OBJETO;
                        int LARGO   = OBJ.VALOR.Length;

                        //===========================================================
                        // VALIDO LOS TEXBOX CON LARGO
                        //===========================================================
                        if (LARGO < OBJ_TEXTBOX.MIN_CARACTERES && OBJ_TEXTBOX.ID_TIPO_DATO  == 1 || LARGO > OBJ_TEXTBOX.MAX_CARACTERES && OBJ_TEXTBOX.ID_TIPO_DATO == 1){

                            //===========================================================
                            // MENSAJE DE ALERTA
                            //===========================================================
                            script = @"<script type='text/javascript'> alert(' Campo :  "+ OBJ_TEXTBOX.LABEL + " , No cumple el minimo de caracteres ("+ OBJ_TEXTBOX.MIN_CARACTERES.ToString()  + ") '); </script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);


                            return false;
                        }

                    }


                }

                return true;

            }catch{
                throw;
            }

        }


        /// <summary>
        /// BUSCAR ELEMENTOS POST DESDE OTRAS PAGINAS
        /// </summary>
        /// <returns></returns>
        private bool  BuscarElementosPOSTEnviados(){

            try
            {
                //===========================================================
                // ELEMENTOS POST                         
                //===========================================================
                DatosRequest DatosPOST  = new DatosRequest();
                DatosPOST.QueryString   = Request.QueryString;
                DatosPOST.Form          = Request.Form;

                //===========================================================
                // RECEPCIÓN DE DATOS                       
                //===========================================================
                int ID_ASIGNACION = int.Parse(DatosPOST["ID_ASIGNACION"]);
                int ID_SISTEMA = int.Parse(DatosPOST["ID_SISTEMA"]);
                String INTERACION = DatosPOST["INTERACION"];
                String LOGIN = DatosPOST["LOGIN"];


                //===========================================================
                // LERR CANTIDAD Y DETALLE DE PANTALLAS                 
                //===========================================================
                LEER_PANTALLAS(ID_ASIGNACION, ID_SISTEMA, INTERACION, LOGIN);

                //===========================================================
                // LECTURA DE OBJETOS                      
                //===========================================================
                LEER_OBJETO(ID_ASIGNACION, ID_SISTEMA);
             
                return true;

            }catch{

                throw;
             
            }
        }


        /// <summary>
        /// Metodo que se encarga de invocar la funcion javascript que realizara la llamada al send-api local de mitrol
        /// </summary>
        /// <param name="idInteraccion">Identificador de la llamada en el pad</param>
        /// <param name="idTipoContacto">Identificador del medio de contacto utilizado</param>
        /// <param name="idCedente">Identificador del cedente al cual se le esta haciendo la gestion</param>
        private void GUARDAR_GESTION_MITROL(string idInteraccion, string idcrm, string idResultadoGestionExterno){
            try{
                //===========================================================
                // REALIZAR LLAMADA MITROL                         
                //===========================================================
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("function FGESDIS(){");
                sb.Append("GuardarGestionDiscador('" + idInteraccion + "','" + idcrm + "','" + idResultadoGestionExterno + "');");
                sb.Append("Sys.Application.remove_load(FGESDIS);}");
                sb.Append("Sys.Application.add_load(FGESDIS);");
                sb.Append("</script>");

                ScriptManager.RegisterStartupScript(this, typeof(Page), "GestionApiMitrol", sb.ToString(), false);
            }catch { }
        }


        /// <summary>
        /// VIEWSTATE PARA VARIABLES GLOBALES
        /// </summary>
        private void Establecer_Globales(){
            try{
                 ViewState["GlobalesFormulario"] = new GlobalesFormulario();
            }catch{
                throw;
            }
        }


        /// <summary>
        /// VIEWSTATE PARA VARIABLES GLOBALES
        /// </summary>
        /// <returns></returns>
        private GlobalesFormulario V_Global(){
            GlobalesFormulario item = new GlobalesFormulario();
            try{
                item = (GlobalesFormulario)ViewState["GlobalesFormulario"] ?? null;
                return item;
            }catch{
                return item;
            }

        }


        /// <summary>
        /// MENSAJE LOG      
        /// </summary>
        /// <param name="Mensaje"></param>
        /// <param name="Titulo"></param>
        private void MensajeLOG(string Mensaje, string Titulo){
            try{

                //===========================================================
                // CONTENIDO DIV CON RUNAT SERVER                       
                //===========================================================
                var DIV                      = (HtmlGenericControl)Page.Master.FindControl("LOG_MENSAJE_SERVER");
                DIV.InnerHtml                = WebUtility.HtmlDecode(Mensaje);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("function FLOG(){");
                sb.Append("MensajeLOG('" + Titulo + "');");
                sb.Append("Sys.Application.remove_load(FLOG);}");
                sb.Append("Sys.Application.add_load(FLOG);");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, typeof(Page), "PopupJS", sb.ToString(), false);
            }catch { }
        }


        /// <summary>
        /// TAB ACTIVO
        /// </summary>
        private void TabActivoAjax(){
            try{
                //===========================================================
                // TAB ACTIVO                       
                //===========================================================
                ScriptManager.RegisterStartupScript(this, this.GetType(), "TabBootstrap", "TAB_ACTIVO();", true);
            }catch { }
        }

        /// <summary>
        /// LEER PANTALLAS ASOCIADAS
        /// </summary>
        /// <param name="ID_ASIGNACION"></param>
        /// <param name="ID_SISTEMA"></param>
        private void LEER_PANTALLAS(int ID_ASIGNACION, int ID_SISTEMA,string INTERACCION , string LOGIN){

            //===========================================================
            // DECLARACION DE VARIABLES
            //===========================================================
            SMetodos Servicio = new SMetodos();
            List<oSP_READ_PANTALLA_X_ASIGNACION> Cantidad = new List<oSP_READ_PANTALLA_X_ASIGNACION>();

            //===========================================================
            // PARAMETROS DE ENTRADA 
            //===========================================================
            iSP_READ_PANTALLA_X_ASIGNACION ParametrosInput = new iSP_READ_PANTALLA_X_ASIGNACION();
            ParametrosInput.ID_ASIGNACION = ID_ASIGNACION;
            ParametrosInput.ID_SISTEMA    = ID_SISTEMA;

            //===========================================================
            // LLAMADA DEL SERVICIO                                    
            //===========================================================
            Cantidad = Servicio.SP_READ_PANTALLA_X_ASIGNACION(ParametrosInput);

            //===========================================================
            // CREACION  OBJETO PANTALLA                     
            //===========================================================
            List<PANTALLA> LST_PANTALLA = new List<PANTALLA>();
           
            foreach (var c in Cantidad){
                PANTALLA pantalla = new PANTALLA();
                pantalla.ID_ASIGNACION = ID_ASIGNACION;
                pantalla.ID_SISTEMA    = ID_SISTEMA;
                pantalla.INTERACCION   = INTERACCION;
                pantalla.LOGIN         = LOGIN;
                pantalla.Q_PANTALLA    = c.Q_PANTALLA;
                pantalla.ORDEN         = c.ORDEN;
                pantalla.DESCRIPCION   = c.DESCRIPCION;

                LST_PANTALLA.Add(pantalla);
            }

            //===========================================================
            // ASIGNACION VARIABLE GLOBAL PANTALLA                   
            //===========================================================
            V_Global().LST_PANTALLA = LST_PANTALLA;

        }


        /// <summary>
        /// LEER OBJETOS
        /// </summary>
        /// <param name="ID_ASIGNACION"></param>
        /// <param name="ID_SISTEMA"></param>
        private void LEER_OBJETO(int ID_ASIGNACION, int ID_SISTEMA){
            try{
                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos();
                List<oSP_READ_OBJETO_X_OPCION> LST_OBJETO = new List<oSP_READ_OBJETO_X_OPCION>();
                List<CONTROL> LST_CONTROLES               = new List<CONTROL>();

                //===========================================================
                // PARAMETROS DE ENTRADA 
                //===========================================================
                iSP_READ_OBJETO_X_OPCION ParametrosInput = new iSP_READ_OBJETO_X_OPCION();
                ParametrosInput.ID_ASIGNACION            = ID_ASIGNACION;
                ParametrosInput.ID_SISTEMA               = ID_SISTEMA;

                //===========================================================
                // LLAMADA DEL SERVICIO                                    
                //===========================================================
                LST_OBJETO = Servicio.SP_READ_OBJETO_X_OPCION(ParametrosInput);

                if (LST_OBJETO == null){
                    throw new Exception("NO EXISTE ASIGNACIÓN POR OBJETOS");
                }

                if (LST_OBJETO.Count <= 0){
                    throw new Exception("NO EXISTE ASIGNACIÓN POR OBJETOS");
                }

                //===========================================================
                // ITERACION POR SERVICIO                            
                //===========================================================
                foreach (oSP_READ_OBJETO_X_OPCION item in LST_OBJETO){
                    //=======================================================
                    // DEFINO ENTIDAD PRINCIPAL 
                    //=======================================================
                    CONTROL Objeto          = new CONTROL();
                    Objeto.ID_OBJETO        = item.ID_OBJETO;
                    Objeto.PARENT_ID        = item.PARENT_ID;
                    Objeto.CODIGO           = item.CODIGO;
                    Objeto.DESCRIPCION      = item.DESCRIPCION;
                    Objeto.ID_TIPO_OBJETO   = item.ID_TIPO_OBJETO;
                    Objeto.ID_FORMULARIO    = item.ID_FORMULARIO;
                    Objeto.VALOR            = item.VALOR;
                    Objeto.OBLIGATORIO      = item.OBLIGATORIO;
                    Objeto.PANTALLA         = item.PANTALLA;

                    //=======================================================
                    // OBJETO COMBO
                    //=======================================================
                    if (item.ID_TIPO_OBJETO == 1){

                        OBJETO_DROPDOWLIST OBJ_COMBO = new OBJETO_DROPDOWLIST();

                        List<oSP_READ_OBJETO_X_COMBO> LST_OBJ = new List<oSP_READ_OBJETO_X_COMBO>();
                        LST_OBJ = Servicio.SP_READ_OBJETO_X_COMBO(new iSP_READ_OBJETO_X_COMBO { ID_OBJETO = item.ID_OBJETO });

                        if (LST_OBJ == null){
                      
                            throw new Exception("NO EXISTE CONFIGURACIÓN DE OBJETO");
                        
                        }

                        if (LST_OBJ.Count <= 0){
                        
                            throw new Exception("NO EXISTE CONFIGURACIÓN DE OBJETO");

                        }

                        OBJ_COMBO.LABEL     = LST_OBJ.First().LABEL;
                        OBJ_COMBO.ID_TABLA  = LST_OBJ.First().ID_TABLA;
                        OBJ_COMBO.VALOR     = item.VALOR;

                        //===================================================
                        // DATOS DE TABLA
                        //===================================================

                        DataTable TBL = Servicio.SP_READ_TABLA_PIVOT_WEB(new iSP_READ_TABLA_PIVOT_WEB { 
                                                                        ID_TABLA = LST_OBJ.First().ID_TABLA ,
                                                                        ID_OBJETO = item.ID_OBJETO
                        });
                        OBJ_COMBO.TABLA = TBL;
                       
                        //===================================================
                        // FILTRO
                        //===================================================
                        List<oSP_READ_FILTRO> LST_FILTRO = new List<oSP_READ_FILTRO>();
                        LST_FILTRO = Servicio.SP_READ_FILTRO(new iSP_READ_FILTRO { ID_OBJETO = item.ID_OBJETO });

                        if (LST_FILTRO != null){

                            if (LST_FILTRO.Count > 0){
                                
                                OBJ_COMBO.FILTRO = new List<FILTROS>();

                                foreach (oSP_READ_FILTRO FIL in LST_FILTRO){
                                    FILTROS ENTIDAD   = new FILTROS();
                                    ENTIDAD.COLUMNA   = FIL.COLUMNA;
                                    ENTIDAD.FILTRO    = FIL.TIPO_FILTRO;
                                    ENTIDAD.PARAMETRO = FIL.PARAMETRO;

                                    OBJ_COMBO.FILTRO.Add(ENTIDAD);

                                }

                            }else{

                                OBJ_COMBO.FILTRO = new List<FILTROS>();

                            }

                        }else{

                            OBJ_COMBO.FILTRO = new List<FILTROS>();

                        }

                        //===================================================
                        // PARAMETROS
                        //===================================================
                        List<oSP_READ_PARAMETROS_OUTPUT> LST_OUTPUT = new List<oSP_READ_PARAMETROS_OUTPUT>();
                        LST_OUTPUT = Servicio.SP_READ_PARAMETROS_OUTPUT(new iSP_READ_PARAMETROS_OUTPUT { ID_OBJETO = item.ID_OBJETO });

                        if (LST_OUTPUT != null){
                          
                            if (LST_OUTPUT.Count > 0){
                            
                                OBJ_COMBO.OUTPUT = new List<PARAMETROS_OUTPUT>();

                                foreach (oSP_READ_PARAMETROS_OUTPUT P_OUT in LST_OUTPUT){

                                    PARAMETROS_OUTPUT ENTIDAD = new PARAMETROS_OUTPUT();
                                    ENTIDAD.COLUMNA           = P_OUT.COLUMNA;
                                    ENTIDAD.ID_TIPO_HTML      = P_OUT.ID_TIPO_HTML;
                                    ENTIDAD.ID_TIPO_DATO      = P_OUT.ID_TIPO_DATO;
                                    ENTIDAD.VISIBLE           = P_OUT.VISIBLE;
                                    OBJ_COMBO.OUTPUT.Add(ENTIDAD);
                                }

                            }else{
                                
                                OBJ_COMBO.OUTPUT = new List<PARAMETROS_OUTPUT>();
                            
                            }

                        }else{
                            
                            OBJ_COMBO.OUTPUT = new List<PARAMETROS_OUTPUT>();
                        }

                        Objeto.OBJETO = OBJ_COMBO;
                    }

                    //=======================================================
                    // OBJETO GRILLA
                    //=======================================================
                    if (item.ID_TIPO_OBJETO == 2){

                        OBJETO_GRILLA OBJ_GRILLA = new OBJETO_GRILLA();

                        List<oSP_READ_OBJETO_X_GRILLA> LST_OBJ = new List<oSP_READ_OBJETO_X_GRILLA>();
                        LST_OBJ = Servicio.SP_READ_OBJETO_X_GRILLA(new iSP_READ_OBJETO_X_GRILLA { ID_OBJETO = item.ID_OBJETO });

                        if (LST_OBJ == null){
                            
                            throw new Exception("NO EXISTE CONFIGURACIÓN DE OBJETO");
                        }

                        if (LST_OBJ.Count <= 0){
                            
                            throw new Exception("NO EXISTE CONFIGURACIÓN DE OBJETO");
                        }

                        OBJ_GRILLA.LABEL          = LST_OBJ.First().LABEL;
                        OBJ_GRILLA.ID_TABLA       = LST_OBJ.First().ID_TABLA;
                        OBJ_GRILLA.ID_TIPO_GRILLA = LST_OBJ.First().ID_TIPO_GRILLA;

                        //===================================================
                        // VALIDO TIPO DE GRILLA MILTI SELECCION
                        //===================================================          
                        if (OBJ_GRILLA.ID_TIPO_GRILLA == 3 ) {

                            //===========================================================
                            // DATOS DE TABLA + EL CAMPO CHECKED DE LA GRILLA MULTISELECT
                            //===========================================================

                            DataTable TBL = Servicio.SP_READ_TABLA_PIVOT_WEB_GRV_MULTICHECKED(new iSP_READ_TABLA_PIVOT_WEB_GRV_MULTICHECKED { ID_TABLA = LST_OBJ.First().ID_TABLA ,
                                                                                                                                              ID_OBJETO = item.ID_OBJETO });
                            OBJ_GRILLA.TABLA = TBL;
                            

                        }else if(OBJ_GRILLA.ID_TIPO_GRILLA == 4){
                            //===========================================================
                            // DATOS DE TABLA Y VALORES POR DEFECTO
                            //===========================================================

                            DataTable TBL = Servicio.SP_READ_TABLA_PIVOT_WEB_GRV_DINAMIC(new iSP_READ_TABLA_PIVOT_WEB_GRV_DINAMIC{ ID_TABLA = LST_OBJ.First().ID_TABLA ,
                                                                                                                                              ID_OBJETO = item.ID_OBJETO });
                            OBJ_GRILLA.TABLA = TBL;



                        }else{
                            //===================================================
                            // DATOS DE TABLA
                            //===================================================
                            DataTable TBL = Servicio.SP_READ_TABLA_PIVOT_WEB(new iSP_READ_TABLA_PIVOT_WEB { ID_TABLA = LST_OBJ.First().ID_TABLA});
                            OBJ_GRILLA.TABLA = TBL;

                        }

                        //===================================================
                        // FILTRO
                        //===================================================
                        List<oSP_READ_FILTRO> LST_FILTRO = new List<oSP_READ_FILTRO>();
                        LST_FILTRO = Servicio.SP_READ_FILTRO(new iSP_READ_FILTRO { ID_OBJETO = item.ID_OBJETO });

                        if (LST_FILTRO != null){

                            if (LST_FILTRO.Count > 0){
                            
                                OBJ_GRILLA.FILTRO = new List<FILTROS>();
                                
                                foreach (oSP_READ_FILTRO FIL in LST_FILTRO){
                                    FILTROS ENTIDAD   = new FILTROS();
                                    ENTIDAD.COLUMNA   = FIL.COLUMNA;
                                    ENTIDAD.FILTRO    = FIL.TIPO_FILTRO;
                                    ENTIDAD.PARAMETRO = FIL.PARAMETRO;

                                    OBJ_GRILLA.FILTRO.Add(ENTIDAD);
                                }

                            }else{
                                
                                OBJ_GRILLA.FILTRO = new List<FILTROS>();
                            
                            }

                        }else{
                            
                            OBJ_GRILLA.FILTRO = new List<FILTROS>();
                        
                        }

                        //===================================================
                        // PARAMETROS
                        //===================================================
                        List<oSP_READ_PARAMETROS_OUTPUT> LST_OUTPUT = new List<oSP_READ_PARAMETROS_OUTPUT>();
                        LST_OUTPUT = Servicio.SP_READ_PARAMETROS_OUTPUT(new iSP_READ_PARAMETROS_OUTPUT { ID_OBJETO = item.ID_OBJETO });

                        if (LST_OUTPUT != null){
                        
                            if (LST_OUTPUT.Count > 0){
                            
                                OBJ_GRILLA.OUTPUT = new List<PARAMETROS_OUTPUT>();
                                
                                foreach (oSP_READ_PARAMETROS_OUTPUT P_OUT in LST_OUTPUT){
                                    PARAMETROS_OUTPUT ENTIDAD = new PARAMETROS_OUTPUT();
                                    ENTIDAD.COLUMNA                  = P_OUT.COLUMNA;
                                    ENTIDAD.VISIBLE                  = P_OUT.VISIBLE;
                                    ENTIDAD.ID_TIPO_HTML             = P_OUT.ID_TIPO_HTML;
                                    ENTIDAD.ID_TIPO_DATO             = P_OUT.ID_TIPO_DATO;
                                    ENTIDAD.ID_TABLA_GRV_DINAMICA    = P_OUT.ID_TABLA_GRV_DINAMICA;
                                    //===================================================
                                    // SI LA GRILLA TIENE UN OBJETO DDL
                                    //===================================================
                                    if (P_OUT.ID_TIPO_DATO == 5)
                                    {



                                        //===================================================
                                        // OBTENGO LOS DATOS DE LA TABLA 
                                        //===================================================

                                        DataTable TBL                   = Servicio.SP_READ_TABLA_PIVOT_WEB_GRV_DINAMICA_DDL(new iSP_READ_TABLA_PIVOT_WEB_GRV_DINAMICA_DDL
                                        {
                                            ID_TABLA_GRV_DINAMICA       = P_OUT.ID_TABLA_GRV_DINAMICA,

                                        });




                                        ////===================================================
                                        //// OBTENGO LAS COLUMNAS DEL OBJETO DDL GRV DINAMICA
                                        ////===================================================
                                        OBJETO_GRV_DINAMICA_DDL OBJETO_GRV_DIN_DDL = new OBJETO_GRV_DINAMICA_DDL();
                                        List<oSP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL> LST_OUTPUT_GRV_DINAMICA_DDL = new List<oSP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL>();
                                        LST_OUTPUT_GRV_DINAMICA_DDL                = Servicio.SP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL(new iSP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL
                                        {
                                                         ID_COLUMNA                = P_OUT.ID_COLUMNA,
                                        });

                                        OBJETO_GRV_DIN_DDL.OUTPUT                  = new List<PARAMETROS_OUTPUT_GRV_DINAMICA>();

                                        foreach (oSP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL P_OUT_GRV_DINAMICA_DDL in LST_OUTPUT_GRV_DINAMICA_DDL)
                                        {

                                            PARAMETROS_OUTPUT_GRV_DINAMICA ENTIDAD_GRV_DINAMICA_DDL = new PARAMETROS_OUTPUT_GRV_DINAMICA();
                                            ENTIDAD_GRV_DINAMICA_DDL.COLUMNA                        = P_OUT_GRV_DINAMICA_DDL.COLUMNA;
                                            ENTIDAD_GRV_DINAMICA_DDL.ID_TIPO_HTML                   = P_OUT_GRV_DINAMICA_DDL.ID_TIPO_HTML;
                                            ENTIDAD_GRV_DINAMICA_DDL.ID_TIPO_DATO                   = P_OUT_GRV_DINAMICA_DDL.ID_TIPO_DATO;
                                            ENTIDAD_GRV_DINAMICA_DDL.VISIBLE                        = P_OUT_GRV_DINAMICA_DDL.VISIBLE;
                                            ENTIDAD_GRV_DINAMICA_DDL.ID_TABLA_GRV_DINAMICA          = P_OUT.ID_TABLA_GRV_DINAMICA;
                                            OBJETO_GRV_DIN_DDL.OUTPUT.Add(ENTIDAD_GRV_DINAMICA_DDL);
                                        }

                                        OBJETO_GRV_DIN_DDL.TABLA                                    = TBL;
                                        ENTIDAD.OBJETO_DDL_GRV_DINAMICA                             = OBJETO_GRV_DIN_DDL;

                                    }


                                    OBJ_GRILLA.OUTPUT.Add(ENTIDAD);

                                }

                            }else{
                                
                                OBJ_GRILLA.OUTPUT = new List<PARAMETROS_OUTPUT>();
                            
                            }

                        }else{
                            
                            OBJ_GRILLA.OUTPUT = new List<PARAMETROS_OUTPUT>();
                        
                        }

                        OBJ_GRILLA.SELECCIONADO = new List<SELECCION>();
                        Objeto.OBJETO           = OBJ_GRILLA;
                    }


                    //=======================================================
                    // OBJETO TEXTBOX
                    //=======================================================
                    if (item.ID_TIPO_OBJETO == 3){

                        OBJETO_TEXTBOX OBJ_TEXTBOX = new OBJETO_TEXTBOX();

                        List<oSP_READ_OBJETO_X_TEXTBOX> LST_OBJ = new List<oSP_READ_OBJETO_X_TEXTBOX>();
                        LST_OBJ = Servicio.SP_READ_OBJETO_X_TEXTBOX(new iSP_READ_OBJETO_X_TEXTBOX { ID_OBJETO = item.ID_OBJETO });


                        if (LST_OBJ == null){
                        
                            throw new Exception("NO EXISTE CONFIGURACIÓN DE OBJETO");

                        }

                        if (LST_OBJ.Count <= 0){

                            throw new Exception("NO EXISTE CONFIGURACIÓN DE OBJETO");

                        }

                        //=======================================================
                        // LABEL VALIDA LARGO PERMITIDO
                        //=======================================================
                        string LABEL_DINAMICO = "";

                        if (LST_OBJ.First().ID_TIPO_DATO == 1) {
                            LABEL_DINAMICO = LST_OBJ.First().LABEL + " (Min.: " + LST_OBJ.First().MIN_CARACTERES.ToString() + " Max.:" + LST_OBJ.First().MAX_CARACTERES.ToString() + ")";
                        }
                        else {
                            LABEL_DINAMICO = LST_OBJ.First().LABEL;
                        }


                        //=======================================================
                        // LABEL VALIDA OBLIGATORIO
                        //=======================================================

                        if (item.OBLIGATORIO == true){
                            OBJ_TEXTBOX.LABEL = LABEL_DINAMICO.ToString() + " (Obligatorio)" ;
                        }else{
                            OBJ_TEXTBOX.LABEL = LABEL_DINAMICO;
                        }
                        
                        OBJ_TEXTBOX.ID_TIPO_DATO   = LST_OBJ.First().ID_TIPO_DATO;
                        OBJ_TEXTBOX.VALOR          = item.VALOR;
                        OBJ_TEXTBOX.MIN_CARACTERES = LST_OBJ.First().MIN_CARACTERES;
                        OBJ_TEXTBOX.MAX_CARACTERES = LST_OBJ.First().MAX_CARACTERES;
                        Objeto.OBJETO              = OBJ_TEXTBOX;
                    }


                    //=======================================================
                    // OBJETO CHECKBOX
                    //=======================================================
                    if (item.ID_TIPO_OBJETO == 4){

                        OBJETO_CHECKBOX OBJ_CHK                  = new OBJETO_CHECKBOX();
                        List<oSP_READ_OBJETO_X_CHECKBOX> LST_OBJ = new List<oSP_READ_OBJETO_X_CHECKBOX>();
                        LST_OBJ                                  = Servicio.SP_READ_OBJETO_X_CHECKBOX(new iSP_READ_OBJETO_X_CHECKBOX { ID_OBJETO = item.ID_OBJETO });


                        if (LST_OBJ == null){

                            throw new Exception("NO EXISTE CONFIGURACIÓN DE OBJETO");

                        }

                        if (LST_OBJ.Count <= 0){

                            throw new Exception("NO EXISTE CONFIGURACIÓN DE OBJETO");

                        }

                        OBJ_CHK.LABEL = LST_OBJ.First().LABEL;
                        OBJ_CHK.VALOR = item.VALOR;
                        Objeto.OBJETO = OBJ_CHK;

                    }
                    LST_CONTROLES.Add(Objeto);
                }

                //===========================================================
                // ASIGNAR VIEWSTATE                                  
                //===========================================================
           
                V_Global().LST_CONTROLES = LST_CONTROLES;

            }catch(Exception EX){
                
                throw;
            
            }
        }


        /// <summary>
        /// LEER OBJETOS
        /// </summary>
        /// <param name="ID_ASIGNACION"></param>
        /// <param name="ID_SISTEMA"></param>
        private void LEER_OBJETO_NO_POSTBACK(){
            try{
              
                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos();
                List<CONTROL> LST_CONTROLES = new List<CONTROL>();

                //===========================================================
                // OBTENER VIEWSTATE
                //===========================================================
                LST_CONTROLES = V_Global().LST_CONTROLES;

                //===========================================================
                // ITERACION POR SERVICIO                            
                //===========================================================
               foreach (CONTROL Objeto in LST_CONTROLES){

                    //=======================================================
                    // PANEL
                    //=======================================================
                    string PANEL_FIND  = "PNL_" + Objeto.PANTALLA;
                    Panel PNL_PANTALLA = (Panel)FindControlRecursive(PNL_DINAMICO, PANEL_FIND);
  
                    //=======================================================
                    // OBJETO COMBO
                    //=======================================================
                    if (Objeto.ID_TIPO_OBJETO == 1){
            
                        OBJETO_DROPDOWLIST OBJ_COMBO = new OBJETO_DROPDOWLIST();
                        OBJ_COMBO = (OBJETO_DROPDOWLIST)Objeto.OBJETO;

                        Label LBL_CTR   = new Label();
                        LBL_CTR.Text    = OBJ_COMBO.LABEL;
                        LBL_CTR.ID      = "LBL_" + Objeto.ID_OBJETO;

                        string DataValueField = OBJ_COMBO.OUTPUT.Where(d => d.ID_TIPO_HTML == 2).First().COLUMNA;
                        string DataTextField  = OBJ_COMBO.OUTPUT.Where(d => d.ID_TIPO_HTML == 3).First().COLUMNA;

                        DropDownList DDL_CTR = new DropDownList();
                        DDL_CTR.ID                  = "DDL_" + Objeto.ID_OBJETO;
                        DDL_CTR.DataValueField      = DataValueField;
                        DDL_CTR.DataTextField       = DataTextField;              
                        DDL_CTR.Attributes["style"] = "width: 100%;";
                        DDL_CTR.Attributes.Add("Class", "form-control");
                        
                        //===========================================================================
                        // VALIDO SI EL OBJETO ES UN OBJETO PADRE PARA ASIGNARLE AUTOPOSTBACK 
                        //===========================================================================

                        if (OBJ_COMBO.FILTRO.Count() > 0){

                            DDL_CTR.AutoPostBack          = true;
                            DDL_CTR.SelectedIndexChanged += new EventHandler(DDLSelectedIndexChanged);

                        }

                        //=======================================================
                        // VALIDA SI ES UN OBJETO HIJO 
                        //=======================================================
                        if (Objeto.PARENT_ID > 0){
                            //=======================================================
                            // SE BUSCA EL OBJETO PADRE
                            //=======================================================
                            OBJETO_DROPDOWLIST OBJ_COMBO_PADRE = new OBJETO_DROPDOWLIST();
                            OBJ_COMBO_PADRE = (OBJETO_DROPDOWLIST)V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == Objeto.PARENT_ID).First().OBJETO;

                            string CAMPO_FILTRAR = OBJ_COMBO_PADRE.FILTRO.First().COLUMNA;
                            string TIPO_FILTRO   = OBJ_COMBO_PADRE.FILTRO.First().FILTRO;

                            //=======================================================
                            // BUSCAR VALOR SELECCIONADO DEL PADRE
                            //=======================================================
                            //string VALOR_FILTRAR = OBJ_COMBO_PADRE.VALOR.ToString();
                            DropDownList dropDown   = (DropDownList)FindControlRecursive(PNL_DINAMICO, "DDL_" + Objeto.PARENT_ID);
                            string VALOR_FILTRAR    = dropDown.SelectedValue.ToString();

                            //=======================================================
                            // SE GENERA FILTRO PARA CREAR DATOS DEL HIJO FILTRADO 
                            //=======================================================
                            string QUERY_FILTRO      = CAMPO_FILTRAR + " "+ TIPO_FILTRO + " " + VALOR_FILTRAR;
                            DataTable TABLA_FILTRADA = OBJ_COMBO.TABLA.Select(QUERY_FILTRO).CopyToDataTable();
                            DDL_CTR.DataSource       = TABLA_FILTRADA;
                            DDL_CTR.ClientIDMode     = System.Web.UI.ClientIDMode.Static;
                            DDL_CTR.DataBind();
                            string VALUE_DEFAULT     = "";
                            if (OBJ_COMBO.VALOR.ToString() == "")
                            {
                                //========================================================
                                // SI NO TIENE VALOR SELECCIONADO SE SELECCIONA EL PRIMERO
                                //========================================================
                                VALUE_DEFAULT         = TABLA_FILTRADA.Rows[0][DataValueField].ToString();
                                DDL_CTR.SelectedValue = VALUE_DEFAULT;
                                V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == Objeto.ID_OBJETO).First().VALOR = VALUE_DEFAULT;

                            }else{
                                //==================================================================
                                // SI TIENE  VALOR SELECCIONADO SE BUSCA SI EXISTE EN LA NUEVA TABLA
                                //==================================================================
                                foreach (DataRow row in TABLA_FILTRADA.Rows)
                                {
                                    if(row[DataValueField].ToString() == OBJ_COMBO.VALOR.ToString())
                                    {
                                        VALUE_DEFAULT = OBJ_COMBO.VALOR.ToString();

                                    } 

                                }

                                //======================================================================================
                                // SI LA TABLA NO TIENE EL VALOR GUARDADO SE SELECCIONA EL PRIMERO DE LA TABLA FILTRADA
                                //======================================================================================

                                if (VALUE_DEFAULT =="")
                                {
                                    DDL_CTR.SelectedValue = TABLA_FILTRADA.Rows[0][DataValueField].ToString();
                                }else{
                                    DDL_CTR.SelectedValue = OBJ_COMBO.VALOR.ToString();
                                }
                            }

                        }else{
                            //=======================================================
                            // SE INSERTA TABLA SIN FILTRAR
                            //=======================================================
                            DDL_CTR.DataSource      = OBJ_COMBO.TABLA;
                            DDL_CTR.ClientIDMode    = System.Web.UI.ClientIDMode.Static;
                            DDL_CTR.DataBind();
      

                            if (OBJ_COMBO.VALOR.ToString() == "")
                            {
                                //========================================================
                                // SI NO TIENE VALOR SELECCIONADO SE SELECCIONA EL PRIMERO
                                //========================================================
                                string VALUE_DEFAULT            = OBJ_COMBO.TABLA.Rows[0][DataValueField].ToString();
                                DDL_CTR.SelectedValue           = VALUE_DEFAULT;
                                V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == Objeto.ID_OBJETO).First().VALOR = VALUE_DEFAULT;
   
                            }else{
                                //========================================================
                                // SI TIENE  VALOR SELECCIONADO , SE LE ASIGNA
                                //========================================================
                                DDL_CTR.SelectedValue = OBJ_COMBO.VALOR.ToString();
                            }

                        }



                        CrearControl(DDL_CTR, LBL_CTR, PNL_PANTALLA,1);
                    }

                    //=======================================================
                    // OBJETO GRILLA
                    //=======================================================
                    if (Objeto.ID_TIPO_OBJETO == 2){

                        OBJETO_GRILLA OBJ_GRILLA    = new OBJETO_GRILLA();
                        OBJ_GRILLA                  = (OBJETO_GRILLA)Objeto.OBJETO;
                        Objeto.OBJETO               = OBJ_GRILLA;

                        Label LBL_CTR               = new Label();
                        LBL_CTR.Text                = OBJ_GRILLA.LABEL;
                        LBL_CTR.ID                  = "LBL_" + Objeto.ID_OBJETO;

                        GridView GRV_CTR            = new GridView();
                        GRV_CTR.ID                  = "GRV_" + Objeto.ID_OBJETO;
                        GRV_CTR.Attributes["style"] = "width: 100%;";


                        //=============================================================================
                        // SE VALIDA EL TIPO DE GRILLA 1 INFORMATIVA 2 SELECCION 3 SELECCION MULTIPLE 4 DINAMICA
                        //=============================================================================                 

                        if (OBJ_GRILLA.ID_TIPO_GRILLA == 1){

                            //===========================================================================
                            // SE AGREGA LAS COLUMNAS 
                            //===========================================================================
                            GRV_CTR.PreRender += new EventHandler(GRDDinamica_PreRender);

                            foreach (var c in OBJ_GRILLA.TABLA.Columns){
                                BoundField bound        = new BoundField();
                                bound.DataField         = c.ToString();
                                bound.HeaderText        = c.ToString();
                                bound.SortExpression    = c.ToString();
                                GRV_CTR.Columns.Add(bound);
                            }

                        }else if (OBJ_GRILLA.ID_TIPO_GRILLA == 2){
                            //===========================================================================
                            // SI ES GRILLA DE SELECCION SE INSERTA EL BOTON
                            //===========================================================================

                                GRV_CTR.RowCommand             += new GridViewCommandEventHandler(GRDDinamica_RowCommand);
                                GRV_CTR.RowDataBound           += new GridViewRowEventHandler(GRDDinamica_RowDataBound);
                                GRV_CTR.PreRender              += new EventHandler(GRDDinamica_PreRender);
                                ButtonField BTN_LNK             = new ButtonField();
                                BTN_LNK.ButtonType              = ButtonType.Link;
                                BTN_LNK.HeaderText              = "OPCIONES";
                                BTN_LNK.ControlStyle.CssClass   = "btn btn-default";
                                BTN_LNK.Text                    = "<span class='glyphicon glyphicon-ok-circle' aria-hidden='true' ></span>";
                                BTN_LNK.CommandName             = "SELECCION";
                                GRV_CTR.Columns.Add(BTN_LNK);

                            //===========================================================================
                            // SE AGREGA LAS COLUMNAS 
                            //===========================================================================
                            foreach (var c in OBJ_GRILLA.TABLA.Columns){
                                BoundField bound     = new BoundField();
                                bound.DataField      = c.ToString();
                                bound.HeaderText     = c.ToString();
                                bound.SortExpression = c.ToString();                               
                                GRV_CTR.Columns.Add(bound);
                            }

                        }else if (OBJ_GRILLA.ID_TIPO_GRILLA == 3){
                            //===========================================================================
                            // SE AGREGA EL CAMPO CHECKED
                            //===========================================================================
                            GRV_CTR.RowDataBound          += new GridViewRowEventHandler(GRDDinamica_RowDataBound);
                            GRV_CTR.RowCommand            += new GridViewCommandEventHandler(GRDDinamica_RowCommand);
                            GRV_CTR.PreRender             += new EventHandler(GRDDinamica_PreRender);
                            TemplateField customField      = new TemplateField();
                            customField.ItemTemplate       = new GridViewTemplate(DataControlRowType.DataRow, "VALOR");
                            customField.HeaderTemplate     = new GridViewTemplate(DataControlRowType.Header, "VALOR");
                            GRV_CTR.Columns.Add(customField);

                            //===========================================================================
                            // SE AGREGA LAS COLUMNAS 
                            //===========================================================================
                            foreach (var c in OBJ_GRILLA.TABLA.Columns){
                                BoundField bound        = new BoundField();
                                bound.DataField         = c.ToString();
                                bound.HeaderText        = c.ToString();
                                bound.SortExpression    = c.ToString();
                                //===========================================================================
                                // NO DIBUJO EL CAMPO VALOR 
                                //===========================================================================
                                if (c.ToString() == "VALOR")
                                {
                                    bound.Visible = false;
                                }

                                GRV_CTR.Columns.Add(bound);
                            }
                            
                        }if (OBJ_GRILLA.ID_TIPO_GRILLA == 4){

                            //===========================================================================
                            // AGREGAR CAMPO DE SELECCION
                            //===========================================================================
                            TemplateField TEMPLATE_SELECCION = new TemplateField();

                            TEMPLATE_SELECCION.HeaderText           = "OPCIONES";
                            TEMPLATE_SELECCION.ItemTemplate         = new AddTemplateToGridView(ListItemType.Item, "OPCIONES", 0, Objeto.ID_OBJETO);
                            TEMPLATE_SELECCION.FooterTemplate = new AddTemplateToGridView(ListItemType.Footer, "OPCIONES", 0, Objeto.ID_OBJETO);
                            TEMPLATE_SELECCION.EditItemTemplate = new AddTemplateToGridView(ListItemType.EditItem, "OPCIONES", 0, Objeto.ID_OBJETO);
                            GRV_CTR.Columns.Add(TEMPLATE_SELECCION);

       
                            //===========================================================================
                            // AGREGAR COLUMNAS
                            //===========================================================================
                            foreach (var c in OBJ_GRILLA.TABLA.Columns){

                                TemplateField template     = new TemplateField();
                                int ID_TIPO_DATO;

                                if (c.ToString()     == "FILA"){
                                    template.Visible = false;
                                    ID_TIPO_DATO     = 1;
                                }else{
                                    ID_TIPO_DATO = OBJ_GRILLA.OUTPUT.Where(C => C.COLUMNA == c.ToString()).First().ID_TIPO_DATO;
                                }

                                template.HeaderText        = c.ToString();
                                template.ItemTemplate      = new AddTemplateToGridView(ListItemType.Item, c.ToString(), ID_TIPO_DATO, Objeto.ID_OBJETO);
                                template.EditItemTemplate  = new AddTemplateToGridView(ListItemType.EditItem, c.ToString(), ID_TIPO_DATO, Objeto.ID_OBJETO);

                                if (ID_TIPO_DATO == 5)
                                {
                                    PARAMETROS_OUTPUT OBJETO_DDL_GRV_DINAMICA = new PARAMETROS_OUTPUT();
                                    OBJETO_DDL_GRV_DINAMICA = OBJ_GRILLA.OUTPUT.Where(C => C.COLUMNA == c.ToString()).First();
                                    template.FooterTemplate = new AddTemplateToGridView(ListItemType.Footer, c.ToString(), ID_TIPO_DATO, Objeto.ID_OBJETO, OBJETO_DDL_GRV_DINAMICA);

                                }
                                else
                                {

                                    template.FooterTemplate = new AddTemplateToGridView(ListItemType.Footer, c.ToString(), ID_TIPO_DATO, Objeto.ID_OBJETO);
                                }





                                GRV_CTR.Columns.Add(template);

                            }

                            GRV_CTR.ShowFooter           = true;
                            GRV_CTR.ShowHeader           = true;
                            GRV_CTR.RowDataBound        += new GridViewRowEventHandler(GRDDinamica_RowDataBound);
                            GRV_CTR.RowDeleting         += new GridViewDeleteEventHandler(GRDDinamica_RowDeleting);
                            GRV_CTR.RowCommand          += new GridViewCommandEventHandler(GRDDinamica_RowCommand);
                            GRV_CTR.PreRender           += new EventHandler(GRDDinamica_PreRender);
                            GRV_CTR.RowEditing          += new GridViewEditEventHandler(GRDDinamica_RowEditing);
                            GRV_CTR.RowCancelingEdit    += new GridViewCancelEditEventHandler(GRDDinamica_RowCancelingEdit);
                            GRV_CTR.RowUpdating         += new GridViewUpdateEventHandler(GRDDinamica_RowUpdating);
                        }

               
                        GRV_CTR.AutoGenerateColumns = false;
                        GRV_CTR.CellPadding         = 4;
                        GRV_CTR.ClientIDMode        = System.Web.UI.ClientIDMode.Static;
                        GRV_CTR.CssClass            = "table table-striped table-bordered table-hover tablaConBuscador";
                        GRV_CTR.ShowHeaderWhenEmpty = true;
                        GRV_CTR.AllowPaging         = false;

                        //=======================================================
                        // VERIFICA SI ES UN OBJETO HIJO
                        //=======================================================
                        if (Objeto.PARENT_ID > 0){

                            List<oSP_READ_FILTRO> LST_FILTRO = new List<oSP_READ_FILTRO>();
                            //=======================================================
                            // SE BUSCA LOS FILTRO APLICADOS AL OBJETO HIJO 
                            //=======================================================
                            LST_FILTRO = Servicio.SP_READ_FILTRO(new iSP_READ_FILTRO { ID_OBJETO = Objeto.PARENT_ID });

                            string CAMPO_FILTRAR = LST_FILTRO.First().COLUMNA;
                            string TIPO_FILTRO   = LST_FILTRO.First().TIPO_FILTRO;

                            //=======================================================
                            // BUSCAR VALOR SELECCIONADO DEL PADRE
                            //=======================================================
                            string VALOR_FILTRAR = V_Global().LST_CONTROLES.Where(p => p.ID_OBJETO == Objeto.PARENT_ID).First().VALOR;

                            if (VALOR_FILTRAR == "" || VALOR_FILTRAR ==  null) {
                                //=======================================================
                                // PADRE NO TIENE VALOR SELECCIONADO
                                //=======================================================
                                GRV_CTR.DataSource = null;
                                GRV_CTR.DataBind();
                         
                            }else{
                                //=======================================================
                                // SE GENERA FILTRO PARA CREAR DATOS DEL HIJO FILTRADO 
                                //=======================================================
                                string QUERY_FILTRO         = CAMPO_FILTRAR + " " + TIPO_FILTRO + " " + VALOR_FILTRAR;
                                DataTable TABLA_FILTRADA    = OBJ_GRILLA.TABLA.Select(QUERY_FILTRO).CopyToDataTable();
                                GRV_CTR.DataSource          = TABLA_FILTRADA;
                                GRV_CTR.DataBind();
                            }

                        }else{

                            GRV_CTR.DataSource = OBJ_GRILLA.TABLA;
                            GRV_CTR.DataBind();
                      
                        
                        }
                     

                        CrearControl(GRV_CTR, LBL_CTR, PNL_PANTALLA,2);
                    }

                    //=======================================================
                    // OBJETO TEXTBOX
                    //=======================================================
                    if (Objeto.ID_TIPO_OBJETO == 3){

                        OBJETO_TEXTBOX OBJ_TEXTBOX = new OBJETO_TEXTBOX();
                        OBJ_TEXTBOX                 = (OBJETO_TEXTBOX)Objeto.OBJETO;

                        //===================================================
                        // CREAR OBJETOS SEGUN EL TIPO DE DATOS
                        //===================================================
                        if (OBJ_TEXTBOX.ID_TIPO_DATO == 1){

                            Label LBL_CTR    = new Label();
                            LBL_CTR.Text     = OBJ_TEXTBOX.LABEL;
                            LBL_CTR.ID       = "LBL_" + Objeto.ID_OBJETO;

                            TextBox TXT_CTR             = new TextBox();
                            TXT_CTR.ID                  = "TXT_" + Objeto.ID_OBJETO;
                            TXT_CTR.Text                = Objeto.VALOR;
                            TXT_CTR.Attributes["style"] = "width: 100%;";
                            TXT_CTR.Attributes.Add("Class", "form-control");
                            TXT_CTR.Attributes.Add("required", "true");
                            TXT_CTR.MaxLength           = OBJ_TEXTBOX.MAX_CARACTERES;

                            ObjetoTexto(TXT_CTR);

                            CrearControl(TXT_CTR, LBL_CTR, PNL_PANTALLA,3);

                        }
                        //=======================================================
                        // NUMERO                                                
                        //=======================================================
                        if (OBJ_TEXTBOX.ID_TIPO_DATO == 2){

                            Label LBL_CTR  = new Label();
                            LBL_CTR.Text   = OBJ_TEXTBOX.LABEL;
                            LBL_CTR.ID     = "LBL_" + Objeto.ID_OBJETO;

                            TextBox TXT_CTR             = new TextBox();
                            TXT_CTR.ID                  = "TXT_" + Objeto.ID_OBJETO;
                            TXT_CTR.Text                = Objeto.VALOR;
                            TXT_CTR.Attributes["style"] = "width: 100%;";
                            TXT_CTR.Attributes.Add("Class", "form-control");
                            TXT_CTR.Attributes.Add("required", "true");

                            ObjetoNumero(TXT_CTR);

                            CrearControl(TXT_CTR, LBL_CTR, PNL_PANTALLA,3);
                        }

                        //=======================================================
                        // FECHA                                                
                        //=======================================================
                        if (OBJ_TEXTBOX.ID_TIPO_DATO == 3){
                            Label LBL_CTR               = new Label();
                            LBL_CTR.Text                = OBJ_TEXTBOX.LABEL;
                            LBL_CTR.ID                  = "LBL_" + Objeto.ID_OBJETO;

                            TextBox TXT_CTR             = new TextBox();
                            TXT_CTR.ID                  = "TXT_" + Objeto.ID_OBJETO;
                            TXT_CTR.Text                = Objeto.VALOR;
                            TXT_CTR.Attributes["style"] = "width: 100%;";
                            TXT_CTR.Attributes.Add("Class", "form-control");
                            TXT_CTR.Attributes.Add("required", "true");

                            ObjetoFecha(TXT_CTR);

                            CrearControl(TXT_CTR, LBL_CTR, PNL_PANTALLA,3);

                        }

                        //=======================================================
                        // BOOL                                                
                        //=======================================================
                        if (OBJ_TEXTBOX.ID_TIPO_DATO == 4){
                            continue;
                        }
                    }

                    //=======================================================
                    // OBJETO CHECKBOX
                    //=======================================================
                    if (Objeto.ID_TIPO_OBJETO == 4){

                        OBJETO_CHECKBOX  OBJ_CHK    = new OBJETO_CHECKBOX();
                        OBJ_CHK                     = (OBJETO_CHECKBOX)Objeto.OBJETO;
                        Label LBL_CTR               = new Label();
                        LBL_CTR.Text                = OBJ_CHK.LABEL;
                        LBL_CTR.ID                  = "LBL_" + Objeto.ID_OBJETO;

                        CheckBox CHK_CTR            = new CheckBox();
                        CHK_CTR.ID                  = "CHK_" + Objeto.ID_OBJETO;
                        CHK_CTR.Attributes.Add("Class", "form-check");
                        CHK_CTR.Checked             = true;
                        CHK_CTR.Visible             = true;

                        if(Objeto.VALOR == ""){
                          Objeto.VALOR              = "false";
                        }

                        ObjetoBool(CHK_CTR, bool.Parse(Objeto.VALOR));
                        CrearControl(CHK_CTR, LBL_CTR, PNL_PANTALLA,4);
                    }
               }


            }
            catch(Exception EX){
                throw;
            }

        }


        /// <summary>
        /// ACTUALIZAR 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GRDDinamica_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {

                GridView GRD_PADRE       = (GridView)sender;
                SMetodos Servicio        = new SMetodos();
                int ID_OBJETO_PADRE      = int.Parse(GRD_PADRE.ClientID.Replace("GRV_", ""));
                CONTROL Objeto           = new CONTROL();
                Objeto                   = V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO_PADRE).First();
                OBJETO_GRILLA OBJ_GRILLA = new OBJETO_GRILLA();
                OBJ_GRILLA               = (OBJETO_GRILLA)Objeto.OBJETO;
                int ROW                  = int.Parse(OBJ_GRILLA.TABLA.Rows[GRD_PADRE.EditIndex]["FILA"].ToString());

                foreach (var c in OBJ_GRILLA.OUTPUT)
                {
                    int ID_TIPO_DATO = c.ID_TIPO_DATO;
                    string VALOR_GUARDAR = "";

                    if (ID_TIPO_DATO < 2)
                    {
                        //===========================================================================
                        // TEXTBOX COMUN
                        //===========================================================================
                        TextBox textBox      = (TextBox)FindControlRecursive(PNL_DINAMICO, "TXTDNC_ADD_" + c.COLUMNA.ToString() + "_" + ID_OBJETO_PADRE);
                        VALOR_GUARDAR        = textBox.Text;

                    }
                    else if (ID_TIPO_DATO == 2)
                    {
                        //===========================================================================
                        // TEXTBOX NUMERO
                        //===========================================================================
                        TextBox textBox      = (TextBox)FindControlRecursive(PNL_DINAMICO, "TXTDNC_ADD_" + c.COLUMNA.ToString() + "_" + ID_OBJETO_PADRE);
                        VALOR_GUARDAR        = textBox.Text;

                    }
                    else if (ID_TIPO_DATO == 3)
                    {
                        //===========================================================================
                        // TEXTBOX FECHA
                        //===========================================================================
                        TextBox textBox      = (TextBox)FindControlRecursive(PNL_DINAMICO, "TXTDNC_ADD_" + c.COLUMNA.ToString() + "_" + ID_OBJETO_PADRE);
                        VALOR_GUARDAR        = DateTime.Parse(textBox.Text).ToShortDateString();
                    }

                    else if (ID_TIPO_DATO == 4)
                    {
                        //===========================================================================
                        // CHECKBOX 
                        //===========================================================================
                        CheckBox checkBox    = (CheckBox)FindControlRecursive(PNL_DINAMICO, "CHKDNC_ADD_" + c.COLUMNA.ToString() + "_" + ID_OBJETO_PADRE);
                        VALOR_GUARDAR        = checkBox.Checked.ToString();
                    }
                    else if (ID_TIPO_DATO == 5)
                    {
                        //===========================================================================
                        // DROPDOWNLIST 
                        //===========================================================================
                        DropDownList dropDownList = (DropDownList)FindControlRecursive(PNL_DINAMICO, "DDLDNC_ADD_" + c.COLUMNA.ToString() + "_" + ID_OBJETO_PADRE);
                        VALOR_GUARDAR = dropDownList.SelectedValue.ToString();
                    }

                    iSP_UPDATE_ROW_GRILLA_DINAMICA parametro_actualizar = new iSP_UPDATE_ROW_GRILLA_DINAMICA();
                    parametro_actualizar.VALOR = VALOR_GUARDAR;
                    parametro_actualizar.VALOR_REFERENCIA = c.COLUMNA.ToString();
                    parametro_actualizar.PARENT_ID_CHK = (int)Objeto.ID_OBJETO;
                    parametro_actualizar.ROW = ROW;

                    Servicio.SP_UPDATE_ROW_GRILLA_DINAMICA(parametro_actualizar);
                }

                DataTable TBL = Servicio.SP_READ_TABLA_PIVOT_WEB_GRV_DINAMIC(new iSP_READ_TABLA_PIVOT_WEB_GRV_DINAMIC
                {
                    ID_TABLA = OBJ_GRILLA.ID_TABLA,
                    ID_OBJETO = Objeto.ID_OBJETO
                });

                //=======================================================
                // ACTUALIZO LA VARIABLE GLOBAL
                //=======================================================
                OBJ_GRILLA.TABLA       = TBL;
                V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO_PADRE).First().OBJETO = OBJ_GRILLA;

                //=======================================================
                // ACTUALIZO LA GRILLA
                //=======================================================
                GRD_PADRE.DataSource   = TBL;
                GRD_PADRE.EditIndex    = -1;
                GRD_PADRE.DataBind();
            }catch (Exception ex) { }
        }



        /// <summary>
        /// EDITAR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GRDDinamica_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                
                //===========================================================
                // DECLARACION DE OBJETOS                                          
                //===========================================================
                GridView GRD_PADRE = (GridView)sender;
                if (GRD_PADRE.EditIndex < 0){
                    //===========================================================
                    // CUANDO SE EDITA LA GRILLA                                         
                    //===========================================================
                    GRD_PADRE.EditIndex       = e.NewEditIndex;
                    GRD_PADRE.DataBind();
                    int ID_OBJETO_PADRE       = int.Parse(GRD_PADRE.ClientID.Replace("GRV_", ""));

                    //=======================================================
                    // OBTENGO LOS DATOS DE LA TABLA PADRE 
                    //=======================================================
                    CONTROL Objeto            = new CONTROL();
                    Objeto                    = V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO_PADRE).First();
                    OBJETO_GRILLA OBJ_GRILLA  = new OBJETO_GRILLA();
                    OBJ_GRILLA                = (OBJETO_GRILLA)Objeto.OBJETO;
                    int FILA                  = int.Parse(OBJ_GRILLA.TABLA.Rows[e.NewEditIndex]["FILA"].ToString());

                    foreach (var c in OBJ_GRILLA.OUTPUT){
                        int ID_TIPO_DATO = c.ID_TIPO_DATO;


                        if (ID_TIPO_DATO < 2)
                        {
                            //===========================================================================
                            // TEXTBOX COMUN
                            //===========================================================================
                            TextBox textBox = (TextBox)FindControlRecursive(PNL_DINAMICO, "TXTDNC_ADD_" + c.COLUMNA.ToString() + "_" + Objeto.ID_OBJETO);
                            textBox.Text = ((Label)FindControlRecursive(PNL_DINAMICO, "LBLDNC_" + c.COLUMNA.ToString() + "_" + FILA.ToString())).Text;
                            textBox.DataBind();

                        }
                        else if (ID_TIPO_DATO == 2)
                        {
                            //===========================================================================
                            // TEXTBOX NUMERO
                            //===========================================================================
                            TextBox textBox = (TextBox)FindControlRecursive(PNL_DINAMICO, "TXTDNC_ADD_" + c.COLUMNA.ToString() + "_" + Objeto.ID_OBJETO);
                            textBox.Text = ((Label)FindControlRecursive(PNL_DINAMICO, "LBLDNC_" + c.COLUMNA.ToString() + "_" + FILA.ToString())).Text;
                            textBox.DataBind();
                        }
                        else if (ID_TIPO_DATO == 3)
                        {
                            //===========================================================================
                            // TEXTBOX FECHA
                            //===========================================================================
                            TextBox textBox = (TextBox)FindControlRecursive(PNL_DINAMICO, "TXTDNC_ADD_" + c.COLUMNA.ToString() + "_" + Objeto.ID_OBJETO);
                            DateTime Fecha = Convert.ToDateTime(((Label)FindControlRecursive(PNL_DINAMICO, "LBLDNC_" + c.COLUMNA.ToString() + "_" + FILA.ToString())).Text);
                            textBox.Text = Fecha.ToString("yyyy-MM-dd");
                            textBox.DataBind();
                        }

                        else if (ID_TIPO_DATO == 4)
                        {
                            //===========================================================================
                            // CHECKBOX 
                            //===========================================================================
                            CheckBox checkBox = (CheckBox)FindControlRecursive(PNL_DINAMICO, "CHKDNC_ADD_" + c.COLUMNA.ToString() + "_" + Objeto.ID_OBJETO);
                            checkBox.Checked = Boolean.Parse(((Label)FindControlRecursive(PNL_DINAMICO, "LBLDNC_" + c.COLUMNA.ToString() + "_" + FILA.ToString())).Text);
                            checkBox.DataBind();
                        }
                        else if (ID_TIPO_DATO == 5)
                        {
                            //===========================================================================
                            // DROPDOWNLIST 
                            //===========================================================================
                            DropDownList dropDownList = (DropDownList)FindControlRecursive(PNL_DINAMICO, "DDLDNC_ADD_" + c.COLUMNA.ToString() + "_" + ID_OBJETO_PADRE);
                            String VALOR = ((Label)FindControlRecursive(PNL_DINAMICO, "LBLDNC_" + c.COLUMNA.ToString() + "_" + FILA.ToString())).Text;
                            if (VALOR != "" || VALOR != null) { 
                                     dropDownList.SelectedValue = VALOR;
                            }
                        }




                    }

                    LinkButton LNK_CREAR = (LinkButton)FindControlRecursive(GRD_PADRE, "AddNew");
                    LNK_CREAR.Visible = false;
                    LNK_CREAR.DataBind();

                }
                else
                {

                    //===========================================================
                    // CUANDO SE CANCELA LA GRILLA                                        
                    //===========================================================

                    GRD_PADRE.EditIndex  = -1;
                    e.Cancel             = true;
                    GRD_PADRE.DataBind();
                    return;
                }
              
            }catch(Exception EX) { }
        }


        /// <summary>
        /// ELIMINAR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GRDDinamica_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try{
                //=======================================================
                // OBTENGO LOS DATOS DE LA TABLA PADRE 
                //=======================================================
                int INDEX_SELECTED = e.RowIndex;
                GridView GRD_PADRE = (GridView)sender;

                var CAN_ROWS = GRD_PADRE.Rows;

                if (CAN_ROWS.Count > 1)
                {
                       SMetodos Servicio         = new SMetodos();

                       int ID_OBJETO_PADRE       = int.Parse(GRD_PADRE.ClientID.Replace("GRV_", ""));

                        //=======================================================
                        // OBTENGO LOS DATOS DE LA TABLA PADRE 
                        //=======================================================
                        CONTROL Objeto           = new CONTROL();
                        Objeto                   = V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO_PADRE).First();

                        OBJETO_GRILLA OBJ_GRILLA = new OBJETO_GRILLA();
                        OBJ_GRILLA               = (OBJETO_GRILLA)Objeto.OBJETO;

                        //=======================================================
                        // OBTENGO EL VALOR DEL CAMPO FILA DEL INDICE SELECCIONADO
                        //=======================================================
                        int FILA                 = int.Parse(OBJ_GRILLA.TABLA.Rows[INDEX_SELECTED]["FILA"].ToString());
                        DataTable TBL            = Servicio.SP_DELETE_ROW_GRILLA_DINAMICA(new iSP_DELETE_ROW_GRILLA_DINAMICA
                        {
                            ID_OBJETO            = ID_OBJETO_PADRE,
                            ROW                  = FILA
                        });

                        //=======================================================
                        // ACTUALIZO LA VARIABLE GLOBAL
                        //=======================================================
                        OBJ_GRILLA              = (OBJETO_GRILLA)Objeto.OBJETO;
                        OBJ_GRILLA.TABLA        = TBL;
                        V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO_PADRE).First().OBJETO = OBJ_GRILLA;

                        //=======================================================
                        // ACTUALIZO LA GRILLA
                        //=======================================================
                        GRD_PADRE.DataSource    = TBL;
                        GRD_PADRE.DataBind();

                }
            }
            catch(Exception EX) { }
        }

        protected void GRDDinamica_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            GridView GRD_PADRE  = (GridView)sender;
            GRD_PADRE.EditIndex = -1;
            GRD_PADRE.DataBind();
        }


        /// <summary>
        /// PRERENDER GRILLA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GRDDinamica_PreRender(object sender, EventArgs e)
        {
            try
            {
            
                GridView GRD_PADRE                  = (GridView)sender;

                GRD_PADRE.UseAccessibleHeader       = true;
                GRD_PADRE.HeaderRow.TableSection    = TableRowSection.TableHeader;
 

            }
            catch { }
        }


        /// <summary>
        /// CREACION Y CONTROL DE FILAS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GRDDinamica_RowDataBound(object sender, GridViewRowEventArgs e){
            try{

                if (e.Row.RowType == DataControlRowType.DataRow){

                    GridView Grilla          = (GridView)sender;
                    int ID_OBJETO            = int.Parse(Grilla.ClientID.Replace("GRV_", ""));
                    //=======================================================
                    // OBTENGO LOS DATOS DE LA TABLA 
                    //=======================================================
                    CONTROL Objeto           = new CONTROL();
                    Objeto                   = V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO).First();

                    OBJETO_GRILLA OBJ_GRILLA = new OBJETO_GRILLA();
                    OBJ_GRILLA               = (OBJETO_GRILLA)Objeto.OBJETO;
                    Objeto.OBJETO            = OBJ_GRILLA;
                    int TIPO_GRILLA          = OBJ_GRILLA.ID_TIPO_GRILLA;


                    if (TIPO_GRILLA == 3)
                    {
                        //=======================================================
                        // GRILLA MULTICHECKED
                        //=======================================================
                        Boolean CHECKED         = bool.Parse(DataBinder.Eval(e.Row.DataItem, "VALOR").ToString());
                        LinkButton CHK          = (LinkButton)e.Row.FindControl("CHK_BTN");

                        if (CHECKED)
                        {
                            CHK.CssClass        = "btn btn-primary";
                            CHK.Text            = "<i class='glyphicon glyphicon-check'></i>";
                            CHK.CommandArgument = DataBinder.Eval(e.Row.DataItem, "FILA").ToString();
                        }
                        else
                        {
                            CHK.CssClass        = "btn btn-default";
                            CHK.Text            = "<i class='glyphicon glyphicon-unchecked'></i>";
                            CHK.CommandArgument = DataBinder.Eval(e.Row.DataItem, "FILA").ToString();
                        }


                    }else if(TIPO_GRILLA == 4) {

                              
                        if (Grilla.EditIndex == e.Row.RowIndex)
                        {
                            e.Row.BackColor    = System.Drawing.Color.FromArgb(65, 105, 225);
                            e.Row.ForeColor    = System.Drawing.Color.Black;
                        }

                    }
                    else{

                        string CAMPO_FILTRAR;
                        string VALOR_FILTRAR;

                        //===========================================================================
                        // VALIDO SI EL OBJETO ES UN OBJETO PADRE PARA ASIGNARLE COLOR 
                        //===========================================================================
                        List<oSP_READ_FILTRO> LST_FILTRO = new List<oSP_READ_FILTRO>();
                        SMetodos Servicio                = new SMetodos();
                        LST_FILTRO                       = Servicio.SP_READ_FILTRO(new iSP_READ_FILTRO { ID_OBJETO = ID_OBJETO });

                        if (LST_FILTRO.Count() > 0)
                        {
                            //=======================================================
                            // SE BUSCA LOS FILTRO APLICADOS 
                            //=======================================================
                            CAMPO_FILTRAR                = LST_FILTRO.First().COLUMNA;
                            VALOR_FILTRAR                = V_Global().LST_CONTROLES.Where(p => p.ID_OBJETO == ID_OBJETO).First().VALOR;

                            //=======================================================
                            // PINTA LA GRILLA PADRE
                            //=======================================================
                            string Valor_Grilla          = (DataBinder.Eval(e.Row.DataItem, CAMPO_FILTRAR).ToString());

                            if (Valor_Grilla == VALOR_FILTRAR)
                            {
                                e.Row.BackColor          = System.Drawing.Color.FromArgb(65, 105, 225);
                                e.Row.ForeColor          = System.Drawing.Color.Black;
                            }

                        }
                        else
                        {
                            //=======================================================
                            // OBTENGO LOS DATOS DE LA TABLA 
                            //=======================================================
                            Objeto                      = V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO).First();
                            OBJ_GRILLA                  = (OBJETO_GRILLA)Objeto.OBJETO;
                            Objeto.OBJETO               = OBJ_GRILLA;
                            CAMPO_FILTRAR               = OBJ_GRILLA.OUTPUT.Where(d => d.ID_TIPO_HTML == 2).First().COLUMNA;
                            VALOR_FILTRAR               = V_Global().LST_CONTROLES.Where(p => p.ID_OBJETO == ID_OBJETO).First().VALOR;

                            //=======================================================
                            // PINTA LA GRILLA PADRE
                            //=======================================================
                            string Valor_Grilla = (DataBinder.Eval(e.Row.DataItem, CAMPO_FILTRAR).ToString());

                            if (Valor_Grilla == VALOR_FILTRAR)
                            {

                                e.Row.BackColor         = System.Drawing.Color.FromArgb(65, 105, 225);
                                e.Row.ForeColor         = System.Drawing.Color.Black;

                            }
                        }
                    }
                }
            }catch(Exception ex) { }
        }

           
            /// <summary>
            /// GRILLA 
            /// </summary>
            /// <param name = "sender" ></ param >
            /// < param name="e"></param>
        protected void GRDDinamica_RowCommand(object sender, GridViewCommandEventArgs e){
        try
        {
            //===========================================================
            // OBTENGO LA POSICION ENVIADA DE LA GRILLA                                          
            //===========================================================
            int ID = Convert.ToInt32(e.CommandArgument);

            //===========================================================
            // CREACION DE OBJETOS                                          
            //===========================================================
            SMetodos Servicio        = new SMetodos();
            GridView GRD_PADRE       = (GridView)sender;
            string CAMPO_FILTRAR     = "";
            string TIPO_FILTRO       = "";
            var VALOR_FILTRAR        = "";


            //===========================================================
            // OBTENGO EL ID DEL OBJETO PADRE
            //===========================================================
            int ID_OBJETO_PADRE      = int.Parse(GRD_PADRE.ClientID.Replace("GRV_", ""));

            //=======================================================
            // OBTENGO LOS DATOS DE LA TABLA PADRE 
            //=======================================================
            CONTROL Objeto           = new CONTROL();
            Objeto                   = V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO_PADRE).First();

            OBJETO_GRILLA OBJ_GRILLA = new OBJETO_GRILLA();
            OBJ_GRILLA               = (OBJETO_GRILLA)Objeto.OBJETO;

            //=======================================================
            // VALIDA SI ES GRILLA MULTISELECT
            //=======================================================
            if (OBJ_GRILLA.ID_TIPO_GRILLA == 3)
            {

                //=======================================================
                // EL VALOR SELECCIONADO EN EL DATATABLE
                //=======================================================
                int CONTADOR = 0;
                foreach (DataRow row in OBJ_GRILLA.TABLA.Rows)
                {

                    if (Convert.ToInt32(row["FILA"].ToString()) == ID)
                    {
                        Boolean VALOR_ACTUAL = Boolean.Parse(row["VALOR"].ToString());

                        if (VALOR_ACTUAL)
                        {
                            OBJ_GRILLA.TABLA.Rows[CONTADOR]["VALOR"] = "false";
                        }
                        else
                        {
                            OBJ_GRILLA.TABLA.Rows[CONTADOR]["VALOR"] = "true";
                        }
                    }
                    CONTADOR = CONTADOR + 1;
                }
                //=======================================================
                // ACTUALIZO EL OBJETO ESTATICO
                //=======================================================
                V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO_PADRE).First().OBJETO = OBJ_GRILLA;

                //=======================================================
                // RECARGO LA GRILLA
                //=======================================================               
                GRD_PADRE.DataBind();


            }
            else if (OBJ_GRILLA.ID_TIPO_GRILLA == 4)
            {

                string TIPO_BTN = e.CommandName.ToString();
                int ID_TIPO_DATO;
                //=======================================================
                // AGREGAR NUEVA FILA
                //=======================================================  
                if (TIPO_BTN == "AddNew")
                {
                    //===========================================================
                    // PARAMETROS DE ENTRADA 
                    //===========================================================
                    iSP_CREATE_ROW_GRILLA_DINAMICA ParametrosInput = new iSP_CREATE_ROW_GRILLA_DINAMICA();
                    ParametrosInput.ID_FORMULARIO = Objeto.ID_FORMULARIO;
                    ParametrosInput.ID_OBJETO = Objeto.ID_OBJETO;

                    oSP_RETURN_STATUS ROW_CREADO = new oSP_RETURN_STATUS();

                    //===========================================================
                    // SE CREA LOS CAMPOS EN LA TABLA ASIGNACION
                    //===========================================================
                    ROW_CREADO = Servicio.SP_CREATE_ROW_GRILLA_DINAMICA(ParametrosInput);


                    //===========================================================
                    //      RECORRO LOS CAMPOS DINAMICOS
                    //===========================================================

                    foreach (var c in OBJ_GRILLA.OUTPUT)
                    {
                        string VALOR_GUARDAR = "";
                        ID_TIPO_DATO         = c.ID_TIPO_DATO;


                        if (ID_TIPO_DATO < 2)
                        {
                            //===========================================================================
                            // TEXTBOX COMUN
                            //===========================================================================
                            TextBox textBox = (TextBox)FindControlRecursive(PNL_DINAMICO, "TXTDNC_ADD_" + c.COLUMNA.ToString() + "_" + Objeto.ID_OBJETO);
                            VALOR_GUARDAR = textBox.Text;
                        }
                        else if (ID_TIPO_DATO == 2)
                        {
                            //===========================================================================
                            // TEXTBOX NUMERO
                            //===========================================================================
                            TextBox textBox = (TextBox)FindControlRecursive(PNL_DINAMICO, "TXTDNC_ADD_" + c.COLUMNA.ToString() + "_" + Objeto.ID_OBJETO);
                            VALOR_GUARDAR = textBox.Text;
                        }
                        else if (ID_TIPO_DATO == 3)
                        {
                            //===========================================================================
                            // TEXTBOX FECHA
                            //===========================================================================
                            TextBox textBox = (TextBox)FindControlRecursive(PNL_DINAMICO, "TXTDNC_ADD_" + c.COLUMNA.ToString() + "_" + Objeto.ID_OBJETO);
                            VALOR_GUARDAR = DateTime.Parse(textBox.Text).ToShortDateString();
                        }
                        else if (ID_TIPO_DATO == 4)
                        {
                            //===========================================================================
                            // CHECKBOX 
                            //===========================================================================
                            CheckBox checkBox = (CheckBox)FindControlRecursive(PNL_DINAMICO, "CHKDNC_ADD_" + c.COLUMNA.ToString() + "_" + Objeto.ID_OBJETO);
                            VALOR_GUARDAR = checkBox.Checked.ToString();
                        }
                        else if (ID_TIPO_DATO == 5)
                        {
                            //===========================================================================
                            // DROPDOWNLIST 
                            //===========================================================================
                            DropDownList dropDownList = (DropDownList)FindControlRecursive(PNL_DINAMICO, "DDLDNC_ADD_" + c.COLUMNA.ToString() + "_" + ID_OBJETO_PADRE);
                            VALOR_GUARDAR = dropDownList.SelectedValue.ToString();
                        }

                        //===========================================================
                        //      SE ACTUALIZA EL VALOR EN LA BD
                        //===========================================================

                        iSP_UPDATE_ROW_GRILLA_DINAMICA parametro_actualizar = new iSP_UPDATE_ROW_GRILLA_DINAMICA();
                        parametro_actualizar.VALOR = VALOR_GUARDAR;
                        parametro_actualizar.VALOR_REFERENCIA = c.COLUMNA.ToString();
                        parametro_actualizar.PARENT_ID_CHK = (int)Objeto.ID_OBJETO;
                        parametro_actualizar.ROW = (int)ROW_CREADO.RETURN_VALUE;

                        Servicio.SP_UPDATE_ROW_GRILLA_DINAMICA(parametro_actualizar);

                    }

                    DataTable TBL = Servicio.SP_READ_TABLA_PIVOT_WEB_GRV_DINAMIC(new iSP_READ_TABLA_PIVOT_WEB_GRV_DINAMIC
                    {
                        ID_TABLA = OBJ_GRILLA.ID_TABLA,
                        ID_OBJETO = Objeto.ID_OBJETO
                    });

                    //=======================================================
                    // ACTUALIZO LA VARIABLE GLOBAL
                    //=======================================================
                    OBJ_GRILLA.TABLA = TBL;
                    V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO_PADRE).First().OBJETO = OBJ_GRILLA;

                    //=======================================================
                    // ACTUALIZO LA GRILLA
                    //=======================================================
                    GRD_PADRE.DataSource = TBL;
                    GRD_PADRE.DataBind();
                }            



            } else{

                //=============================================================================================
                // SI NO ENCUENTRA FILTROS ES PORQUE ES UN OBJETO HIJO POR LO QUE HAY QUE BUSCAR FILTROS PADRE
                //=============================================================================================

                if (OBJ_GRILLA.FILTRO.Count() < 1)
                {
                    CAMPO_FILTRAR = OBJ_GRILLA.OUTPUT.Where(d => d.ID_TIPO_HTML == 2).First().COLUMNA;

                    //=============================================================================================
                    // VALIDO SI TIENE ALGUN OBJETO PADRE QUE DEBA FILTRAR SU TABLA
                    //=============================================================================================
                    int PARENT_ID = V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO_PADRE).First().PARENT_ID;

                    if (PARENT_ID > 0)
                    {
                        //=======================================================
                        // OBTENGO LOS DATOS DEL PARENT_ID
                        //=======================================================   
                        OBJETO_GRILLA OBJ_GRILLA_HIJO = new OBJETO_GRILLA();
                        Objeto                        = V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == PARENT_ID).First();
                        OBJ_GRILLA_HIJO               = (OBJETO_GRILLA)Objeto.OBJETO;


                        string CAMPO_FILTRAR_PARENT = OBJ_GRILLA_HIJO.FILTRO.First().COLUMNA;
                        string TIPO_FILTRO_PARENT   = OBJ_GRILLA_HIJO.FILTRO.First().FILTRO;
                        string VALOR_PARENT         = V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == PARENT_ID).First().VALOR;

                        //=======================================================
                        // SE GENERA FILTRO PARA CREAR DATOS DEL HIJO FILTRADO 
                        //=======================================================
                        string QUERY_FILTRO          = CAMPO_FILTRAR_PARENT + " " + TIPO_FILTRO_PARENT + " " + VALOR_PARENT;

                        //=======================================================
                        // FILTRO TABLA E INSERTO DATOS AL OBJETO HIJO 
                        //=======================================================
                        DataTable TABLA_FILTRADA     = OBJ_GRILLA.TABLA.Select(QUERY_FILTRO).CopyToDataTable();

                        VALOR_FILTRAR                = TABLA_FILTRADA.Rows[ID][CAMPO_FILTRAR].ToString();

                    }else{

                        VALOR_FILTRAR                = OBJ_GRILLA.TABLA.Rows[ID][CAMPO_FILTRAR].ToString();

                    }

                }else{
                    //=======================================================
                    // VALIDO SI TIENE OBJETO HIJO 
                    //=======================================================
                    CAMPO_FILTRAR   = OBJ_GRILLA.FILTRO.First().COLUMNA;
                    TIPO_FILTRO     = OBJ_GRILLA.FILTRO.First().FILTRO;
                    VALOR_FILTRAR   = OBJ_GRILLA.TABLA.Rows[ID][CAMPO_FILTRAR].ToString();

                    //=======================================================
                    // BUSCO EL ID DEL OBJETO HIJO
                    //=======================================================
                    int ID_OBJETO_HIJO = V_Global().LST_CONTROLES.Where(P => P.PARENT_ID == ID_OBJETO_PADRE).First().ID_OBJETO;

                    //=======================================================
                    // SE GENERA FILTRO PARA CREAR DATOS DEL HIJO FILTRADO 
                    //=======================================================
                    string QUERY_FILTRO = CAMPO_FILTRAR + " " + TIPO_FILTRO + " " + VALOR_FILTRAR;


                    //=======================================================
                    // OBTENEGO LOS DATOS DE LA TABLA HIJO
                    //=======================================================
                    Objeto       = V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO_HIJO).First();
                    OBJ_GRILLA   = (OBJETO_GRILLA)Objeto.OBJETO;


                    //=======================================================
                    // SE BUSCA EL OBJETO GRIDVIEW HIJO
                    //=======================================================
                    GridView gridViewhijo = (GridView)FindControlRecursive(PNL_DINAMICO, "GRV_" + ID_OBJETO_HIJO);


                    //=======================================================
                    // FILTRO TABLA E INSERTO DATOS AL OBJETO HIJO 
                    //=======================================================
                    DataTable TABLA_FILTRADA = OBJ_GRILLA.TABLA.Select(QUERY_FILTRO).CopyToDataTable();

                    gridViewhijo.DataSource = TABLA_FILTRADA;
                    gridViewhijo.DataBind();

                }
                //=======================================================
                // GUARDAR EL VALOR FILTRADO 
                //=======================================================
                V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO_PADRE).First().VALOR = VALOR_FILTRAR;

                //=======================================================
                // RECARGO LA GRILLA
                //=======================================================  
                GRD_PADRE.DataBind();
            }
        }
        catch (System.Exception ex)
        {
            MensajeLOG(ALO.WebSite.Error.ThrowError.MensajeThrow(ex), "ERRORES DE APLICACIÓN");
        }
    }



        protected void DDLSelectedIndexChanged(object sender, EventArgs e){
            //=======================================================
            // DECLARACION DE OBJETOS
            //=======================================================
            SMetodos Servicio                  = new SMetodos();
            CONTROL Objeto                     = new CONTROL();
            OBJETO_DROPDOWLIST OBJ_COMBO_PADRE = new OBJETO_DROPDOWLIST();
            OBJETO_DROPDOWLIST OBJ_COMBO_HIJO  = new OBJETO_DROPDOWLIST();
            DropDownList DDL_PADRE             = (DropDownList)sender;

            //=======================================================
            // BUSCAR VALOR SELECCIONADO DEL PADRE
            //=======================================================
            string VALOR_FILTRAR    = DDL_PADRE.SelectedValue.ToString();
            int    ID_OBJETO_PADRE  = int.Parse(DDL_PADRE.ClientID.Replace("DDL_", ""));

            //=======================================================
            // OBTENGO DATOS DEL OBJETO PADRE
            //=======================================================
            Objeto          = V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO_PADRE).First();
            OBJ_COMBO_PADRE = (OBJETO_DROPDOWLIST)Objeto.OBJETO;

            //=======================================================
            // BUSCOS LOS CAMPOS A APLICAR AL OBJETO HIJO
            //=======================================================
            string CAMPO_FILTRAR = OBJ_COMBO_PADRE.FILTRO.First().COLUMNA;
            string TIPO_FILTRO   = OBJ_COMBO_PADRE.FILTRO.First().FILTRO;       

            //=======================================================
            // BUSCO EL ID DEL OBJETO HIJO
            //=======================================================
            int ID_OBJETO_HIJO = V_Global().LST_CONTROLES.Where(P => P.PARENT_ID == ID_OBJETO_PADRE).First().ID_OBJETO;

            //=======================================================
            // SE BUSCA EL OBJETO DROPDONWLIST HIJO
            //=======================================================
            DropDownList  DDL_HIJO = (DropDownList)FindControlRecursive(PNL_DINAMICO, "DDL_" + ID_OBJETO_HIJO);
   
            //=======================================================
            // SE GENERA FILTRO PARA CREAR DATOS DEL HIJO FILTRADO 
            //=======================================================
            string QUERY_FILTRO = CAMPO_FILTRAR + " " + TIPO_FILTRO + " " + VALOR_FILTRAR;

            //=======================================================
            // OBTENEGO LOS DATOS DE LA TABLA 
            //=======================================================
            Objeto         = V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO_HIJO).First();
            OBJ_COMBO_HIJO = (OBJETO_DROPDOWLIST)Objeto.OBJETO;

            //=======================================================
            // FILTRO TABLA PARA EL DDL HIJO
            //=======================================================
            DataTable TABLA_FILTRADA_HIJO = OBJ_COMBO_HIJO.TABLA.Select(QUERY_FILTRO).CopyToDataTable();


            //===========================================================================
            // CAMBIO EL SELECTED VALUE A LA PRIMERA OPCION DISPONIBLE EN EL NUEVO FILTRO
            //===========================================================================
            string CAMPO_SELECT_HIJO = OBJ_COMBO_HIJO.OUTPUT.Where(P => P.ID_TIPO_HTML == 3).First().COLUMNA;
            string VALOR_SELECT_HIJO = OBJ_COMBO_HIJO.OUTPUT.Where(P => P.ID_TIPO_HTML == 2).First().COLUMNA;


            DDL_HIJO.SelectedItem.Value  =  TABLA_FILTRADA_HIJO.Rows[0][VALOR_SELECT_HIJO].ToString();
            DDL_HIJO.SelectedItem.Text   =  TABLA_FILTRADA_HIJO.Rows[0][CAMPO_SELECT_HIJO].ToString();
            DDL_HIJO.SelectedValue       =  TABLA_FILTRADA_HIJO.Rows[0][VALOR_SELECT_HIJO].ToString();

            //=======================================================
            // ACTUALIZO LOS DATOS DEL DDL HIJO 
            //=======================================================
            DDL_HIJO.DataSource = TABLA_FILTRADA_HIJO;
            DDL_HIJO.DataBind();


            //=======================================================
            // CONSULTO SI EL OBJETO HIJO TIENE UN HIJO 
            //=======================================================

            if (V_Global().LST_CONTROLES.Where(P => P.PARENT_ID == ID_OBJETO_HIJO).Count() > 0) {

                //=======================================================
                // BUSCAR VALOR SELECCIONADO DEL HIJO(PADRE)
                //=======================================================
                VALOR_FILTRAR = TABLA_FILTRADA_HIJO.Rows[0][VALOR_SELECT_HIJO].ToString();

                OBJETO_DROPDOWLIST OBJ_COMBO_HIJO_HIJO = new OBJETO_DROPDOWLIST();
                //=======================================================
                // SE BUSCA LOS FILTRO APLICADOS AL OBJETO HIJO 
                //=======================================================
                Objeto              = V_Global().LST_CONTROLES.Where(P => P.PARENT_ID == ID_OBJETO_HIJO).First();
                OBJ_COMBO_HIJO_HIJO = (OBJETO_DROPDOWLIST)Objeto.OBJETO;
                CAMPO_FILTRAR       = OBJ_COMBO_HIJO.FILTRO.First().COLUMNA;
                TIPO_FILTRO         = OBJ_COMBO_HIJO.FILTRO.First().FILTRO;

                //=======================================================
                // BUSCO EL ID DEL OBJETO HIJO(HIJO)
                //=======================================================
                int ID_OBJETO_HIJO_HIJO = V_Global().LST_CONTROLES.Where(P => P.PARENT_ID == ID_OBJETO_HIJO).First().ID_OBJETO;

                //=======================================================
                // SE BUSCA EL OBJETO DROPDONWLIST HIJO(HIJO)
                //=======================================================
                DropDownList DDL_HIJO_HIJO = (DropDownList)FindControlRecursive(PNL_DINAMICO, "DDL_" + ID_OBJETO_HIJO_HIJO);

                //=======================================================
                // SE GENERA FILTRO PARA CREAR DATOS DEL HIJO FILTRADO 
                //=======================================================
                QUERY_FILTRO = CAMPO_FILTRAR + " " + TIPO_FILTRO + " " + VALOR_FILTRAR;

                //=======================================================
                // FILTRO TABLA PARA EL DDL HIJO
                //=======================================================
                DataTable TABLA_FILTRADA_HIJO_HIJO = OBJ_COMBO_HIJO_HIJO.TABLA.Select(QUERY_FILTRO).CopyToDataTable();

                //===========================================================================
                // CAMBIO EL SELECTED VALUE A LA PRIMERA OPCION DISPONIBLE EN EL NUEVO FILTRO
                //===========================================================================
                string CAMPO_SELECT_HIJO_HIJO       = OBJ_COMBO_HIJO_HIJO.OUTPUT.Where(P => P.ID_TIPO_HTML == 3).First().COLUMNA;
                string VALOR_SELECT_HIJO_HIJO       = OBJ_COMBO_HIJO_HIJO.OUTPUT.Where(P => P.ID_TIPO_HTML == 2).First().COLUMNA;
                DDL_HIJO_HIJO.SelectedItem.Value    = TABLA_FILTRADA_HIJO_HIJO.Rows[0][VALOR_SELECT_HIJO_HIJO].ToString();
                DDL_HIJO_HIJO.SelectedItem.Text     = TABLA_FILTRADA_HIJO_HIJO.Rows[0][CAMPO_SELECT_HIJO_HIJO].ToString();
                DDL_HIJO_HIJO.SelectedValue         = TABLA_FILTRADA_HIJO_HIJO.Rows[0][VALOR_SELECT_HIJO_HIJO].ToString();

                //=======================================================
                // ACTUALIZO LOS DATOS DEL DDL HIJO 
                //=======================================================
                DDL_HIJO_HIJO.DataSource = TABLA_FILTRADA_HIJO_HIJO;
                DDL_HIJO_HIJO.DataBind();
            }
        }


        /// <summary>
        /// GRABAR VALOR OBJETO EN LA BD
        /// </summary>
        private void GUARDAR_VALOR_OBJETO_V_GLOBAL(){
            //===========================================================
            // DECLARACION DE VARIABLES
            //===========================================================
            SMetodos Servicio = new SMetodos();

            //===========================================================
            // ACTUALIZA VALOR Y VALOR REFERENCIA EN VARIABLE GLOBAL
            //===========================================================
            foreach (var d in V_Global().LST_CONTROLES)
            {
                //===========================================================
                // OBJETOS TIPO DROPDOWNLIST
                //===========================================================
                if (d.ID_TIPO_OBJETO == 1){
                    DropDownList dropDown = (DropDownList)FindControlRecursive(PNL_DINAMICO, "DDL_" + d.ID_OBJETO);
                    V_Global().LST_CONTROLES.Where(p => p.ID_OBJETO == d.ID_OBJETO).First().VALOR = dropDown.SelectedValue.ToString();
                    V_Global().LST_CONTROLES.Where(p => p.ID_OBJETO == d.ID_OBJETO).First().VALOR_REFERENCIA = "";

                //===========================================================
                // ACTUALIZA EL VALOR EN LA BD
                //===========================================================
                    GUARDAR_OBJETO_BD(d.ID_OBJETO, dropDown.SelectedValue.ToString(), "");
                }

                //===========================================================
                // OBJETOS TIPO GRILLA
                //===========================================================
                if (d.ID_TIPO_OBJETO == 2){

                    OBJETO_GRILLA OBJ_GRILLA    = new OBJETO_GRILLA();
                    OBJ_GRILLA                  = (OBJETO_GRILLA)d.OBJETO;
                    int Tipo_Grilla             = OBJ_GRILLA.ID_TIPO_GRILLA;
    
                    //====================================================================
                    // OBJETOS TIPO GRILLA 1:INFORMATIVA 2:SELECCION 3:SELECCION MULTIPLE 
                    //====================================================================
                        if(Tipo_Grilla == 1){
                            //===========================================================
                            // ACTUALIZA EL VALOR EN LA BD
                            //===========================================================
                            GUARDAR_OBJETO_BD(d.ID_OBJETO,"","");

                        }else if (Tipo_Grilla == 2){
                            //===========================================================
                            // OBTENGO EL VALOR SELECCIONADO
                            //===========================================================
                            string Valor_Seleccionado = V_Global().LST_CONTROLES.Where(p => p.ID_OBJETO == d.ID_OBJETO).First().VALOR;

                            //===========================================================
                            // ACTUALIZA EL VALOR EN LA BD
                            //===========================================================
                            GUARDAR_OBJETO_BD(d.ID_OBJETO,Valor_Seleccionado, "");

                        }else if (Tipo_Grilla == 3){

                            //===========================================================
                            // ACTUALIZA EL VALOR EN LA BD PARA GRILLA MULTICHECKED
                            //===========================================================
                            GUARDAR_OBJETO_BD_MULTICHECKED(d.ID_OBJETO);

                        }
                } 

                //===========================================================
                // OBJETOS TIPO TEXTBOX
                //===========================================================
                if (d.ID_TIPO_OBJETO == 3){
                    TextBox Caja = (TextBox)FindControlRecursive(PNL_DINAMICO, "TXT_" + d.ID_OBJETO);
                    V_Global().LST_CONTROLES.Where(p => p.ID_OBJETO == d.ID_OBJETO).First().VALOR = Caja.Text;
                    V_Global().LST_CONTROLES.Where(p => p.ID_OBJETO == d.ID_OBJETO).First().VALOR_REFERENCIA = "";

                //===========================================================
                // ACTUALIZA EL VALOR EN LA BD
                //===========================================================
                    GUARDAR_OBJETO_BD(d.ID_OBJETO, Caja.Text,"");
                }

                //===========================================================
                // OBJETOS TIPO CHECKBOX
                //===========================================================
                if (d.ID_TIPO_OBJETO == 4)
                {
                 CheckBox Check = (CheckBox)FindControlRecursive(PNL_DINAMICO, "CHK_" + d.ID_OBJETO);
                 V_Global().LST_CONTROLES.Where(p => p.ID_OBJETO == d.ID_OBJETO).First().VALOR = Check.Checked.ToString();
                 V_Global().LST_CONTROLES.Where(p => p.ID_OBJETO == d.ID_OBJETO).First().VALOR_REFERENCIA = "";

                //===========================================================
                // ACTUALIZA EL VALOR EN LA BD
                //===========================================================
                GUARDAR_OBJETO_BD(d.ID_OBJETO,Check.Checked.ToString(),"");
                }
            }
        }


        /// <summary>
        /// GRABAR VALOR OBJETO EN LA BD PARA GRILLAS MULTISELECCION
        /// </summary>
        /// <param name="ID_OBJETO"></param>
        /// <param name="VALOR"></param>
        /// <param name="VALOR_REFERENCIA"></param>
        protected void GUARDAR_OBJETO_BD_MULTICHECKED(int ID_OBJETO){
            //===========================================================
            // DECLARACION DE VARIABLES
            //===========================================================
            SMetodos Servicio = new SMetodos();

            //===========================================================
            // OBTENER ID ASIGNACION ASOCIADO AL OBJETO
            //===========================================================
            int ID_ASIGNACION = V_Global().LST_PANTALLA.First().ID_ASIGNACION;

            //=======================================================
            // OBTENGO LOS DATOS DE LA TABLA PADRE 
            //=======================================================
            CONTROL Objeto           = new CONTROL();
            Objeto                   = V_Global().LST_CONTROLES.Where(P => P.ID_OBJETO == ID_OBJETO).First();
            OBJETO_GRILLA OBJ_GRILLA = new OBJETO_GRILLA();
            OBJ_GRILLA               = (OBJETO_GRILLA)Objeto.OBJETO;
            Objeto.OBJETO            = OBJ_GRILLA;

            //===========================================================
            // ACTUALIZO LOS CAMPOS DE LA GRILLA MULTISELECCION
            //===========================================================
            foreach (DataRow row in OBJ_GRILLA.TABLA.Rows){

                //===========================================================
                // PARAMETROS DE ENTRADA 
                //===========================================================
                iSP_UPDATE_FORMULARIO_X_OBJETO_MULTICHECKED ParametrosInput = new iSP_UPDATE_FORMULARIO_X_OBJETO_MULTICHECKED();

                ParametrosInput.ID_OBJETO            = ID_OBJETO;
                ParametrosInput.VALOR                = Boolean.Parse(row["VALOR"].ToString());
                ParametrosInput.FILA                 = row["FILA"].ToString();  //VALOR_REFERENCIA;
                ParametrosInput.ID_ASIGNACION        = ID_ASIGNACION;

                //===========================================================
                // ENVIAR OBJETO A NEGOCIO 
                //===========================================================
                Servicio.SP_UPDATE_FORMULARIO_X_OBJETO_MULTICHECKED(ParametrosInput);

            }
        }


        /// <summary>
        /// GRABAR VALOR OBJETO EN LA BD
        /// </summary>
        /// <param name="ID_OBJETO"></param>
        /// <param name="VALOR"></param>
        /// <param name="VALOR_REFERENCIA"></param>
        protected void GUARDAR_OBJETO_BD(int ID_OBJETO , string VALOR , string VALOR_REFERENCIA){
        //===========================================================
        // DECLARACION DE VARIABLES
        //===========================================================
        SMetodos Servicio            = new SMetodos();
        List<PANTALLA> LST_PANTALLAS = new List<PANTALLA>();

        //===========================================================
        // OBTENER VIEWSTATE
        //===========================================================
        LST_PANTALLAS                = V_Global().LST_PANTALLA;

        int ID_ASIGNACION            = LST_PANTALLAS.First().ID_ASIGNACION;

        //===========================================================
        // PARAMETROS DE ENTRADA 
        //===========================================================
        iSP_UPDATE_FORMULARIO_X_OBJETO ParametrosInput = new iSP_UPDATE_FORMULARIO_X_OBJETO();
        ParametrosInput.ID_OBJETO           = ID_OBJETO;
        ParametrosInput.VALOR               = VALOR;
        ParametrosInput.VALOR_REFERENCIA    = "";  
        ParametrosInput.ID_ASIGNACION       = ID_ASIGNACION;

        //===========================================================
        // ENVIAR OBJETO A NEGOCIO 
        //===========================================================
            Servicio.SP_UPDATE_FORMULARIO_X_OBJETO(ParametrosInput);
        }




        /// <summary>
        /// CLICK PANELES
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CLICK_PANELES(object sender, EventArgs e) {

           OnPreRender(e);
            {
                LinkButton OBJETO    = (LinkButton)sender;
                string NOMBRE        = OBJETO.ID;
                string REF           = NOMBRE.Replace("LNK_", "");

                if (REF == "1000") {

                    //=========================================================== 
                    // LLAMAR ASHX POST                                        
                    //===========================================================
                    string URL = new UConexionesADO().URLPost() + "?MODO=0&LOGIN_ID=" + V_Global().LST_PANTALLA.First().LOGIN + "&CAMPANA=&IDENTIFICADOR=&FONO=&IDLLAMADA=" + V_Global().LST_PANTALLA.First().INTERACCION + "&TIPO=";
                    
                    using (WebClient wc = new WebClient())
                    {
                        string Response = wc.DownloadString(URL);

                    }


                    //===========================================================
                    // CIERRA LA PESTAÑA DEL FORMULARIO
                    //===========================================================
                    ScriptManager.RegisterStartupScript(this.Page, typeof(string), "f04", "close_window()", true);

                }
                else
                {
                    //===========================================================
                    // BOTON SIGUIENTE
                    //===========================================================
                        String[] SPLIT       = REF.Split('_');
                        string DV_ORI        = "DV_" + SPLIT[0];
                        string DV_DES        = "DV_" + SPLIT[1];

                        if (SPLIT[0] != "0")
                        {
                            if (!VALIDAR_OBJETO(SPLIT[0]))
                            {
                                DV_DES = "DV_" + SPLIT[0];
                                DV_ORI = "DV_" + (int.Parse(SPLIT[0]) - 1).ToString();
                             }
                        }

                        HD_TAB_ORI.Value = DV_ORI;
                        HD_TAB_DES.Value = DV_DES;

                }


            }
        }

        /// <summary>
        /// CREACION DE CONTROL
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Padre"></param>
        protected void CrearPantallas(HtmlGenericControl PADRE){
            try{
                int PANTALLAS = 0;
                string TITULO = "";
                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio            = new SMetodos();
                List<PANTALLA> LST_PANTALLAS = new List<PANTALLA>();

                //===========================================================
                // OBTENER VIEWSTATE
                //===========================================================
                LST_PANTALLAS = V_Global().LST_PANTALLA;

                //===========================================================
                // CUENTA LA CANTIDAD DE PANTALLAS
                //===========================================================
                PANTALLAS = LST_PANTALLAS.Count();

                //===========================================================
                // SETEO DE PANTALLAS                       
                //===========================================================
                int FIN = PANTALLAS + 1;

                //===========================================================
                // TAG PADRE                        
                //===========================================================
                HtmlGenericControl TAG_PADRE  = new HtmlGenericControl("div");
                TAG_PADRE.Attributes["class"] = "tab-content";
                TAG_PADRE.Attributes["id"]    = "TAB_INFO";

                //===========================================================
                // TAG INICIAL                        
                //===========================================================
                HtmlGenericControl TAG_DIV_INI  = new HtmlGenericControl("div");
                TAG_DIV_INI.Attributes["id"]    = "DV_1000";
                TAG_DIV_INI.Attributes["class"] = "tab-pane fade in active show";

                HtmlGenericControl TAG_A_INI        = new HtmlGenericControl("a");
                TAG_A_INI.Attributes["class"]       = "nav-link";
                TAG_A_INI.Attributes["data-toggle"] = "tab";
                TAG_A_INI.Attributes["href"]        = "#DV_0";
                TAG_A_INI.Attributes["style"]       = "display: none;";


                TAG_DIV_INI.Controls.Add(TAG_A_INI);
                TAG_PADRE.Controls.Add(TAG_DIV_INI);

                //===========================================================
                // ITERACION                        
                //===========================================================
                for (int i = 0; i <= FIN; i++){
                    if (i == 0){
            
                        //===================================================
                        // CONTROL PADRE
                        //===================================================
                        HtmlGenericControl TAG_DIV      = new HtmlGenericControl("div");
                        TAG_DIV.Attributes["id"]        = "DV_" + i;
                        TAG_DIV.Attributes["class"]     = "tab-pane fade";

                        //===================================================
                        // PANEL
                        //===================================================
                        HtmlGenericControl TAG_PANEL  = new HtmlGenericControl("div");
                        TAG_PANEL.Attributes["class"] = "panel panel-primary";

                        HtmlGenericControl TAG_HEADER  = new HtmlGenericControl("div");
                        TAG_HEADER.Attributes["class"] = "panel-heading";

                        HtmlGenericControl TAG_BODY  = new HtmlGenericControl("div");
                        TAG_BODY.Attributes["class"] = "panel-body";

                        HtmlGenericControl TAG_PIE  = new HtmlGenericControl("div");
                        TAG_PIE.Attributes["class"] = "panel-footer";

                        //===================================================
                        // FIN PANEL
                        //===================================================

                        //===================================================
                        // CONTENIDO
                        //===================================================
                        HtmlGenericControl TAG_CONTENIDO = new HtmlGenericControl("h4");
                        TAG_CONTENIDO.InnerText          = "INICIO DE CONTENIDO";
                        TAG_BODY.Controls.Add(TAG_CONTENIDO);
                        //===================================================
                        // FIN CONTENIDO
                        //===================================================


                        //===================================================
                        // TITULO
                        //===================================================
                        HtmlGenericControl TAG_TITULO       = new HtmlGenericControl("h3");
                        TAG_TITULO.Attributes["class"]      = "panel-title";
                        TAG_TITULO.InnerText                = "DATOS DE ASIGNACIÓN";
                        TAG_HEADER.Controls.Add(TAG_TITULO);

                        //===================================================
                        // FOOTER
                        //===================================================
                        HtmlGenericControl TAG_PIE_CONTAINER  = new HtmlGenericControl("div");
                        TAG_PIE_CONTAINER.Attributes["class"] = "container-fluid";

                        HtmlGenericControl TAG_ROW_P        = new HtmlGenericControl("div");
                        TAG_ROW_P.Attributes["class"]       = "row";

                        HtmlGenericControl TAG_COL_P1       = new HtmlGenericControl("div");
                        TAG_COL_P1.Attributes["class"]      = "col-sm-4";
                        TAG_COL_P1.Attributes["style"]      = "text-align:left;";

                        HtmlGenericControl TAG_COL_P2 = new HtmlGenericControl("div");
                        TAG_COL_P2.Attributes["class"] = "col-sm-4";

                        HtmlGenericControl TAG_COL_P3 = new HtmlGenericControl("div");
                        TAG_COL_P3.Attributes["class"] = "col-sm-4";
                        TAG_COL_P3.Attributes["style"] = "text-align:right;";

                        //===============================================
                        // CONTROLES
                        //===============================================
                        HtmlGenericControl TAG_A        = new HtmlGenericControl("a");
                        TAG_A.Attributes["class"]       = "nav-link";
                        TAG_A.Attributes["data-toggle"] = "tab";
                        TAG_A.Attributes["href"]        = "#DV_" + (i + 1);
                        TAG_A.Attributes["style"]       = "display: none;";

                        LinkButton Botton_s = new LinkButton();
                        Botton_s.ID         = "LNK_0" + "_" + (i + 1);
                        Botton_s.Text       = "SIGUIENTE";
                        Botton_s.CssClass   = "btn btn-success";
                        Botton_s.Click     += new System.EventHandler(CLICK_PANELES);

                        TAG_COL_P3.Controls.Add(TAG_A);
                        TAG_COL_P3.Controls.Add(Botton_s);
                        //===============================================
                        // FIN CONTROLES
                        //===============================================

                        TAG_ROW_P.Controls.Add(TAG_COL_P1);
                        TAG_ROW_P.Controls.Add(TAG_COL_P2);
                        TAG_ROW_P.Controls.Add(TAG_COL_P3);
                        TAG_PIE_CONTAINER.Controls.Add(TAG_ROW_P);

                        TAG_PIE.Controls.Add(TAG_PIE_CONTAINER);

                        TAG_PANEL.Controls.Add(TAG_HEADER);
                        TAG_PANEL.Controls.Add(TAG_BODY);
                        TAG_PANEL.Controls.Add(TAG_PIE);
                        TAG_DIV.Controls.Add(TAG_PANEL);

                        TAG_PADRE.Controls.Add(TAG_DIV);

                    }else{

                        //===============================================
                        // CONTROL PADRE
                        //===============================================
                        HtmlGenericControl TAG_DIV = new HtmlGenericControl("div");
                        TAG_DIV.Attributes["id"] = "DV_" + i;
                        TAG_DIV.Attributes["class"] = "tab-pane fade";

                        //===============================================
                        // PANEL
                        //===============================================
                        HtmlGenericControl TAG_PANEL = new HtmlGenericControl("div");
                        TAG_PANEL.Attributes["class"] = "panel panel-primary";

                        HtmlGenericControl TAG_HEADER = new HtmlGenericControl("div");
                        TAG_HEADER.Attributes["class"] = "panel-heading";

                        HtmlGenericControl TAG_BODY = new HtmlGenericControl("div");
                        TAG_BODY.Attributes["class"] = "panel-body";

                        HtmlGenericControl TAG_PIE = new HtmlGenericControl("div");
                        TAG_PIE.Attributes["class"] = "panel-footer";
                        

                        //===============================================
                        // FIN PANEL
                        //===============================================

                        if (i == FIN){
                            HtmlGenericControl TAG_CONTENIDO = new HtmlGenericControl("h4");
                            TAG_CONTENIDO.InnerText = "FIN DE CONTENIDO";
                            TAG_BODY.Controls.Add(TAG_CONTENIDO);

                        }else {

                            //====================================================
                            // OBTENGO EL NOMBRE DEL TITULO DE LA VARIABLE GLOBAL
                            //==================================================

                            TITULO = LST_PANTALLAS.Where(d => d.ORDEN == i).First().DESCRIPCION;

                            //===========================================
                            // CONTENIDO
                            //===========================================
                            HtmlGenericControl TAG_BODY_CONTENIDO = new HtmlGenericControl("div");
                            TAG_BODY_CONTENIDO.Attributes["class"] = "container-fluid";

                            Panel PNL_CONTENIDO = new Panel();
                            PNL_CONTENIDO.ID = "PNL_" + i;
                            PNL_CONTENIDO.Attributes["runat"] = "server";
                            PNL_CONTENIDO.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                            TAG_BODY_CONTENIDO.Controls.Add(PNL_CONTENIDO);
                            TAG_BODY.Controls.Add(TAG_BODY_CONTENIDO);

                            //===========================================
                            // FIN CONTENIDO
                            //===========================================
                        }

                        //===============================================
                        // TITULO
                        //===============================================
                        HtmlGenericControl TAG_TITULO = new HtmlGenericControl("h3");
                        TAG_TITULO.Attributes["class"] = "panel-title";

                        if (i == FIN){
                            TAG_TITULO.InnerText = "FIN";
                        }else{
                            TAG_TITULO.InnerText = "PANEL " + TITULO ;
                        }
                        
            
                        TAG_HEADER.Controls.Add(TAG_TITULO);

                        //===============================================
                        // FOOTER
                        //===============================================
                        HtmlGenericControl TAG_PIE_CONTAINER = new HtmlGenericControl("div");
                        TAG_PIE_CONTAINER.Attributes["class"] = "container-fluid";

                        HtmlGenericControl TAG_ROW_P = new HtmlGenericControl("div");
                        TAG_ROW_P.Attributes["class"] = "row";

                        HtmlGenericControl TAG_COL_P1 = new HtmlGenericControl("div");
                        TAG_COL_P1.Attributes["class"] = "col-sm-4";
                        TAG_COL_P1.Attributes["style"] = "text-align:left;";

                        HtmlGenericControl TAG_COL_P2 = new HtmlGenericControl("div");
                        TAG_COL_P2.Attributes["class"] = "col-sm-4";

                        HtmlGenericControl TAG_COL_P3 = new HtmlGenericControl("div");
                        TAG_COL_P3.Attributes["class"] = "col-sm-4";
                        TAG_COL_P3.Attributes["style"] = "text-align:right;";

                        //===============================================
                        // CONTROLES
                        //===============================================
                        HtmlGenericControl TAG_A        = new HtmlGenericControl("a");
                        TAG_A.Attributes["class"]       = "nav-link";
                        TAG_A.Attributes["data-toggle"] = "tab";
                        TAG_A.Attributes["href"]        = "#DV_" + (i + 1);
                        TAG_A.Attributes["style"]       = "display: none;";

                        LinkButton Botton_s             = new LinkButton();
                        Botton_s.ID                     = "LNK_" + i + "_" + (i + 1);
                        Botton_s.Text                   = "SIGUIENTE";
                        Botton_s.CssClass               = "btn btn-success";
                        Botton_s.Click                 += new System.EventHandler(CLICK_PANELES);


                        HtmlGenericControl TAG_B        = new HtmlGenericControl("a");
                        TAG_B.Attributes["class"]       = "nav-link btn btn-primary";
                        TAG_B.Attributes["data-toggle"] = "tab";
                        TAG_B.Attributes["role"]        = "button";
                        TAG_B.Attributes["href"]        = "#DV_" + (i - 1);
                        TAG_B.InnerText                 = "ANTERIOR";

                        TAG_COL_P1.Controls.Add(TAG_B);
                        TAG_COL_P3.Controls.Add(TAG_A);

                        if (i != FIN)
                        {
                            TAG_COL_P3.Controls.Add(Botton_s);
                        }
                        else
                        {
                            //===============================================
                            //  BOTON FINALIZAR GESTION
                            //===============================================

                            LinkButton Botton_f = new LinkButton();
                            Botton_f.ID = "LNK_1000";
                            Botton_f.Text = " <i class='glyphicon glyphicon-check'></i>  FINALIZAR GESTION ";
                            Botton_f.CssClass = "btn btn-danger";
                            Botton_f.Click += new System.EventHandler(CLICK_PANELES);

                            TAG_COL_P3.Controls.Add(Botton_f);
                        }
                        //===============================================
                        // FIN CONTROLES
                        //===============================================


                        TAG_ROW_P.Controls.Add(TAG_COL_P1);
                        TAG_ROW_P.Controls.Add(TAG_COL_P2);
                        TAG_ROW_P.Controls.Add(TAG_COL_P3);
                        TAG_PIE_CONTAINER.Controls.Add(TAG_ROW_P);
                        TAG_PIE.Controls.Add(TAG_PIE_CONTAINER);
                        TAG_PANEL.Controls.Add(TAG_HEADER);
                        TAG_PANEL.Controls.Add(TAG_BODY);
                        TAG_PANEL.Controls.Add(TAG_PIE);
                        TAG_DIV.Controls.Add(TAG_PANEL);
                        TAG_PADRE.Controls.Add(TAG_DIV);
                    }
                }

                //===========================================================
                // SE INGRESA UN FINAL                   
                //===========================================================
                HtmlGenericControl TAG_FIN       = new HtmlGenericControl("div");
                TAG_FIN.Attributes["id"]         = "DIV_" + PANTALLAS;
                TAG_FIN.Attributes["class"]      = "tab-pane fade";

                HtmlGenericControl TAG_A_FIN        = new HtmlGenericControl("a");
                TAG_A_FIN.Attributes["class"]       = "nav-link";
                TAG_A_FIN.Attributes["data-toggle"] = "tab";
                TAG_A_FIN.Attributes["href"]        = "#DV_" + (PANTALLAS + 1);
                TAG_A_FIN.Attributes["style"]       = "display: none;";

                TAG_FIN.Controls.Add(TAG_A_FIN);
                TAG_PADRE.Controls.Add(TAG_FIN);

                //===========================================================
                // SE INGRESA TAG                      
                //===========================================================
                PADRE.Controls.Add(TAG_PADRE);
            }catch { }
        }

        /// <summary>
        /// CREACION DE CONTROL
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Padre"></param>
        protected void CrearControl(Control Control, Label Label, Panel Padre,int TIPO_BJETO){
            try{
                //===========================================================
                // DIV ROW ESPACIO                   
                //===========================================================
                HtmlGenericControl DivRowSpace = new HtmlGenericControl("div");
                DivRowSpace.Attributes["class"] = "row";
                DivRowSpace.Attributes["style"] = "margin-top: 5px;";

                //===========================================================
                // DIV ROW                     
                //===========================================================
                HtmlGenericControl DivRow = new HtmlGenericControl("div");
                DivRow.Attributes["class"] = "row";

                HtmlGenericControl DivCol_1 = new HtmlGenericControl("div");
                DivCol_1.Attributes["class"] = "col-sm-1";

                HtmlGenericControl DivCol_2 = new HtmlGenericControl("div");
                DivCol_2.Attributes["class"] = "col-sm-4";

                HtmlGenericControl DivCol_3 = new HtmlGenericControl("div");
                DivCol_3.Attributes["class"] = "col-sm-5";

                HtmlGenericControl DivCol_4 = new HtmlGenericControl("div");
                DivCol_4.Attributes["class"] = "col-sm-1";

                HtmlGenericControl DivCol_12 = new HtmlGenericControl("div");
                DivCol_12.Attributes["class"] = "col-sm-12 table-responsive";



                HtmlGenericControl  Hr = new HtmlGenericControl("hr");

                //===========================================================
                // SE CREA CONTROL EN PADRE
                //===========================================================
                Padre.Controls.Add(DivRowSpace);

                //===========================================================
                // SE AGREGA CONTROL                       
                //===========================================================
                DivRow.Controls.Add(DivCol_1);
                DivRow.Controls.Add(Hr);

                if (TIPO_BJETO == 2) {


                    DivCol_2.Controls.Add(Label);
                    DivRow.Controls.Add(DivCol_2);
                    DivRow.Controls.Add(DivCol_3);
                    DivRow.Controls.Add(DivCol_4);
                    Padre.Controls.Add(DivRow);

                    DivCol_12.Controls.Add(Control);
                    DivRow.Controls.Add(DivCol_12);


                }
                else {

                    DivCol_2.Controls.Add(Label);
                    DivRow.Controls.Add(DivCol_2);
                    DivCol_3.Controls.Add(Control);
                    DivRow.Controls.Add(DivCol_3);
                    DivRow.Controls.Add(DivCol_4);
                    Padre.Controls.Add(DivRow);
                }


           
            }catch{ }
        }

        /// <summary>
        /// Recursive FindControl method, to search a control and all child
        /// controls for a control with the specified ID.
        /// </summary>
        /// <returns>Control if found or null</returns>
        public static Control FindControlRecursive(Control root, string id){
            if (id == string.Empty)
                return null;

            if (root.ID == id)
                return root;

            foreach (Control c in root.Controls){
                Control t = FindControlRecursive(c, id);
                if (t != null){
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// CAJA DE TEXTO
        /// </summary>
        /// <param name="Caja"></param>
        private void ObjetoTexto(TextBox Caja){
            try{
                Caja.Attributes.Add("placeholder", "INGRESE VALOR");
                Caja.Attributes.Add("type", "text");
                Caja.Visible      = true;
                Caja.CssClass     = "form-control";
                Caja.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            }catch{
                throw;
            }
        }

        /// <summary>
        /// CAJA DE NUMERO
        /// </summary>
        /// <param name="Caja"></param>
        private void ObjetoNumero(TextBox Caja){
            try{
                Caja.Attributes.Add("placeholder", "INGRESE VALOR");
                Caja.Attributes.Add("type", "number");
                Caja.Attributes.Add("onkeypress", "return numbersonly(event)");
                Caja.Visible      = true;
                Caja.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                Caja.CssClass     = "form-control";
                Caja.Attributes.Add("required", "required");
            }catch{
                throw;
            }
        }

        /// <summary>
        /// CAJA DE FECHA
        /// </summary>
        /// <param name="Caja"></param>
        private void ObjetoFecha(TextBox Caja){
            try{
                Caja.Attributes.Add("placeholder", "INGRESE VALOR");
                Caja.Attributes.Add("type" , "date");
                Caja.Attributes.Add("onkeypress", "return numbersonly(event)");
                Caja.Visible      = true;
                Caja.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                Caja.CssClass     = "form-control";
            }catch{
                throw;
            }
        }


        /// <summary>
        /// checkbox
        /// </summary>
        /// <param name="checked"></param>
        private void ObjetoBool(CheckBox Check, bool estado){
            try{
                Check.Visible       = true;
                Check.Checked       = estado;
                Check.ClientIDMode  = System.Web.UI.ClientIDMode.Static;
                Check.CssClass      = "form-check";
            }catch{
                throw;
            }
        }


        /// <summary>
        /// GRIDVIEW
        /// </summary>
        /// <param name="checked"></param>
        private void ObjetoGridview(GridView grid){
            try{
                grid.Visible        = true;
                grid.ClientIDMode   = System.Web.UI.ClientIDMode.Static;
                grid.CssClass       = "table table-condensed";
                grid.DataBind();
            }catch{
                throw;
            }
        }


        /// <summary>
        /// RESOLVER URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ResolveURL(string url){
            var resolvedURL = this.Page.ResolveClientUrl(url);
            return resolvedURL;
        }

        /// <summary>
        ///  RESOLVER URL
        /// </summary>
        /// <param name="jsURL"></param>
        /// <returns></returns>
        public string UrlWeb(string jsURL){
            return ResolveURL(jsURL);
        }

    }

    [Serializable]
    public class GlobalesFormulario{
        public List<CONTROL>    LST_CONTROLES { get; set; }
        public List<PANTALLA>   LST_PANTALLA { get; set; }
    }

}