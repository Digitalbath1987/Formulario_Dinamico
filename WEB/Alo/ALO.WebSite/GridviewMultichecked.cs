using ALO.Entidades;
using ALO.WebSite.Formularios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ALO.Entidades
{
    public class GridViewTemplate : ITemplate
    {
        private DataControlRowType templateType;
        private string columnName;

        public GridViewTemplate(DataControlRowType type, string colname)
        {
            templateType = type;
            columnName = colname;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {

            switch (templateType)
            {
                case DataControlRowType.DataRow:

                    LinkButton CHK_BTN = new LinkButton();
                    CHK_BTN.ID = "CHK_BTN";
                    CHK_BTN.CommandName = "CHECKED";
                    container.Controls.Add(CHK_BTN);
                    break;
            }
        }
    }
}
