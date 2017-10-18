using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Demo.Service.Dtos.SystemManage.Role
{
    /// <summary>
    /// 权限
    /// </summary>
    public class AuthorizeActionModel
    {
        /// <summary>
        /// 模块Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ActionUrl
        /// </summary>
        public string Url { get; set; }
    }
}
