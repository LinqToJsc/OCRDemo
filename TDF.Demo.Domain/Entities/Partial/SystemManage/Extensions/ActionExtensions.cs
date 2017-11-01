using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities.Partial.SystemManage.Enums;

namespace TDF.Demo.Domain.Entities.Partial.SystemManage.Extensions
{
    public static class ActionExtensions
    {
        public static IQueryable<SystemAction> WhereRoleId(this IQueryable<SystemAction> query, Guid roleId)
        {
            return query.Where(x => x.SystemActionRoles.Any(y => y.RoleId == roleId));
        }

        public static IQueryable<SystemAction> WhereByOperId(this IQueryable<SystemAction> query, Guid? operId)
        {
            if (operId == null)
            {
                return query;
            }
            return
                query.Where(
                    x =>
                        x.SystemActionRoles.Any(
                            y => y.SystemRole.SystemMemberRoles.Any(
                                z => z.SystemMemberId == operId.Value)));
        }

        public static IQueryable<SystemAction> WhereByActionType(this IQueryable<SystemAction> query, SystemActionType? actionType)
        {
            return query;
            //if (actionType == null)
            //{
            //    return query;
            //}
            //return
            //    query.Where(
            //        x =>
            //            x.ActionType== actionType.Value);
        }
    }
}
