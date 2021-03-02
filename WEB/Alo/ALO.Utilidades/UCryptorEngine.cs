using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ALO.Utilidades
{
    public class UCryptorEngine
    {
        private const string RGBKEY = "99887722";
        private const string RGBIV = "22778899";


        /// <summary>
        /// DESENCRIPTAR CADENAS 
        /// </summary>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        public static string Desencriptar(string encrypted)
        {
            byte[] data = System.Convert.FromBase64String(encrypted);
            byte[] rgbKey = System.Text.ASCIIEncoding.ASCII.GetBytes(RGBKEY);
            byte[] rgbIV = System.Text.ASCIIEncoding.ASCII.GetBytes(RGBIV);

            MemoryStream memoryStream = new MemoryStream(data.Length);
            DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Read);

            memoryStream.Write(data, 0, data.Length);
            memoryStream.Position = 0;

            string decrypted = new StreamReader(cryptoStream).ReadToEnd();

            cryptoStream.Close();

            return decrypted;
        }


        /// <summary>
        /// ENCRIPTAR CADENAS 
        /// </summary>
        /// <param name="decrypted"></param>
        /// <returns></returns>
        public static string Encriptar(string decrypted)
        {


            byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(decrypted);
            byte[] rgbKey = System.Text.ASCIIEncoding.ASCII.GetBytes(RGBKEY);
            byte[] rgbIV = System.Text.ASCIIEncoding.ASCII.GetBytes(RGBIV);

            MemoryStream memoryStream = new MemoryStream(1024);
            DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);

            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();

            byte[] result = new byte[(int)memoryStream.Position];

            memoryStream.Position = 0;
            memoryStream.Read(result, 0, result.Length);
            cryptoStream.Close();

            return System.Convert.ToBase64String(result);
        }


    }
}
