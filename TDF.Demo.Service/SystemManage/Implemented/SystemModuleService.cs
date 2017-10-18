using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TDF.Core.Exceptions;
using TDF.Core.Ioc;
using TDF.Core.Models;
using TDF.Core.Tools;
using TDF.Data.EntityFramework.Repository;
using TDF.Data.Repository;
using TDF.Demo.Domain.Entities;
using TDF.Demo.Domain.Entities.Extensions;
using TDF.Demo.Domain.Entities.Partial.SystemManage.Enums;
using TDF.Demo.Domain.Entities.Partial.SystemManage.Extensions;
using TDF.Demo.Domain.Infrastructure;
using TDF.Demo.Service.Dtos.SystemManage;
using TDF.Demo.Service.Dtos.SystemManage.Action;
using TDF.Demo.Service.Dtos.SystemManage.Module;

namespace TDF.Demo.Service.SystemManage
{
    public class SystemModuleService : ISystemModuleService
    {
        public void AddMoudule(SystemModuleDto module)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemModule>>())
            {
                module.Code = string.IsNullOrEmpty(module.Code) ? Common.BuildKey() : module.Code;
                var entity = AutoMapper.Mapper.Map<SystemModule>(module);
                entity.CreateByOperator();
                repository.Insert(entity);
            }
        }

        public IPagedList<SystemModuleDto> GetModulePagedList(ModuleCriteria criteria)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemModule>>())
            {
                return repository.IQueryable().WhereNotDelete()
                    .WhereByEnabled(criteria.Enabled)
                    .OrderBy(x => x.ParentId).ThenBy(x => x.Sort)
                    .ToDtos()
                    .ToPageResult(criteria.PageIndex, criteria.PageSize);
            }
        }

        public SystemModuleDto GetModuleById(Guid id)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemModule>>())
            {
                var entity = repository.FindEntity(id);
                if (entity.IsNullOrDeleted())
                {
                    throw new KnownException("模块未找到");
                }
                return AutoMapper.Mapper.Map<SystemModuleDto>(entity);
            }
        }

        public void UpdateModule(SystemModuleDto module)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemModule>>())
            {
                var entity = repository.FindEntity(module.Id);
                if (entity.IsNullOrDeleted())
                {
                    throw new KnownException("菜单未找到");
                }
                entity.ParentId = module.ParentId;
                entity.Name = module.Name;
                entity.IconClass = module.IconClass;
                entity.Sort = module.Sort;
                entity.Displayed = module.Displayed;
                entity.Disabled = module.Disabled;
                entity.ModifyByOperator();
                repository.Update(entity);
            }
        }

        public void DeleteModuleById(Guid moduleId)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemModule>>())
            {
                var entity = repository.FindEntity(moduleId);
                if (entity.IsNullOrDeleted())
                {
                    throw new KnownException("该菜单已被删除");
                }
                if (!entity.Disabled)
                {
                    throw new KnownException("次菜单未被禁用，不允许删除");
                }
                entity.RemoveByOperator();
                repository.Delete(entity);
            }
        }

        public List<SystemModuleDto> GetModuleList()
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemModule>>())
            {
                return repository.IQueryable()
                    .WhereNotDelete()
                    .OrderBy(x => x.Sort)
                    .ToList()
                    .Select(AutoMapper.Mapper.Map<SystemModuleDto>)
                    .ToList();
            }
        }

        public List<SystemModuleDto> GetModuleIncludeActionList(Guid? operId = null, List<Guid> roleIds = null)
        {
            // 获取所有菜单和资源
            if (operId == null)
            {
                using (var repository = Ioc.Resolve<IRepositoryBase<SystemModule>>())
                {
                    return repository.IQueryable()
                        .WhereNotDelete()
                        .Include(x => x.SystemActions)
                        //.Where(x=> !x.SystemActions.Any()||x.SystemActions.Any(y=>y.DeleteMark == null || y.DeleteMark.Value == false))
                        .ToFullDtos()
                        .OrderBy(x => x.Sort)
                        .ToList();
                }
            }

            // 根据用户ID 获取该用户拥有权限的 菜单和资源
            return GetModuleIncludeActionListByRoleIds(roleIds);
        }



        private List<SystemModuleDto> GetModuleIncludeActionListByRoleIds(List<Guid> roleIds)
        {
            if (roleIds == null || roleIds.Count == 0)
                return new List<SystemModuleDto>();
            using (var repository = Ioc.Resolve<IRepositoryBase>())
            {
                var actionIds = repository.IQueryable<SystemAction>()
                    .WhereNotDelete()
                    .Where(z =>
                        z.SystemActionRoles
                            .Where(y => roleIds.Contains(y.RoleId))
                            .Select(y => y.ActionId)
                            .Any(y => y == z.Id))
                    .Select(x => x.Id)
                    .Distinct() //去重
                    .ToList();
                //查询未被禁用 且“显示到菜单”的Module 以及对应的所有资源
                return repository.IQueryable<SystemModule>()
                    .WhereNotDelete()
                    .WhereByEnabled(true)
                    .Where(x => x.Displayed)
                    //.Where(x=> !x.SystemActions.Any()||x.SystemActions.Any(y=>y.DeleteMark == null || y.DeleteMark.Value == false))
                    .WhereByRoleIds(roleIds)
                    .ToFullDtosByActionIds(actionIds)
                    .OrderBy(x => x.Sort)
                    .ToList();
            }
        }


        public void EnableModule(Guid moduleId, bool enabled)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemModule>>())
            {
                var entity = repository.FindEntity(moduleId);
                entity.Disabled = !enabled;
                entity.ModifyByOperator();
                repository.Update(entity);
            }
        }


        public List<SystemActionDto> GetActionList()
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemAction>>())
            {
                return repository.IQueryable()
                    .WhereNotDelete()
                    .ToDtos()
                    .OrderBy(x => x.ModuleSort)
                    .ThenBy(x => x.Sort)
                    .ToList();
            }
        }


        public List<SystemActionDto> GetActionTableList()
        {
            List<SystemActionDto> list = new List<SystemActionDto>();
            List<SystemActionDto> actionTable = new List<SystemActionDto>();
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemAction>>())
            {
                list = repository.IQueryable()
                   .WhereNotDelete()
                   .ToDtos()
                   .OrderBy(x=>x.ModuleSort)
                   .ThenBy(x=>x.Sort)
                   .ToList();
            }
            var pageAction = list.Where(x => x.ActionType == SystemActionType.Page).ToList();
            var functionAction = list.Where(x => x.ActionType == SystemActionType.Function).ToList();
            pageAction.ForEach(item =>
            {
                actionTable.Add(item);
                var functionActionByPage = functionAction.Where(x => x.ActionParentId == item.Id.Value).ToList();
                actionTable.AddRange(functionActionByPage);
            });
            return actionTable;
        }

        public List<SystemActionDto> GetActionByActionType(SystemActionType actionType)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemAction>>())
            {
                return repository.IQueryable()
                    .WhereNotDelete()
                    .WhereByActionType(actionType)
                    .ToDtos()
                    .OrderBy(x => x.ModuleSort)
                    .ThenBy(x => x.Sort)
                    .ToList();
            }
        }

        public void AddAction(SystemActionDto action)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemAction>>())
            {
                var sort = repository.IQueryable().Where(x => x.ActionType == action.ActionType).Max(x => (int?)x.Sort) ?? 0;

                var entity = AutoMapper.Mapper.Map<SystemAction>(action);
                entity.Code = Common.BuildKey();
                entity.Sort = sort + 1;
                entity.CreateByOperator();
                repository.Insert(entity);
            }
        }

        public void UpdateAction(SystemActionDto action)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemAction>>())
            {
                var entity = repository.FindEntity(action.Id);
                entity.Name = action.Name;
                entity.Url = action.Url;
                entity.ModuleId = action.ModuleId;
                entity.Disabled = action.Disabled;
                entity.Displayed = action.Displayed;
                entity.ModifyByOperator();
                repository.Update(entity);
            }
        }

        public void DeleteActionById(Guid id)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemAction>>())
            {
                var entity = repository.FindEntity(id);
                if (entity.IsNullOrDeleted())
                {
                    throw new KnownException("资源已被删除");
                }
                entity.RemoveByOperator();
                repository.Update(entity);
            }
        }

        public List<SystemActionDto> GetActionListByRoleId(Guid roleId)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemAction>>())
            {
                return repository.IQueryable().WhereNotDelete()
                    .WhereRoleId(roleId)
                    .ToDtos()
                    .ToList();
            }
        }

        public void EmpowermentToRole(Guid roleId, List<Guid> moduleIds, List<Guid> actionIds)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase>().BeginTrans())
            {
                var role = repository.FindEntity<SystemRole>(roleId);
                if (role.IsNullOrDeleted())
                {
                    throw new KnownException("角色不存在");
                }
                role.SystemActionRoles.ToList().ForEach(x => repository.Delete(x));
                role.SystemModuleRoles.ToList().ForEach(x => repository.Delete(x));
                role.AttachModules(moduleIds);
                role.AttachActions(actionIds);
                repository.Commit();
            }
        }
    }
}
