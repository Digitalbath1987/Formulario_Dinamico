using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace ALO.WebSite
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

            //==============================================
            // HOME
            //==============================================
            RouteTable.Routes.MapPageRoute("Home", "Home", "~/Default.aspx");


            //==============================================
            // TAREAS
            //==============================================
            RouteTable.Routes.MapPageRoute("VTA_FORMULARIO", "VTA/FORMULARIO", "~/Formularios/Formularios.aspx");


            

        


        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}