using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ALO.Entidades.Form;


namespace ALO.WebSite
{
    public class AddTemplateToGridView : ITemplate
    {
        ListItemType POSICION_GRV;
        string NOMBRE_COLUMNA;
        int TIPO_DATO;
        int ID_OBJETO;

        public PARAMETROS_OUTPUT PARAMETROS { get;  set; }

        public AddTemplateToGridView(ListItemType Posicion_Grv, string Nombre_Columna, int Tipo_Dato, int Id_Objeto,object Parametros = null)
        {
            POSICION_GRV = Posicion_Grv;
            NOMBRE_COLUMNA = Nombre_Columna;
            TIPO_DATO = Tipo_Dato;
            ID_OBJETO = Id_Objeto;
            PARAMETROS = (PARAMETROS_OUTPUT)Parametros;

        }

        void ITemplate.InstantiateIn(System.Web.UI.Control container)
        {


            switch (POSICION_GRV)
            {

                case ListItemType.Header:
                    Label LABEL_HEADER = new Label();
                    LABEL_HEADER.Text = NOMBRE_COLUMNA;
                    container.Controls.Add(LABEL_HEADER);
                    break;
  
                case ListItemType.Item:

                    if (NOMBRE_COLUMNA == "OPCIONES") {


                        //===========================================================================
                        // BOTON EDITAR
                        //===========================================================================
                        LinkButton LNK_EDITAR = new LinkButton();

                        LNK_EDITAR.CommandName = "Edit";
                        LNK_EDITAR.ToolTip = "Editar";
                        LNK_EDITAR.Attributes.Add("title", "Edit");
                        LNK_EDITAR.Text = "<span class='glyphicon glyphicon-pencil btn btn-default' aria-hidden='true'></span>";
                        LNK_EDITAR.DataBinding += new EventHandler(LinkDinamico_DataBinding);
                        container.Controls.Add(LNK_EDITAR);

                        //===========================================================================
                        // BOTON ELIMINAR
                        //===========================================================================
                        LinkButton LNK_ELIMINAR = new LinkButton();

                        LNK_ELIMINAR.CommandName = "Delete";
                        LNK_ELIMINAR.ToolTip = "Eliminar";
                        LNK_ELIMINAR.Attributes.Add("title", "Delete");
                        LNK_ELIMINAR.Text = "<span class='glyphicon glyphicon-trash btn btn-default' aria-hidden='true'></span>";
                        LNK_ELIMINAR.DataBinding += new EventHandler(LinkDinamico_DataBinding);
                        container.Controls.Add(LNK_ELIMINAR);

                    }
                    else
                    {
                        Label LABEL_ITEM = new Label();
                        LABEL_ITEM.DataBinding += new EventHandler(LabelDinamico_DataBinding);
                        container.Controls.Add(LABEL_ITEM);
                    }
                 
                    break;

                case ListItemType.EditItem:

                    if (NOMBRE_COLUMNA == "OPCIONES")
                    {


                        //===========================================================================
                        // BOTON CANCELAR
                        //===========================================================================
                        LinkButton LNK_CANCELAR = new LinkButton();

                        LNK_CANCELAR.CommandName = "Cancel";
                        LNK_CANCELAR.ToolTip = "Cancel";
                        LNK_CANCELAR.Attributes.Add("title", "Cancel");
                        LNK_CANCELAR.Text = "<span class='glyphicon glyphicon-floppy-remove btn btn-default' aria-hidden='true'></span>";
                        LNK_CANCELAR.DataBinding += new EventHandler(Link_Cancel_Dinamico_DataBinding);
                        container.Controls.Add(LNK_CANCELAR);

                        //===========================================================================
                        // BOTON ACTUALIZAR
                        //===========================================================================

                        LinkButton LNK_ACTUALIZAR = new LinkButton();

                        LNK_ACTUALIZAR.CommandName = "Update";
                        LNK_ACTUALIZAR.ToolTip = "Update";
                        LNK_ACTUALIZAR.Attributes.Add("title", "Update");
                        LNK_ACTUALIZAR.Text = "<span class='glyphicon glyphicon-floppy-saved btn btn-default' aria-hidden='true'></span>";
                        LNK_ACTUALIZAR.DataBinding += new EventHandler(Link_Update_Dinamico_DataBinding);
                        container.Controls.Add(LNK_ACTUALIZAR);


                    }
                    else
                    {

                        //===========================================================================
                        // LABEL BOUND EDIT
                        //===========================================================================
                        Label LABEL_ITEM = new Label();
                        LABEL_ITEM.DataBinding += new EventHandler(LabelDinamico_DataBinding);
                        container.Controls.Add(LABEL_ITEM);
  
                    }


                    break;

                //agregar CAMPO FOOTER
                case ListItemType.Footer:
                    if (NOMBRE_COLUMNA == "OPCIONES")
                    {
                        //===========================================================================
                        // BOTON CREAR
                        //===========================================================================
                        LinkButton LNK_CREAR = new LinkButton();

                        LNK_CREAR.CommandName = "AddNew";
                        LNK_CREAR.CommandArgument = "-1";
                        LNK_CREAR.ID = "AddNew";
                        LNK_CREAR.ToolTip = "Crear";
                        LNK_CREAR.Text = "<span class='glyphicon glyphicon-plus btn btn-default' aria-hidden='true'></span>";
                        container.Controls.Add(LNK_CREAR);

      


                    }
                    else
                    {

                        if (TIPO_DATO == 1 || TIPO_DATO == 0)
                        {
                            //===========================================================================
                            // TEXTBOX COMUN
                            //===========================================================================
                            TextBox TXT_EDITAR = new TextBox();
                            TXT_EDITAR.ID = "TXTDNC_ADD_" + NOMBRE_COLUMNA + "_" + ID_OBJETO;
                            TXT_EDITAR.Attributes.Add("Class", "form-control");
                            TXT_EDITAR.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                            container.Controls.Add(TXT_EDITAR);


                        }
                        else if (TIPO_DATO == 2)
                        {
                            //===========================================================================
                            // TEXTBOX NUMERO
                            //===========================================================================
                            TextBox TXT_EDITAR = new TextBox();
                            TXT_EDITAR.ID = "TXTDNC_ADD_" + NOMBRE_COLUMNA + "_" + ID_OBJETO;
                            TXT_EDITAR.Attributes.Add("Class", "form-control");
                            TXT_EDITAR.Attributes.Add("type", "number");
                            TXT_EDITAR.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                            container.Controls.Add(TXT_EDITAR);
                        }
                        else if (TIPO_DATO == 3)
                        {
                            //===========================================================================
                            // TEXTBOX FECHA
                            //===========================================================================
                            TextBox TXT_EDITAR = new TextBox();
                            TXT_EDITAR.ID = "TXTDNC_ADD_" + NOMBRE_COLUMNA + "_" + ID_OBJETO;
                            TXT_EDITAR.Attributes.Add("Class", "form-control");
                            TXT_EDITAR.Attributes.Add("type", "date");
                            TXT_EDITAR.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                            container.Controls.Add(TXT_EDITAR);

                        }
                        else if (TIPO_DATO == 4)
                        {
                            //===========================================================================
                            // CHECKBOX 
                            //===========================================================================
                            CheckBox CHK_EDITAR = new CheckBox();
                            CHK_EDITAR.ID = "CHKDNC_ADD_" + NOMBRE_COLUMNA + "_" + ID_OBJETO;
                            CHK_EDITAR.Attributes.Add("Class", "form-control");
                            CHK_EDITAR.Attributes.Add("type", "checkbox");
                            CHK_EDITAR.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                            container.Controls.Add(CHK_EDITAR);
                        }
                        else if (TIPO_DATO == 5)
                        {

                            //===========================================================================
                            // CHECKBOX 
                            //===========================================================================

                            OBJETO_GRV_DINAMICA_DDL OBJ_GRV_DIN_DDL = new OBJETO_GRV_DINAMICA_DDL();
                            OBJ_GRV_DIN_DDL = (OBJETO_GRV_DINAMICA_DDL)PARAMETROS.OBJETO_DDL_GRV_DINAMICA;

                            DropDownList DDL_EDITAR = new DropDownList();
                            DDL_EDITAR.ID = "DDLDNC_ADD_" + NOMBRE_COLUMNA + "_" + ID_OBJETO;
                            DDL_EDITAR.Attributes.Add("Class", "form-control");
                            DDL_EDITAR.DataValueField = OBJ_GRV_DIN_DDL.OUTPUT.Where(T => T.ID_TIPO_HTML == 2).First().COLUMNA;
                            DDL_EDITAR.DataTextField = OBJ_GRV_DIN_DDL.OUTPUT.Where(T => T.ID_TIPO_HTML == 3).First().COLUMNA;
                            DDL_EDITAR.DataSource = OBJ_GRV_DIN_DDL.TABLA;
                            DDL_EDITAR.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                            container.Controls.Add(DDL_EDITAR);


                        }

                    }

                    break;
            }

        }





