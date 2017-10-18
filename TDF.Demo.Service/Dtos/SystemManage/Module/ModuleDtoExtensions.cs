using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Service.Dtos.SystemManage.Module
{
    public static class ModuleDtoExtensions
    {
        public static IQueryable<SystemModuleDto> ToDtos(this IQueryable<SystemModule> query)
        {
            return query.Select(x => new SystemModuleDto()
            {
                Name = x.Name,
                ParentId = x.ParentId,
                Sort = x.Sort,
                Id = x.Id,
                Disabled = x.Disabled,
                Code = x.Code,
                Desc = x.Desc,
                IconClass = x.IconClass,
                Displayed = x.Displayed
            });
        }

        public static IQueryable<SystemModuleDto> ToFullDtos(this IQueryable<SystemModule> query)
        {

            return query.Select(x => new SystemModuleDto()
            {
                Name = x.Name,
                ParentId = x.ParentId,
                Sort = x.Sort,
                Id = x.Id,
                Disabled = x.Disabled,
                Code = x.Code,
                Desc = x.Desc,
                IconClass = x.IconClass,
                Displayed = x.Displayed,
                SystemActionDtos = x.SystemActions
                    .Where(z => z.Deleted == false && z.Displayed)
                    .Select(y =>
                        new SystemActionDto()
                        {
                            Url = y.Url,
                            Code = y.Code,
                            Desc = y.Desc,
                            Disabled = y.Disabled,
                            Displayed = y.Displayed,
                            Id = y.Id,
                            ModuleId = y.ModuleId,
                            Name = y.Name,
                            Sort = y.Sort,
                            IsDelete = y.Deleted == false
                        }
                    ).ToList()
            });
        }

        public static IQueryable<SystemModuleDto> ToFullDtosByActionIds(this IQueryable<SystemModule> query,
            List<Guid> actionIds)
        {
            return query.Select(x => new SystemModuleDto()
            {
                Name = x.Name,
                ParentId = x.ParentId,
                Sort = x.Sort,
                Id = x.Id,
                Disabled = x.Disabled,
                Code = x.Code,
                Desc = x.Desc,
                IconClass = x.IconClass,
                Displayed = x.Displayed,
                SystemActionDtos = x.SystemActions
                    .Where(z => z.Deleted == false)
                    //根据权限ID查询具有该权限的ActionId
                    .Where(z => actionIds.Contains(z.Id))
                    .Select(y =>
                        new SystemActionDto()
                        {
                            Url = y.Url,
                            Code = y.Code,
                            Desc = y.Desc,
                            Disabled = y.Disabled,
                            Displayed = y.Displayed,
                            Id = y.Id,
                            ModuleId = y.ModuleId,
                            Name = y.Name,
                            Sort = y.Sort,
                            IsDelete = y.Deleted == false
                        }
                    ).ToList()
            });
        }
    }
}
