using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Core
{
    /// <summary>
    /// 系统时间，仅仅用于获取系统的当前时间
    /// </summary>
    public static class SystemTime
    {
        /// <summary>
        /// 系统当前的时间，除单元测试时可以修改以外，其他时候禁止修改
        /// </summary>
        public static Func<DateTime> Now = () => DateTime.Now;
    }
}
