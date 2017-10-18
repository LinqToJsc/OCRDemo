using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Service.Dtos.SystemManage
{
    public static class RoleDtoExtensions
    {
        public static IQueryable<SystemRoleDto> ToDtos(this IQueryable<SystemRole> query)
        {
            return query.Select(x => new SystemRoleDto()
            {
                Name = x.Name,
                Id = x.Id,
                Disabled = x.Disabled,
                CreatedTime = x.CreatedTime,
                CreatorId = x.CreatorId,
                CreatorName = x.CreatorName,
                Desc = x.Desc,
                ModifiedTime = x.ModifiedTime,
                ModifierName = x.ModifierName,
                ModifierId = x.ModifierId,
                Deleted = x.Deleted,
                DeletedTime = x.DeletedTime
            });
        }
    }
}
