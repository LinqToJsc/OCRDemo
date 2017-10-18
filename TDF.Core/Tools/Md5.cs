using System;
using System.Security.Cryptography;
using System.Text;

namespace TDF.Core.Tools
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public class MD5Helper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string GetMD5(string input, int code)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var bytResult = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            var strEncrypt = BitConverter.ToString(bytResult).Replace("-", "");
            if (code == 16)
            {
                strEncrypt = strEncrypt.Substring(8, 16);
            }
            return strEncrypt;
        }

        public static string GetMD5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            var builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static bool VerifyMD5Hash(string input, string hash)
        {
            string hashOfInput = GetMD5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }
    }
}
