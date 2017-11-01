using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Service.Dtos.SystemManage.Action
{
    public static class ActionDtoExtensions
    {
        public static IQueryable<SystemActionDto> ToDtos(this IQueryable<SystemAction> query)
        {
            return query.Select(x => new SystemActionDto()
            {
                Url = x.Url,
                Code = x.Code,
                Desc = x.Desc,
                Disabled = x.Disabled,
                Displayed = x.Displayed,
                Id = x.Id,
                ModuleName = x.SystemModule.Name,
                ModuleId = x.ModuleId,
                Name = x.Name,
                Sort = x.Sort,
                ModuleSort = x.SystemModule.Sort,
               // ActionType = x.ActionType,
                ActionParentId = x.ActionParentId
            });
        }
    }
}
