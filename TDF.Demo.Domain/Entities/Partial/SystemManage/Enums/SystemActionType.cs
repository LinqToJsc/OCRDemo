using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Demo.Domain.Entities.Partial.SystemManage.Enums
{
    /// <summary>
    /// 系统资源类型
    /// </summary>
    public enum SystemActionType
    {
        /// <summary>
        /// 页面
        /// </summary>
        Page = 1,

        /// <summary>
        /// 功能操作
        /// </summary>
        Function = 2
    }

    public static class SystemActionTypeExtensions
    {
        public static string GetDisplayName(this SystemActionType actionType)
        {
            switch (actionType)
            {
                case SystemActionType.Page:
                    return "页面";
                case SystemActionType.Function:
                    return "功能操作";
                default:
                    return "页面";
            }
        }
    }
}
