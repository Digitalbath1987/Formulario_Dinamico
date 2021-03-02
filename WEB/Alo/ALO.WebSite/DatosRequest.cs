using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace ALO.WebSite
{
    public class DatosRequest
    {
        public DatosRequest()
        {
            this._query = new NameValueCollection();
            this._form = new NameValueCollection();
        }

        /// <summary>
        /// Retorna el valor guardado del Request Form/Query
        /// La busqueda del valor se realiza en el Form, si no encuentra valor se prueba en el QueryString
        /// </summary>
        /// <param name="name">Clave a buscar</param>        
        /// <returns></returns>
        public string this[string name]
        {
            get
            {
                string aux = null;
                aux = this._form[name];
                if (string.IsNullOrEmpty(aux))
                {
                    aux = this._query[name];
                }
                return aux;
            }
        }

        private NameValueCollection _form;
        private NameValueCollection _query;

        /// <summary>
        /// Almacena el valor del Request.QueryString
        /// </summary>
        public NameValueCollection QueryString
        {
            set
            {
                _query = value;
            }
            get
            {
                return _query;
            }
        }
        /// <summary>
        /// Almacena el valor del Request.Form
        /// </summary>
        public NameValueCollection Form
        {
            set
            {
                _form = value;
            }
            get
            {
                return _form;
            }
        }

    }
}