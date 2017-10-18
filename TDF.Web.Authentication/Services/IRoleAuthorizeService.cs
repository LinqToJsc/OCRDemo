using System;
using System.Collections.Generic;

namespace TDF.Web.Authentication.Services
{
    /// <summary>
    /// 权限验证服务
    /// </summary>
    public interface IRoleAuthorizeService
    {
        /// <summary>
        /// 角色是否可以访问
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="roleIds"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        bool ActionValidate(Guid memberId, List<Guid> roleIds, string action);

        /// <summary>
        /// 分配角色
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="roleIds"></param>
        void Assignment(Guid memberId, List<Guid> roleIds);
    }
}
