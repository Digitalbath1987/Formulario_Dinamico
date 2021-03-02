using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ALO.Entidades;
using ALO.Utilidades;

namespace ALO.Servicio
{
    public class SRestFul : IDisposable
    {



        public object ObjetoRest;


        /// <summary>
        /// DESTRUCTOR
        /// </summary>
        public void Dispose()
        {
            ObjetoRest = null;
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public SRestFul()
        {

            System.Net.ServicePointManager.DefaultConnectionLimit = 1000;
            System.Net.ServicePointManager.Expect100Continue = false;
            System.Net.ServicePointManager.UseNagleAlgorithm = false;
            WebRequest.DefaultWebProxy = null;
        }


        /// <summary>
        /// SOLICITAR INFORMACION
        /// </summary>
        /// <returns></returns>
        public int Solicitar<T>(string SP
                              , string Sistema
                              , object Parametros
                              , object Filtros)
        {




            try
            {

                //=============================================================
                // RECUPERAR OPCION DE ENVIO DE DATOS                        ==
                //=============================================================
                string Opcion = UConfiguracion.ServidorRest_Opcion;
                int ESTADO = -1;

                //=============================================================
                // GET
                //=============================================================
                if (Opcion == "G")
                {
                    ESTADO = SolicitarGet<T>(SP, Sistema, Parametros, Filtros);

                }

                //=============================================================
                // POST
                //=============================================================
                if (Opcion == "P")
                {
                    ESTADO = SolicitarPost<T>(SP, Sistema, Parametros, Filtros);

                }

                //=============================================================
                // WEBCLIENT
                //=============================================================
                if (Opcion == "W")
                {
                    ESTADO = SolicitarCliente<T>(SP, Sistema, Parametros, Filtros);

                }

                if (Opcion == "F")
                {
                    ESTADO = SolicitarWCFPost<T>(SP, Sistema, Parametros, Filtros);

                }

                return ESTADO;

            }
            catch
            {

                throw;

            }

        }


        /// <summary>
        /// SOLICITAR INFORMACION
        /// </summary>
        /// <returns></returns>
        public int SolicitarCliente<T>(string SP
                                     , string Sistema
                                     , object Parametros
                                     , object Filtros)
        {



            OUTPUT_JSON_ALO Retorno = new OUTPUT_JSON_ALO();

            try
            {

                //=============================================================
                // CONTRUCCION DE OBJETO INPUT                               ==
                //=============================================================
                INPUT_JSON_ALO ObjetoInput = new INPUT_JSON_ALO();
                ObjetoInput.R_METODO = new R_METODO { SP = SP, SISTEMA = Sistema };
                ObjetoInput.R_PARAM.PARAMETROS = Parametros;
                ObjetoInput.R_FILTRO.PARAMETROS = Filtros;

                //=============================================================
                // SE DEBE CONVERTIR EL OBJETO EN JSON PARA SER ENVIADO      ==
                //=============================================================
                SObjetoJson ObjetoParametros = new SObjetoJson();
                string Json = ObjetoParametros.Serialize(ObjetoInput);
                ObjetoParametros.Dispose();
                ObjetoParametros = null;


                //=============================================================
                // URL DE SERVICIO                                           ==
                //=============================================================
                Uri UrlDestino = new Uri(UConfiguracion.ServidorUrl_GET);

                //=============================================================
                // DATOS JSON EN URL                                         ==
                //=============================================================
                Json = "?json=" + Json;



                //=============================================================
                // RESCATO VALORES  WEB CLIENTE                              ==
                //=============================================================
                string JsonReturn = "";
                var client = new WebClient();
                client.Proxy = null;
                client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";


                var responseStream = new GZipStream(client.OpenRead(UrlDestino.ToString() + Json), CompressionMode.Decompress);
                var reader = new StreamReader(responseStream);
                JsonReturn = reader.ReadToEnd();





                //=============================================================
                // SE DEBE DESERIALIZAR RESULTADOS                           ==
                //=============================================================

                SObjetoJson ObjetoRetorno = new SObjetoJson();
                Retorno = ObjetoRetorno.Deserialize<OUTPUT_JSON_ALO>(JsonReturn);
                ObjetoRetorno.Dispose();
                ObjetoRetorno = null;

                //=============================================================
                // COMPROBAR LA EJECUCION                                    ==
                //=============================================================
                int ESTADO = Retorno.HEADER.ESTADO;
                int ID_TIPO_RETORNO = Retorno.HEADER.ID_TIPO_RETORNO;

                if (ESTADO == 1)
                {



                    //=========================================================
                    // RETURN STATUS                                         ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 1)
                    {
                        T Objeto;

                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                        SObjetoJson ObjetoSer = new SObjetoJson();
                        Objeto = ObjetoSer.Deserialize<T>(JsonDetalle);
                        ObjetoSer.Dispose();
                        ObjetoSer = null;



                        ObjetoRest = Objeto;
                    }

                    //=========================================================
                    // DATOS                                                 ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 2)
                    {
                        List<T> Lista = new List<T>();

                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                        SObjetoJson ObjetoSer = new SObjetoJson();
                        Lista = ObjetoSer.Deserialize<List<T>>(JsonDetalle);
                        ObjetoSer.Dispose();
                        ObjetoSer = null;

                        ObjetoRest = Lista;
                    }

                    //=========================================================
                    // OUTPUT                                                ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 3)
                    {
                        T Objeto;


                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                        SObjetoJson ObjetoSer = new SObjetoJson();
                        Objeto = ObjetoSer.Deserialize<T>(JsonDetalle);
                        ObjetoSer.Dispose();
                        ObjetoSer = null;


                        ObjetoRest = Objeto;
                    }

                    //=========================================================
                    // DATOS DINAMICOS                                       ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 4 || ID_TIPO_RETORNO == 5)
                    {
                        throw new Exception("ESTE METODO NO SOPORTA LOS TIPOS DE RETORNOS DINAMICOS");
                    }




                }
                else
                {
                    ObjetoRest = Retorno.ERRORES;

                }

                return ESTADO;

            }
            catch
            {

                throw;

            }

        }

        /// <summary>
        /// SOLICITAR INFORMACION
        /// </summary>
        /// <returns></returns>
        public int SolicitarGet<T>(string SP
                                 , string Sistema
                                 , object Parametros
                                 , object Filtros)
        {



            OUTPUT_JSON_ALO Retorno = new OUTPUT_JSON_ALO();

            try
            {

                //=============================================================
                // CONTRUCCION DE OBJETO INPUT                               ==
                //=============================================================
                INPUT_JSON_ALO ObjetoInput = new INPUT_JSON_ALO();
                ObjetoInput.R_METODO = new R_METODO { SP = SP, SISTEMA = Sistema };
                ObjetoInput.R_PARAM.PARAMETROS = Parametros;
                ObjetoInput.R_FILTRO.PARAMETROS = Filtros;

                //=============================================================
                // SE DEBE CONVERTIR EL OBJETO EN JSON PARA SER ENVIADO      ==
                //=============================================================

                SObjetoJson ObjetoParametros = new SObjetoJson();
                string Json = ObjetoParametros.Serialize(ObjetoInput);
                ObjetoParametros.Dispose();
                ObjetoParametros = null;

                //=============================================================
                // URL DE SERVICIO                                           ==
                //=============================================================
                Uri UrlDestino = new Uri(UConfiguracion.ServidorUrl_GET);

                //=============================================================
                // DATOS JSON EN URL                                         ==
                //=============================================================
                Json = "?json=" + Json;


                //=============================================================
                // SERVICIO RESTFUL                                          ==
                //=============================================================
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(UrlDestino.ToString() + Json);
                myRequest.Method = "GET";
                myRequest.ContentType = "application/json";
                myRequest.Accept = "application/json";
                myRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");

                myRequest.AllowAutoRedirect = false;
                myRequest.KeepAlive = false;
                myRequest.ProtocolVersion = HttpVersion.Version11;

                myRequest.Timeout = 10000;
                myRequest.ReadWriteTimeout = 10000;
                myRequest.Proxy = null;
                myRequest.ServicePoint.ConnectionLimit = 1000000;





                //=============================================================
                // RESCATO VALORES                                           ==
                //=============================================================
                string JsonReturn = "";

                using (var resp = (HttpWebResponse)myRequest.GetResponse())
                {
                    using (Stream Str = resp.GetResponseStream())
                    {


                        Stream Compresion = Str;

                        if (resp.ContentEncoding.ToLower().Contains("gzip"))
                        {
                            Compresion = new GZipStream(Compresion, CompressionMode.Decompress);
                        }
                        else
                        {
                            if (resp.ContentEncoding.ToLower().Contains("deflate"))
                            {
                                Compresion = new DeflateStream(Compresion, CompressionMode.Decompress);
                            }
                        }


                        StreamReader rd = new StreamReader(Compresion);
                        JsonReturn = rd.ReadToEnd();
                        Str.Close();
                    }
                    resp.Close();
                }

                myRequest = null;

                //=============================================================
                // SE DEBE DESERIALIZAR RESULTADOS                           ==
                //=============================================================
                SObjetoJson ObjetoRetorno = new SObjetoJson();
                Retorno = ObjetoRetorno.Deserialize<OUTPUT_JSON_ALO>(JsonReturn);
                ObjetoRetorno.Dispose();
                ObjetoRetorno = null;


                //=============================================================
                // COMPROBAR LA EJECUCION                                    ==
                //=============================================================
                int ESTADO = Retorno.HEADER.ESTADO;
                int ID_TIPO_RETORNO = Retorno.HEADER.ID_TIPO_RETORNO;

                if (ESTADO == 1)
                {



                    //=========================================================
                    // RETURN STATUS                                         ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 1)
                    {
                        T Objeto;

                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                        SObjetoJson ObjetoSer = new SObjetoJson();
                        Objeto = ObjetoSer.Deserialize<T>(JsonDetalle);
                        ObjetoSer.Dispose();
                        ObjetoSer = null;

                        ObjetoRest = Objeto;
                    }

                    //=========================================================
                    // DATOS                                                 ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 2)
                    {
                        List<T> Lista = new List<T>();

                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                        SObjetoJson ObjetoSer = new SObjetoJson();
                        Lista = ObjetoSer.Deserialize<List<T>>(JsonDetalle);
                        ObjetoSer.Dispose();
                        ObjetoSer = null;

                        ObjetoRest = Lista;
                    }

                    //=========================================================
                    // OUTPUT                                                ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 3)
                    {
                        T Objeto;

                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                        SObjetoJson ObjetoSer = new SObjetoJson();
                        Objeto = ObjetoSer.Deserialize<T>(JsonDetalle);
                        ObjetoSer.Dispose();
                        ObjetoSer = null;


                        ObjetoRest = Objeto;
                    }

                    //=========================================================
                    // DATOS DINAMICOS                                       ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 4 || ID_TIPO_RETORNO == 5)
                    {
                        throw new Exception("ESTE METODO NO SOPORTA LOS TIPOS DE RETORNOS DINAMICOS");
                    }




                }
                else
                {
                    ObjetoRest = Retorno.ERRORES;

                }

                return ESTADO;

            }
            catch
            {

                throw;

            }

        }

        /// <summary>
        /// SOLICITAR INFORMACION
        /// </summary>
        /// <returns></returns>
        public int SolicitarPost<T>(string SP
                                  , string Sistema
                                  , object Parametros
                                  , object Filtros)
        {



            OUTPUT_JSON_ALO Retorno = new OUTPUT_JSON_ALO();

            try
            {

                //=============================================================
                // CONTRUCCION DE OBJETO INPUT                               ==
                //=============================================================
                INPUT_JSON_ALO ObjetoInput = new INPUT_JSON_ALO();
                ObjetoInput.R_METODO = new R_METODO { SP = SP, SISTEMA = Sistema };
                ObjetoInput.R_PARAM.PARAMETROS = Parametros;
                ObjetoInput.R_FILTRO.PARAMETROS = Filtros;

                //=============================================================
                // SE DEBE CONVERTIR EL OBJETO EN JSON PARA SER ENVIADO      ==
                //=============================================================

                SObjetoJson ObjetoParametros = new SObjetoJson();
                string Json = ObjetoParametros.Serialize(ObjetoInput);
                ObjetoParametros.Dispose();
                ObjetoParametros = null;


                //=============================================================
                // URL DE SERVICIO                                           ==
                //=============================================================
                Uri UrlDestino = new Uri(UConfiguracion.ServidorUrl_POST);


                //=============================================================
                // DATOS JSON EN URL                                         ==
                //=============================================================
                Json = "json=" + Json;
                byte[] data = Encoding.UTF8.GetBytes(Json);




                //=============================================================
                // SERVICIO RESTFUL                                          ==
                //=============================================================
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(UrlDestino.ToString());
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = data.Length;
                myRequest.Accept = "application/json";
                myRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");

                myRequest.AllowAutoRedirect = false;
                myRequest.KeepAlive = false;
                myRequest.ProtocolVersion = HttpVersion.Version11;

                myRequest.Timeout = 10000;
                myRequest.ReadWriteTimeout = 10000;
                myRequest.Proxy = null;
                myRequest.ServicePoint.ConnectionLimit = 1000000;


                //=============================================================
                // PASAR POST
                //=============================================================
                using (var stream = myRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }




                //=============================================================
                // RESCATO VALORES                                           ==
                //=============================================================
                string JsonReturn = "";

                using (var resp = (HttpWebResponse)myRequest.GetResponse())
                {
                    using (Stream Str = resp.GetResponseStream())
                    {


                        Stream Compresion = Str;

                        if (resp.ContentEncoding.ToLower().Contains("gzip"))
                        {
                            Compresion = new GZipStream(Compresion, CompressionMode.Decompress);
                        }
                        else
                        {
                            if (resp.ContentEncoding.ToLower().Contains("deflate"))
                            {
                                Compresion = new DeflateStream(Compresion, CompressionMode.Decompress);
                            }
                        }


                        StreamReader rd = new StreamReader(Compresion);
                        JsonReturn = rd.ReadToEnd();
                        Str.Close();
                    }
                    resp.Close();
                }


                myRequest = null;


                //=============================================================
                // SE DEBE DESERIALIZAR RESULTADOS                           ==
                //=============================================================
                SObjetoJson ObjetoRetorno = new SObjetoJson();
                Retorno = ObjetoRetorno.Deserialize<OUTPUT_JSON_ALO>(JsonReturn);
                ObjetoRetorno.Dispose();
                ObjetoRetorno = null;


                //=============================================================
                // COMPROBAR LA EJECUCION                                    ==
                //=============================================================
                int ESTADO = Retorno.HEADER.ESTADO;
                int ID_TIPO_RETORNO = Retorno.HEADER.ID_TIPO_RETORNO;

                if (ESTADO == 1)
                {



                    //=========================================================
                    // RETURN STATUS                                         ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 1)
                    {
                        T Objeto;

                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                        SObjetoJson ObjetoSer = new SObjetoJson();
                        Objeto = ObjetoSer.Deserialize<T>(JsonDetalle);
                        ObjetoSer.Dispose();
                        ObjetoSer = null;

                        ObjetoRest = Objeto;
                    }

                    //=========================================================
                    // DATOS                                                 ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 2)
                    {
                        List<T> Lista = new List<T>();

                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                        SObjetoJson ObjetoSer = new SObjetoJson();
                        Lista = ObjetoSer.Deserialize<List<T>>(JsonDetalle);
                        ObjetoSer.Dispose();
                        ObjetoSer = null;

                        ObjetoRest = Lista;
                    }

                    //=========================================================
                    // OUTPUT                                                ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 3)
                    {
                        T Objeto;


                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                        SObjetoJson ObjetoSer = new SObjetoJson();
                        Objeto = ObjetoSer.Deserialize<T>(JsonDetalle);
                        ObjetoSer.Dispose();
                        ObjetoSer = null;

                        ObjetoRest = Objeto;
                    }

                    //=========================================================
                    // DATOS DINAMICOS                                       ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 4 || ID_TIPO_RETORNO == 5)
                    {
                        throw new Exception("ESTE METODO NO SOPORTA LOS TIPOS DE RETORNOS DINAMICOS");
                    }




                }
                else
                {
                    ObjetoRest = Retorno.ERRORES;

                }

                return ESTADO;

            }
            catch
            {

                throw;

            }

        }

        /// <summary>
        /// SOLICITAR INFORMACION
        /// </summary>
        /// <returns></returns>
        public int SolicitarWCFPost<T>(string SP
                                    , string Sistema
                                    , object Parametros
                                    , object Filtros)
        {



            OUTPUT_JSON_ALO Retorno = new OUTPUT_JSON_ALO();

            try
            {

                //=============================================================
                // CONTRUCCION DE OBJETO INPUT                               ==
                //=============================================================
                INPUT_JSON_ALO ObjetoInput = new INPUT_JSON_ALO();
                ObjetoInput.R_METODO = new R_METODO { SP = SP, SISTEMA = Sistema };
                ObjetoInput.R_PARAM.PARAMETROS = Parametros;
                ObjetoInput.R_FILTRO.PARAMETROS = Filtros;

                //=============================================================
                // SE DEBE CONVERTIR EL OBJETO EN JSON PARA SER ENVIADO      ==
                //=============================================================


                SObjetoJson ObjetoParametros = new SObjetoJson();
                string Json = ObjetoParametros.Serialize(ObjetoInput);
                ObjetoParametros.Dispose();
                ObjetoParametros = null;


                //=============================================================
                // URL DE SERVICIO                                           ==
                //=============================================================
                Uri UrlDestino = new Uri(UConfiguracion.ServidorWCF);


                //=============================================================
                // DATOS JSON EN URL                                         ==
                //=============================================================
                StringBuilder StrWebservices = new StringBuilder();
                string econding = @"<?xml version=""1.0"" encoding=""utf-8""?>";
                string EncabezadoWS = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:alo=""alo.ibrlatam.com"">";
                string CabezeraWS = @"<soapenv:Header/><soapenv:Body><alo:RF_DB_POST>";
                string FooterWS = @"</alo:RF_DB_POST></soapenv:Body> </soapenv:Envelope>";


                StrWebservices.AppendLine(econding);
                StrWebservices.AppendLine(EncabezadoWS);
                StrWebservices.AppendLine(CabezeraWS);
                StrWebservices.AppendLine("<alo:json>" + Json + "</alo:json>");
                StrWebservices.AppendLine(FooterWS);



                //=============================================================
                // SERVICIO RESTFUL                                          ==
                //=============================================================
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(UrlDestino.ToString());
                myRequest.Method = "POST";
                myRequest.ContentType = "text/xml; charset=utf-8";
                myRequest.Headers.Add(@"SOAPAction: ""alo.ibrlatam.com/IServicioRestAlo/RF_DB_POST""");
                myRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");


                myRequest.AllowAutoRedirect = false;
                myRequest.KeepAlive = false;
                myRequest.ProtocolVersion = HttpVersion.Version11;

                myRequest.Timeout = 10000;
                myRequest.ReadWriteTimeout = 10000;
                myRequest.Proxy = null;
                myRequest.ServicePoint.ConnectionLimit = 1000000;


                //=============================================================
                // PASAR POST
                //=============================================================
                using (StreamWriter writer = new StreamWriter(myRequest.GetRequestStream()))
                {
                    writer.Write(StrWebservices.ToString());
                }


                //=============================================================
                // RESCATO VALORES                                           ==
                //=============================================================
                string JsonReturn = "";

                using (var resp = (HttpWebResponse)myRequest.GetResponse())
                {
                    using (Stream Str = resp.GetResponseStream())
                    {


                        Stream Compresion = Str;

                        if (resp.ContentEncoding.ToLower().Contains("gzip"))
                        {
                            Compresion = new GZipStream(Compresion, CompressionMode.Decompress);
                        }
                        else
                        {
                            if (resp.ContentEncoding.ToLower().Contains("deflate"))
                            {
                                Compresion = new DeflateStream(Compresion, CompressionMode.Decompress);
                            }
                        }


                        StreamReader rd = new StreamReader(Compresion);
                        JsonReturn = rd.ReadToEnd();
                        Str.Close();
                    }
                    resp.Close();
                }


                myRequest = null;

                //=============================================================
                // PARSING XML                                               ==
                //=============================================================
                XmlDocument doc = new XmlDocument();
                try
                {

                    doc.LoadXml(JsonReturn);

                }
                catch
                {

                    if (JsonReturn.Length > 0)
                    {

                        throw new Exception("XML DEVUELTO NO CORRESPONDE POR PARTE DE WCF(1)");
                    }


                }

                //=============================================================
                // REEMPLAZO DE CODIGO                                       ==
                //=============================================================
                try
                {


                    var soapBody = doc.GetElementsByTagName("s:Body")[0];
                    string innerObject = soapBody.InnerXml;
                    innerObject = innerObject.Replace(@"xmlns=""alo.ibrlatam.com""", "");


                    doc.LoadXml(innerObject);



                }
                catch
                {
                    throw new Exception("XML DEVUELTO NO CORRESPONDE POR PARTE DE WCF(2)");

                }


                //=============================================================
                // LECTURA DE NODOS
                //=============================================================
                XmlNodeList DireccionNodo = null;
                string NewJson = "";

                try
                {
                    /*----------------------------------------------------*/
                    /* LECTURA DE NODOS                                   */
                    /*----------------------------------------------------*/
                    DireccionNodo = doc.SelectNodes(@"//RF_DB_POSTResponse");
                    foreach (XmlNode Nodo in DireccionNodo)
                    {

                        if (Nodo != null)
                        {
                            NewJson = Nodo["RF_DB_POSTResult"].InnerText;

                        }
                    }
                }
                catch
                {
                    throw new Exception("ESTRUCTURA WCF NO TRAJO DATOS JSON DE CONFIGURACIÓN");

                }

                //=============================================================
                // VER SI ESTE CONTIENE DATOS
                //=============================================================
                if (NewJson.Length <= 0)
                {
                    throw new Exception("JSON RETURN WCF VIENE VACIO");
                }



                //=============================================================
                // SE DEBE DESERIALIZAR RESULTADOS                           ==
                //=============================================================
                SObjetoJson ObjetoRetorno = new SObjetoJson();
                Retorno = ObjetoRetorno.Deserialize<OUTPUT_JSON_ALO>(NewJson);
                ObjetoRetorno.Dispose();
                ObjetoRetorno = null;

                //=============================================================
                // COMPROBAR LA EJECUCION                                    ==
                //=============================================================
                int ESTADO = Retorno.HEADER.ESTADO;
                int ID_TIPO_RETORNO = Retorno.HEADER.ID_TIPO_RETORNO;

                if (ESTADO == 1)
                {



                    //=========================================================
                    // RETURN STATUS                                         ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 1)
                    {
                        T Objeto;

                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                        SObjetoJson ObjetoSer = new SObjetoJson();
                        Objeto = ObjetoSer.Deserialize<T>(JsonDetalle);
                        ObjetoSer.Dispose();
                        ObjetoSer = null;


                        ObjetoRest = Objeto;
                    }

                    //=========================================================
                    // DATOS                                                 ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 2)
                    {
                        List<T> Lista = new List<T>();

                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                        SObjetoJson ObjetoSer = new SObjetoJson();
                        Lista = ObjetoSer.Deserialize<List<T>>(JsonDetalle);
                        ObjetoSer.Dispose();
                        ObjetoSer = null;

                        ObjetoRest = Lista;
                    }

                    //=========================================================
                    // OUTPUT                                                ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 3)
                    {
                        T Objeto;

                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                        SObjetoJson ObjetoSer = new SObjetoJson();
                        Objeto = ObjetoSer.Deserialize<T>(JsonDetalle);
                        ObjetoSer.Dispose();
                        ObjetoSer = null;

                        ObjetoRest = Objeto;
                    }

                    //=========================================================
                    // DATOS DINAMICOS                                       ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 4 || ID_TIPO_RETORNO == 5)
                    {
                        throw new Exception("ESTE METODO NO SOPORTA LOS TIPOS DE RETORNOS DINAMICOS");
                    }




                }
                else
                {
                    ObjetoRest = Retorno.ERRORES;

                }

                return ESTADO;

            }
            catch
            {

                throw;

            }

        }
        /// <summary>
        /// SOLICITAR INFORMACION
        /// </summary>
        /// <param name="SP"></param>
        /// <param name="Sistema"></param>
        /// <param name="Parametros"></param>
        /// <param name="Filtros"></param>
        /// <returns></returns>
        public int SolicitarData(string SP
                                         , string Sistema
                                         , object Parametros
                                         , object Filtros)
        {


            OUTPUT_JSON_ALO Retorno = new OUTPUT_JSON_ALO();

            try
            {


                //=============================================================
                // CONTRUCCION DE OBJETO INPUT                               ==
                //=============================================================
                INPUT_JSON_ALO ObjetoInput = new INPUT_JSON_ALO();
                ObjetoInput.R_METODO = new R_METODO { SP = SP, SISTEMA = Sistema };
                ObjetoInput.R_PARAM.PARAMETROS = Parametros;
                ObjetoInput.R_FILTRO.PARAMETROS = Filtros;


                //=============================================================
                // SE DEBE CONVERTIR EL OBJETO EN JSON PARA SER ENVIADO      ==
                //=============================================================
                SObjetoJson ObjetoParametros = new SObjetoJson();
                string Json = ObjetoParametros.Serialize(ObjetoInput);
                ObjetoParametros.Dispose();
                ObjetoParametros = null;

                //=============================================================
                // URL DE SERVICIO                                           ==
                //=============================================================
                Uri UrlDestino = new Uri(UConfiguracion.ServidorUrl_GET);


                //=============================================================
                // DATOS JSON EN URL                                         ==
                //=============================================================
                Json = "?json=" + Json;

                //=============================================================
                // SERVICIO RESTFUL                                          ==
                //=============================================================
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(UrlDestino.ToString() + Json);
                myRequest.Method = "GET";
                myRequest.ContentType = "application/json";
                myRequest.Accept = "application/json";
                myRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                myRequest.Timeout = 10000;
                myRequest.AllowWriteStreamBuffering = false;
                myRequest.Proxy = null;
                myRequest.KeepAlive = false;
                myRequest.ServicePoint.Expect100Continue = false;


                //=============================================================
                // RESCATO VALORES                                           ==
                //=============================================================
                string JsonReturn = "";

                using (var resp = (HttpWebResponse)myRequest.GetResponse())
                {
                    using (Stream Str = resp.GetResponseStream())
                    {


                        Stream Compresion = Str;

                        if (resp.ContentEncoding.ToLower().Contains("gzip"))
                        {
                            Compresion = new GZipStream(Compresion, CompressionMode.Decompress);
                        }
                        else
                        {
                            if (resp.ContentEncoding.ToLower().Contains("deflate"))
                            {
                                Compresion = new DeflateStream(Compresion, CompressionMode.Decompress);
                            }
                        }


                        StreamReader rd = new StreamReader(Compresion);
                        JsonReturn = rd.ReadToEnd();
                        Str.Close();
                    }
                    resp.Close();
                }


                //=============================================================
                // SE DEBE DESERIALIZAR RESULTADOS                           ==
                //=============================================================
                SObjetoJson ObjetoRetorno = new SObjetoJson();
                Retorno = ObjetoRetorno.Deserialize<OUTPUT_JSON_ALO>(JsonReturn);
                ObjetoRetorno.Dispose();
                ObjetoRetorno = null;


                //=============================================================
                // COMPROBAR LA EJECUCION                                    ==
                //=============================================================
                int ESTADO = Retorno.HEADER.ESTADO;
                int ID_TIPO_RETORNO = Retorno.HEADER.ID_TIPO_RETORNO;

                if (ESTADO == 1)
                {

                    //=========================================================
                    // RETORNA DATOS                                         ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 4 || ID_TIPO_RETORNO == 5)
                    {

                        if (ID_TIPO_RETORNO == 4)
                        {
                            DataTable Objeto;
                            string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                            SObjetoJson ObjetoSer = new SObjetoJson();
                            Objeto = ObjetoSer.Deserialize<DataTable>(JsonDetalle);
                            ObjetoSer.Dispose();
                            ObjetoSer = null;

                            ObjetoRest = Objeto;
                        }

                        if (ID_TIPO_RETORNO == 5)
                        {
                            DataSet Objeto;
                            string JsonDetalle = Retorno.RESULT.DETALLES.ToString();
                            SObjetoJson ObjetoSer = new SObjetoJson();
                            Objeto = ObjetoSer.Deserialize<DataSet>(JsonDetalle);
                            ObjetoSer.Dispose();
                            ObjetoSer = null;

                            ObjetoRest = Objeto;
                        }

                    }
                    else
                    {

                        throw new Exception("ESTE METODO SOLO SOPORTA LOS TIPOS DE RETORNOS DINAMICOS");

                    }


                }
                else
                {
                    ObjetoRest = Retorno.ERRORES;

                }

                return ESTADO;

            }
            catch
            {

                throw;

            }




        }


    }


}
