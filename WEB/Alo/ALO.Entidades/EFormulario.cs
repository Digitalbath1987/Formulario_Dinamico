using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALO.Entidades.Form
{


    /// <summary>
    /// CABEZERA DE OBJETO
    /// </summary>
    [Serializable]
    public class CONTROL
    {
        public Int32 ID_OBJETO { get; set; }
        public Int32 PARENT_ID { get; set; }
        public string CODIGO { get; set; }
        public string DESCRIPCION { get; set; }
        public Int32 ID_TIPO_OBJETO { get; set; }
        public Int32 PANTALLA { get; set; }
        public Int32 ID_FORMULARIO { get; set; }
        public String VALOR { get; set; }
        public String VALOR_REFERENCIA { get; set; }
        public bool OBLIGATORIO { get; set; }
        public object OBJETO { get; set; }


    }

    [Serializable]
    public class PANTALLA
    {
        public Int32  Q_PANTALLA { get; set; }
        public string INTERACCION { get; set; }
        public string LOGIN { get; set; }
        public Int32  ID_ASIGNACION { get; set; }
        public Int32  ID_SISTEMA { get; set; }

        public int ORDEN { get; set; }
        public string DESCRIPCION { get; set; }

    }




    /// <summary>
    /// TEXTBOX
    /// </summary>
    [Serializable]
    public class OBJETO_TEXTBOX{
        public string LABEL { get; set; }
        public Int32 ID_TIPO_DATO { get; set; }
        public String VALOR { get; set; }

        public Int32 MIN_CARACTERES { get; set; }
        public Int32 MAX_CARACTERES { get; set; }


    }

    /// <summary>
    /// CHECKBOX
    /// </summary>
    [Serializable]
    public class OBJETO_CHECKBOX
    {
        public string LABEL { get; set; }
        //ESTE LO AGREGUE
        public Int32 ID_TIPO_DATO { get; set; }
        public String VALOR { get; set; }
    }

    /// <summary>
    /// COMBO
    /// </summary>
    [Serializable]
    public class OBJETO_DROPDOWLIST
    {
        public string LABEL { get; set; }
        public Int32 ID_TABLA { get; set; }
        public DataTable TABLA { get; set; }
        public List<FILTROS> FILTRO { get; set; }
        public List<PARAMETROS_OUTPUT> OUTPUT { get; set; }
        public String VALOR { get; set; }

    }


    /// <summary>
    /// GRILLA
    /// </summary>
    [Serializable]
    public class OBJETO_GRILLA
    {

        public string LABEL { get; set; }
        public Int32 ID_TABLA { get; set; }
        public Int32 ID_TIPO_GRILLA { get; set; }
        public DataTable TABLA { get; set; }
        public List<FILTROS> FILTRO { get; set; }
        public List<PARAMETROS_OUTPUT> OUTPUT { get; set; }
        public List<SELECCION> SELECCIONADO { get; set; }

    }


    /// <summary>
    /// FILTRO
    /// </summary>
    [Serializable] 
    public class FILTROS
    {
        public string COLUMNA { get; set; }
        public string FILTRO { get; set; }
        public string PARAMETRO { get; set; }

    }

    /// <summary>
    /// PARAMETROS
    /// </summary>
    [Serializable]
    public class PARAMETROS_OUTPUT
    {
        public string COLUMNA { get; set; }
        public bool VISIBLE { get; set; }
        public int ID_TIPO_HTML { get; set; }
        public int ID_TIPO_DATO { get; set; }
        public int ID_TABLA_GRV_DINAMICA { get; set; }
        public object OBJETO_DDL_GRV_DINAMICA { get; set; }
    }

    [Serializable]
    public class OBJETO_GRV_DINAMICA_DDL
    {

        public DataTable TABLA { get; set; }
        public List<PARAMETROS_OUTPUT_GRV_DINAMICA> OUTPUT { get; set; }

    }

    /// <summary>
    /// PARAMETROS
    /// </summary>
    [Serializable]
    public class PARAMETROS_OUTPUT_GRV_DINAMICA
    {
        public string COLUMNA { get; set; }
        public bool VISIBLE { get; set; }
        public int ID_TIPO_HTML { get; set; }
        public int ID_TIPO_DATO { get; set; }
        public int ID_TABLA_GRV_DINAMICA { get; set; }
    }





    /// <summary>
    /// FILTRO
    /// </summary>
    [Serializable]
    public class SELECCION
    {
        public string ID { get; set; }

    }

}
