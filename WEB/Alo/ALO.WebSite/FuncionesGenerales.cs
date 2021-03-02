using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ALO.Entidades;
namespace ALO.WebSite
{
    public static class FuncionesGenerales
    {



        /// <summary>
        /// CARGAR COMBOS
        /// </summary>
        /// <param name="result"></param>
        /// <param name="Combo"></param>
        /// <returns></returns>
        public static bool CDDLCombos(List<Item_Seleccion> result, DropDownList Combo)
        {
            try
            {
                if (!result.Any()) { throw new Exception("0 valores en campos."); }


                Combo.DataValueField = "Id";
                Combo.DataTextField = "Nombre";
                Combo.DataSource = result;
                Combo.Enabled = true;

                return true;
            }
            catch
            {
                Combo.Items.Clear();
                Combo.DataSource = null;
                Combo.Enabled = false;

                return false;
            }
            finally
            {
                Combo.DataBind();
            }
        }


        /// <summary>
        /// CARGAR COMBOS
        /// </summary>
        /// <param name="result"></param>
        /// <param name="Lista"></param>
        /// <returns></returns>
        public static bool CDDLListbox(List<Item_Seleccion> result, ListBox Lista)
        {
            try
            {
                if (!result.Any()) { throw new Exception("0 valores en campos."); }


                Lista.DataValueField = "Id";
                Lista.DataTextField = "Nombre";
                Lista.DataSource = result;
                Lista.Enabled = true;

                return true;
            }
            catch
            {
                Lista.Items.Clear();
                Lista.DataSource = null;
                Lista.Enabled = false;

                return false;
            }
            finally
            {
                Lista.DataBind();
            }
        }


        /// <summary>
        /// CARGAR GRILLA
        /// </summary>
        /// <param name="l_item"></param>
        /// <param name="Grilla"></param>
        /// <returns></returns>
        public static bool Cargar_Grilla(object l_item, GridView Grilla)
        {
            try
            {



                Grilla.DataSource = l_item;
                Grilla.Enabled = true;

                return true;
            }
            catch
            {
                Grilla.DataSource = null;
                Grilla.Enabled = false;

                return false;
            }
            finally
            {
                Grilla.DataBind();
            }
        }


        /// <summary>
        /// COLOCAR COMBO EN POSICIONES SEGUN ID ENVIADO 
        /// </summary>
        /// <param name="Combo"></param>
        /// <param name="ID"></param>
        public static void BuscarIdCombo(DropDownList Combo, int ID)
        {


            for (int i = 0; i <= Combo.Items.Count - 1; i++)
            {
                Combo.SelectedIndex = i;
                int Valor = Convert.ToInt32(Combo.SelectedValue);

                if (Valor == ID)
                {
                    break;
                }

            }


        }
  
    }
}