        void LabelDinamico_DataBinding(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            GridViewRow container = (GridViewRow)label.NamingContainer;
            object ID_FILA = DataBinder.Eval(container.DataItem, "FILA");
      

            object bindValue = DataBinder.Eval(container.DataItem, NOMBRE_COLUMNA);
            if (bindValue != DBNull.Value)
            {


                label.Text = bindValue.ToString();
                label.ID = "LBLDNC_" + NOMBRE_COLUMNA + "_" + ID_FILA.ToString();
   

            }

        }

        void TexboxDinamico_DataBinding(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            GridViewRow container = (GridViewRow)textBox.NamingContainer;
            object VALOR = DataBinder.Eval(container.DataItem, NOMBRE_COLUMNA);
            object ID_FILA = DataBinder.Eval(container.DataItem, "FILA");
            textBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (VALOR != DBNull.Value)
            {

                textBox.Text = VALOR.ToString();
                textBox.ID = "TXTDNC_" + NOMBRE_COLUMNA + "_" + ID_FILA.ToString();

            }
        }

        void Link_Update_Dinamico_DataBinding(object sender, EventArgs e)
        {
            LinkButton linkButton = (LinkButton)sender;
            GridViewRow container = (GridViewRow)linkButton.NamingContainer;

            object ID_FILA = DataBinder.Eval(container.DataItem, "FILA");
            linkButton.CommandArgument = ID_FILA.ToString();
            linkButton.CommandName = "update";

        }


