using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Core.Tools
{
    public class DESEncrypt
    {
        private static string DESKey = "TDF_2017";

        #region ========加密========
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="sourceStr"></param>
        /// <returns></returns>
        public static string Encrypt(string sourceStr)
        {
            return Encrypt(sourceStr, DESKey);
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="sourceStr"></param> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static string Encrypt(string sourceStr, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            var inputByteArray = Encoding.Default.GetBytes(sourceStr);
            des.Key = Encoding.ASCII.GetBytes(MD5Helper.GetMD5(key,16).Substring(0,8));
            des.IV = Encoding.ASCII.GetBytes(MD5Helper.GetMD5(key, 16).Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (var b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptedStr"></param>
        /// <returns></returns>
        public static string Decrypt(string encryptedStr)
        {
            if (!string.IsNullOrEmpty(encryptedStr))
            {
                return Decrypt(encryptedStr, DESKey);
            }
            return "";
        }

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="encryptedStr"></param> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static string Decrypt(string encryptedStr, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            var len = encryptedStr.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x;
            for (x = 0; x < len; x++)
            {
                var i = Convert.ToInt32(encryptedStr.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = Encoding.ASCII.GetBytes(MD5Helper.GetMD5(key, 16).Substring(0, 8));
            des.IV = Encoding.ASCII.GetBytes(MD5Helper.GetMD5(key, 16).Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion
    }
}
