using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Demo.Domain.Entities.Partial.SystemManage.Extensions
{
    public static class ModuleExtensions
    {
        public static IQueryable<SystemModule> WhereByEnabled(this IQueryable<SystemModule> query, bool? enabled)
        {
            if (enabled == null)
            {
                return query;
            }
            return query.Where(x => x.Disabled != enabled.Value);
        }

        public static IQueryable<SystemModule> WhereByOperId(this IQueryable<SystemModule> query, Guid? operId)
        {
            if (operId == null)
            {
                return query;
            }
            return
                query.Where(
                    x =>
                        x.SystemModuleRoles.Any(
                            y => y.SystemRole.SystemMemberRoles.Any(
                                z => z.SystemMemberId == operId.Value)));
        }

        /// <summary>
        /// 查询一组角色所拥有的 Module
        /// </summary>
        /// <param name="query"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static IQueryable<SystemModule> WhereByRoleIds(this IQueryable<SystemModule> query, List<Guid> roleIds)
        {
            if (roleIds == null || roleIds.Count == 0)
            {
                return query;
            }
            return
                query.Where(
                    x =>
                        x.SystemModuleRoles.Where(y => roleIds.Contains(y.RoleId))
                            .Select(y => y.ModuleId)
                            .Any(y => y == x.Id));
        }
    }
}
