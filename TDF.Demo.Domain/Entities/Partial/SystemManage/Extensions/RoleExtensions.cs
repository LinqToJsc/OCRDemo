using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Demo.Domain.Entities.Partial.SystemManage.Extensions
{
    public static class RoleExtensions
    {
        public static IQueryable<SystemRole> WhereEnabled(this IQueryable<SystemRole> query, bool? enabled)
        {
            if (enabled == null)
            {
                return query;
            }
            return query.Where(x => x.Disabled != enabled.Value);
        }

        public static IQueryable<SystemRole> WhereByMemberId(this IQueryable<SystemRole> query, Guid memberId)
        {
            return query.Where(x => x.SystemMemberRoles.Any(y => y.SystemMemberId == memberId));
        }
    }
}
