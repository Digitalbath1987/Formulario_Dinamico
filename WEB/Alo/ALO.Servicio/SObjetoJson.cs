using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ALO.Servicio
{
    public class SObjetoJson : IDisposable
    {

        /// <summary>
        /// DESTRUCTOR
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public SObjetoJson()
        {


        }

        /// <summary>
        /// SERIALIZACION DE OBJETO
        /// </summary>
        /// <returns></returns>
        public string Serialize(Object Objeto)
        {


            //===============================================================
            // DECLARACION DE VARIABLES
            //===============================================================
            StringBuilder Sb = new StringBuilder();
            StringWriter Sw = new StringWriter(Sb);
            JsonWriter Writer = new JsonTextWriter(Sw);
            JsonSerializer Serializer = new JsonSerializer();


            try
            {



                //===========================================================
                // SERIALIZAR
                //===========================================================                
                Serializer.Serialize(Writer, Objeto);
                String JsonResult = Sb.ToString();


                //===========================================================
                // RETORNO
                //=========================================================== 
                return JsonResult;


            }
            catch
            {
                throw;
            }
            finally
            {

                Sb.Clear();
                Sw.Close();
                Writer.Close();
                Serializer = null;


            }

        }

        /// <summary>
        /// DESERIALIZAR OBJETO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns></returns>
        public T Deserialize<T>(string Json)
        {


            //===============================================================
            // DECLARACION DE VARIABLES
            //===============================================================
            T ResutObjeto;
            MemoryStream StreamJson = null;
            StreamReader ReaderJson = null;
            JsonTextReader Reader = null;
            JsonSerializer SerializerJson = null;


            try
            {

                //===========================================================
                // DECLARACION DE VARIABLES
                //=========================================================== 
                SerializerJson = new JsonSerializer();


                //===========================================================
                // JSON A BYTE
                //=========================================================== 
                byte[] byteArray = Encoding.UTF8.GetBytes(Json);
                StreamJson = new MemoryStream(byteArray);


                //===========================================================
                // LECTURA
                //=========================================================== 
                ReaderJson = new StreamReader(StreamJson);
                Reader = new JsonTextReader(ReaderJson);


                //===========================================================
                // RETORNO
                //=========================================================== 
                ResutObjeto = SerializerJson.Deserialize<T>(Reader);




                return ResutObjeto;


            }
            catch
            {
                throw;
            }
            finally
            {


                try
                {
                    StreamJson.Close();
                    ReaderJson.Close();
                    Reader.Close();
                }
                catch { }

                StreamJson = null;
                ReaderJson = null;
                Reader = null;
                SerializerJson = null;


            }

        }

    }
}
