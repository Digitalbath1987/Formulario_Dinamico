using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ALO.WebSite
{
    public partial class Default1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MaintainScrollPositionOnPostBack = true;
            try
            {


                if (!this.IsPostBack)
                {

                    BuscarElementosPOSTEnviados();

                }

            }
            catch (System.Exception ex)
            {
                MensajeLOG(ALO.WebSite.Error.ThrowError.MensajeThrow(ex), "ERRORES DE APLICACIÓN");
            }

        }

        /// <summary>
        /// MENSAJE LOG      
        /// </summary>
        /// <param name="Mensaje"></param>
        /// <param name="Titulo"></param>
        private void MensajeLOG(string Mensaje, string Titulo)
        {

            try
            {


                //===========================================================
                // CONTENIDO DIV CON RUNAT SERVER                       
                //===========================================================
                var DIV = (HtmlGenericControl)Page.Master.FindControl("LOG_MENSAJE_SERVER");
                DIV.InnerHtml = WebUtility.HtmlDecode(Mensaje);


                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("function FLOG(){");
                sb.Append("MensajeLOG('" + Titulo + "');");
                sb.Append("Sys.Application.remove_load(FLOG);}");
                sb.Append("Sys.Application.add_load(FLOG);");
                sb.Append("</script>");


                ScriptManager.RegisterStartupScript(this, typeof(Page), "PopupJS", sb.ToString(), false);



            }
            catch { }

        }
        /// <summary>
        /// BUSCAR ELEMENTOS POST DESDE OTRAS PAGINAS
        /// </summary>
        /// <returns></returns>
        private bool BuscarElementosPOSTEnviados()
        {

            try
            {
                //===========================================================
                // ELEMENTOS POST                         
                //===========================================================
                DatosRequest DatosPOST = new DatosRequest();
                DatosPOST.QueryString = Request.QueryString;
                DatosPOST.Form = Request.Form;


                //===========================================================
                // MENSAJES                         
                //===========================================================
                string MENSAJE = DatosPOST["MENSAJE"];


                if (string.IsNullOrEmpty(MENSAJE))
                {
                    PNL_Mensaje.Visible = false;
                    LBL_Mensaje.Text = ""; ;
                }
                else
                {

                    PNL_Mensaje.Visible = true;
                    LBL_Mensaje.Text = MENSAJE;
                }


                return true;
            }
            catch
            {
                return false;
            }




        }
    }
}