        void Link_Cancel_Dinamico_DataBinding(object sender, EventArgs e)
        {
            LinkButton linkButton = (LinkButton)sender;
            GridViewRow container = (GridViewRow)linkButton.NamingContainer;

            object ID_FILA = DataBinder.Eval(container.DataItem, "FILA");
            linkButton.CommandArgument = ID_FILA.ToString();
            linkButton.CommandName = "Cancel";

        }


        void LinkDinamico_DataBinding(object sender, EventArgs e)
        {
            LinkButton linkButton = (LinkButton)sender;
            GridViewRow container = (GridViewRow)linkButton.NamingContainer;
            object ID_FILA = DataBinder.Eval(container.DataItem, "FILA");
            linkButton.CommandArgument = ID_FILA.ToString();

        }

        void CheckDinamico_DataBinding(object sender, EventArgs e)
        {
            CheckBox CheckBox = (CheckBox)sender;
            GridViewRow container = (GridViewRow)CheckBox.NamingContainer;
            object VALOR = DataBinder.Eval(container.DataItem, NOMBRE_COLUMNA);
            object ID_FILA = DataBinder.Eval(container.DataItem, "FILA");

            if (VALOR == DBNull.Value || VALOR.ToString() == "")
            {
                CheckBox.Checked = false;
                CheckBox.ID = "CHKDNC_" + NOMBRE_COLUMNA + "_" + ID_FILA.ToString();

            }
            else
            {
                CheckBox.Checked = Boolean.Parse(VALOR.ToString());
                CheckBox.ID = "CHKDNC_" + NOMBRE_COLUMNA + "_" + ID_FILA.ToString();


            }
        }

        


    }

}