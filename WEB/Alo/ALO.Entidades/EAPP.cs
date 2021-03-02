using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALO.Entidades
{
    public class CHECHBOX_DINAMICO
    {
        public string Valor { get; set; }
        public string Item { get; set; }
    }


    public class DDL_DINAMICO
    {
        public int Valor { get; set; }
        public string Item { get; set; }
    }




    public class Item_Seleccion
    {
        public int Id { get; set; }
        public string IdStr { get; set; }
        public string Nombre { get; set; }
        public string Orden { get; set; }

    }

       
    public class iSP_UPDATE_FORMULARIO_X_OBJETO
    {
        public Int32 ID_OBJETO { get; set; }
        public String VALOR { get; set; }
        public String VALOR_REFERENCIA { get; set; }
        public Int32 ID_ASIGNACION { get; set; }
    }


    public class iSP_UPDATE_FORMULARIO_X_OBJETO_MULTICHECKED
    {
        public Int32 ID_OBJETO { get; set; }
        public Boolean VALOR { get; set; }
        public String FILA { get; set; }
        public Int32 ID_ASIGNACION { get; set; }
    }





    public class oSP_RETURN_STATUS
    {
        public Decimal RETURN_VALUE { get; set; }
    }




    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_PANTALLA_X_ASIGNACION
    {
        public Int32 ID_ASIGNACION { get; set; }
        public Int32 ID_SISTEMA { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_PANTALLA_X_ASIGNACION
    {
        public Int32 ID_PANTALLA { get; set; }
        public Int32 Q_PANTALLA { get; set; }
        public Int32 ORDEN { get; set; }
        public string DESCRIPCION { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_TABLA_PIVOT_WEB_GRV_DINAMICA_DDL
    {
        public Int32 ID_TABLA_GRV_DINAMICA { get; set; }
    }


    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL
    {
        public Int32 ID_COLUMNA { get; set; }
    }
    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_PARAMETROS_OUTPUT_GRV_DINAMICA_DLL
    {
        public Int32 ID_PARAMETROS_OUTPUT { get; set; }
        public Int32 ID_COLUMNA { get; set; }
        public String COLUMNA { get; set; }
        public Boolean VISIBLE { get; set; }
        public Int32 ID_TIPO_HTML { get; set; }
        public String TIPO_HTML { get; set; }
        public Int32 ID_OBJETO { get; set; }
        public Int32 ID_TIPO_DATO { get; set; }
    }


    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_OBJETO_X_OPCION
    {
        public Int32 ID_ASIGNACION { get; set; }
        public Int32 ID_SISTEMA { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_OBJETO_X_OPCION
    {
        public Int32 ID_OBJETO { get; set; }
        public Int32 PARENT_ID { get; set; }
        public String CODIGO { get; set; }
        public String DESCRIPCION { get; set; }
        public Int32 ID_TIPO_OBJETO { get; set; }
        public bool OBLIGATORIO { get; set; }
      
      
        public Int32 PANTALLA { get; set; }
        public Int32 ID_FORMULARIO { get; set; }
        public String VALOR { get; set; }
       
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_OBJETO_X_COMBO
    {
        public Int32 ID_OBJETO { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_OBJETO_X_COMBO
    {
        public String LABEL { get; set; }
        public Int32 ID_TABLA { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_OBJETO_X_GRILLA
    {
        public Int32 ID_OBJETO { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_OBJETO_X_GRILLA
    {
        public String LABEL { get; set; }
        public Int32 ID_TABLA { get; set; }
        public Int32 ID_TIPO_GRILLA { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_OBJETO_X_TEXTBOX
    {
        public Int32 ID_OBJETO { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_OBJETO_X_TEXTBOX
    {
        public String LABEL { get; set; }
        public Int32 ID_TIPO_DATO { get; set; }
        public Int32 MIN_CARACTERES { get; set; }
        public Int32 MAX_CARACTERES { get; set; }

    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_OBJETO_X_CHECKBOX
    {
        public Int32 ID_OBJETO { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_OBJETO_X_CHECKBOX
    {
        public String LABEL { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_TABLA_PIVOT_WEB{
        public Int32 ID_TABLA { get; set; }
        public Int32 ID_OBJETO { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_CREATE_ROW_GRILLA_DINAMICA
    {
        public Int32 ID_FORMULARIO { get; set; }
        public Int32 ID_OBJETO { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_DELETE_ROW_GRILLA_DINAMICA
    {
        public Int32 ID_OBJETO { get; set; }
        public Int32 ROW { get; set; }
    }


    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_TABLA_PIVOT_WEB_GRV_MULTICHECKED{
        public Int32 ID_TABLA { get; set; }
        public Int32 ID_OBJETO { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_TABLA_PIVOT_WEB_GRV_DINAMIC
    {
        public Int32 ID_TABLA { get; set; }
        public Int32 ID_OBJETO { get; set; }
    }

    

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_FILTRO
    {
        public Int32 ID_OBJETO { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_FILTRO
    {
        public Int32 ID_FILTRO { get; set; }
        public Int32 ID_COLUMNA { get; set; }
        public String COLUMNA { get; set; }
        public Int32 ID_TIPO_FILTRO { get; set; }
        public String TIPO_FILTRO { get; set; }
        public String PARAMETRO { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_PARAMETROS_OUTPUT
    {
        public Int32 ID_OBJETO { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_PARAMETROS_OUTPUT
    {
        public Int32 ID_PARAMETROS_OUTPUT { get; set; }
        public Int32 ID_COLUMNA { get; set; }
        public String COLUMNA { get; set; }
        public Boolean VISIBLE { get; set; }
        public Int32 ID_TIPO_HTML { get; set; }
        public String TIPO_HTML { get; set; }
        public Int32 ID_TIPO_DATO { get; set; }
        public Int32 ID_TABLA_GRV_DINAMICA { get; set; }
    }


    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_UPDATE_ROW_GRILLA_DINAMICA
    {
        public Int32 PARENT_ID_CHK { get; set; }
        public Int32 ROW { get; set; }
        public String VALOR_REFERENCIA { get; set; }
        public String VALOR { get; set; }
    }


}
