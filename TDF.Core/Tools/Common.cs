using System;

namespace TDF.Core.Tools
{
    public class Common
    {
        /// <summary>
        /// 生成随机Key
        /// </summary>
        /// <param name="codeNum">生成的长度</param>
        /// <returns></returns>
        public static string BuildKey(int codeNum = 16)
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }
    }
}
