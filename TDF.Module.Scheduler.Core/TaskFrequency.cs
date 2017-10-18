using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Module.Scheduler.Core
{
    /// <summary>
    /// 调度平率
    /// </summary>
    public enum TaskFrequency
    {
        /// <summary>
        /// 用户自定义,设置此枚举需重写IsTimeToWork方法
        /// </summary>
        Custom = 0,

        /// <summary>
        /// 每分钟
        /// </summary>
        Minute = 1,

        /// <summary>
        /// 每小时
        /// </summary>
        Hourly = 2,

        /// <summary>
        /// 每天
        /// </summary>
        Daily = 3,

        /// <summary>
        /// 每周
        /// </summary>
        Weekly = 4,

        /// <summary>
        /// 每月
        /// </summary>
        Monthly = 5
    }
}
