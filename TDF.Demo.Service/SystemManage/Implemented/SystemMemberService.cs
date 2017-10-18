using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using TDF.Core.Configuration;
using TDF.Core.Exceptions;
using TDF.Core.Ioc;
using TDF.Core.Models;
using TDF.Core.Operator;
using TDF.Data.EntityFramework.Repository;
using TDF.Data.Repository;
using TDF.Demo.Domain.Entities;
using TDF.Demo.Domain.Entities.Extensions;
using TDF.Demo.Domain.Entities.Partial.SystemManage.Extensions;
using TDF.Demo.Domain.Infrastructure;
using TDF.Demo.Service.Dtos.SystemManage;

namespace TDF.Demo.Service.SystemManage
{
    public class SystemMemberService : ISystemMemberService
    {
        public OperatorModel Login(string userName, string password)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemMember>>())
            {
                var systemMember = repository.FindEntity(x => x.Account == userName);
                if (systemMember.IsNullOrDeleted())
                {
                    throw new KnownException("账户不存在，请重新输入");
                }

                systemMember.CheckStatus();
                systemMember.CheckPassword(password);
                var oper = BuildOperator(systemMember);
                systemMember.Login(oper.LoginToken);
                repository.Update(systemMember);
                
                Ioc.Resolve<IOperatorProvider>().AddCurrent(oper);
                return oper;
            }
        }

        public void LoginOut()
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemMember>>())
            {
                var oper = Ioc.Resolve<IOperatorProvider>().GetCurrent<OperatorModel>();
                var systemMember =
                    repository.FindEntity(x => x.Account == oper.UserName);
                if (systemMember.IsNullOrDeleted())
                {
                    throw new KnownException("用户不存在");
                }
                systemMember.LoginOut();
                repository.Update(systemMember);
            }
            Ioc.Resolve<IOperatorProvider>().RemoveCurrent();
        }

        public IPagedList<SystemRoleDto> GetRolePagedList(RoleCriteria criteria)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemRole>>())
            {
                return repository.IQueryable().WhereNotDelete()
                    .WhereEnabled(!criteria.Disabled)
                    .OrderByDescending(x => x.CreatedTime)
                    .ToDtos()
                    .ToPageResult(criteria.PageIndex, criteria.PageSize);
            }
        }

        public void AddRole(SystemRoleDto role)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemRole>>())
            {

                if (repository.IQueryable().Any(x => x.Name == role.Name))
                {
                    throw new KnownException("角色=>" + role.Name + " 已存在");
                }
                var entity = AutoMapper.Mapper.Map<SystemRole>(role);
                entity.CreateByOperator();
                repository.Insert(entity);
            }
        }

        public void UpdateRole(SystemRoleDto role)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemRole>>())
            {
                var entity = repository.FindEntity(role.Id);
                if (entity.IsNullOrDeleted())
                {
                    throw new KnownException("角色未找到");
                }

                if (repository.IQueryable().Any(x => x.Name == role.Name && x.Id != entity.Id))
                    throw new KnownException("角色=>" + role.Name + " 已存在");

                entity.Name = role.Name;
                entity.Desc = role.Desc;
                entity.ModifyByOperator();
                repository.Update(entity);
            }
        }

        public SystemRoleDto GetRoleById(Guid roleId)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemRole>>())
            {
                var entity = repository.FindEntity(roleId);
                return AutoMapper.Mapper.Map<SystemRoleDto>(entity);
            }
        }

        public void EnabledRole(Guid roleId, bool enabled)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemRole>>())
            {
                var role = repository.FindEntity(roleId);
                if (role.Disabled != enabled)
                {
                    return;
                }

                role.Disabled = !enabled;
                role.ModifyByOperator();
                repository.Update(role);
            }
        }

        public void DeleteRole(Guid roleId)
        {
            //review JSC
            using (var repository = Ioc.Resolve<IRepositoryBase>().BeginTrans())
            {
                var role = repository.FindEntity<SystemRole>(roleId);
                if (role.IsNullOrDeleted())
                {
                    throw new KnownException("角色已删除");
                }
                if (!role.Disabled)
                {
                    throw new KnownException("请先禁用此角色");
                }
                //验证当前登录管理员对 Entity的数据操作权限 抛出 KnownException Entity的数据新增是必须有CreatorId，因此无需对CreatorId进行非空验证
                //ManagerLevelService.CheckOperAuthorityByManagerLv(role.CreatorId.Value);

                Expression<Func<SystemActionRole, bool>> actionRoleExpression = actionRole => actionRole.RoleId == role.Id;
                Expression<Func<SystemModuleRole, bool>> moduleRoleExpression = moduleRole => moduleRole.RoleId == role.Id;
                Expression<Func<SystemMemberRole, bool>> userRoleExpression = userRole => userRole.SystemRoleId == role.Id;
                repository.Delete<SystemActionRole>(actionRoleExpression);
                repository.Delete<SystemModuleRole>(moduleRoleExpression);
                repository.Delete<SystemMemberRole>(userRoleExpression);

                // 假删除才有必要执行这一句
                //role.RemoveByOperator();
                repository.Delete(role);

                repository.Commit();
            }
        }

        public void AddMember(SystemMemberDto systemMember)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemMember>>())
            {
                if (repository.IQueryable(x => x.Account == systemMember.Account).Any())
                {
                    throw new KnownException("登录名已存在");
                }
                var entity = AutoMapper.Mapper.Map<SystemMember>(systemMember);
                entity.SetPassword(systemMember.Password);
                entity.CreateByOperator();
                repository.Insert(entity);
            }
        }

        public IPagedList<SystemMemberDto> GetMemberPagedList(MemberCriteria criteria)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemMember>>())
            {
                return repository.IQueryable().WhereNotDelete()
                    .WhereByKey(criteria.Keyword)
                    .WhereByEnabled(criteria.Enabled)
                    .WhereByCreatedId()
                    .OrderBy(x => x.CreatedTime)
                    .ToDtos()
                    .ToPageResult(criteria.PageIndex, criteria.PageSize);
            }
        }

        public void EnableMember(Guid memberId, bool enabled)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemMember>>())
            {
                var entity = repository.FindEntity(memberId);
                if (entity.IsSuperAdmin)
                {
                    throw new KnownException("不能对超级管理员进行操作");
                }
                //验证当前登录管理员对 Entity的数据操作权限 抛出 KnownException
                //ManagerLevelService.CheckOperAuthorityByManagerLv(entity.CreatorId.Value);

                if (entity.EnabledMark != null && entity.EnabledMark.Value == enabled) return;
                entity.EnabledMark = enabled;
                entity.ModifyByOperator();
                repository.Update(entity);
            }
        }

        public SystemMemberDto GetMemberById(Guid memberId)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemMember>>())
            {
                var entity = repository.FindEntity(memberId);
                if (entity.IsNullOrDeleted())
                {
                    throw new KnownException("成员不存在");
                }
                return AutoMapper.Mapper.Map<SystemMemberDto>(entity);
            }
        }

        public void UpdateMember(SystemMemberDto model)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemMember>>())
            {
                var entity = repository.FindEntity(model.Id);
                if (entity.IsNullOrDeleted())
                {
                    throw new KnownException("成员不存在");
                }
                if (entity.IsSuperAdmin &&
                    entity.Id != Ioc.Resolve<IOperatorProvider>().GetCurrent<OperatorModel>().Id)
                {
                    throw new KnownException("只允许超级管理员修改超级管理员自己的账户信息");
                }
                //验证当前登录管理员对 Entity的数据操作权限 抛出 KnownException
                //ManagerLevelService.CheckOperAuthorityByManagerLv(entity.CreatorId.Value);
                
                entity.Email = model.Email;
                entity.EnabledMark = model.EnabledMark;
                entity.Gender = model.Gender;
                entity.RealName = model.RealName;
                entity.MobilePhone = model.MobilePhone;
                if (!string.IsNullOrEmpty(model.Password))
                {
                    entity.SetPassword(model.Password);
                }
                entity.ModifyByOperator();
                repository.Update(entity);
            }
        }

        public void UpdatePassWord(PassWordDto model)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemMember>>())
            {
                var entity = repository.FindEntity(model.Id);
                if (entity.IsNullOrDeleted())
                {
                    throw new KnownException("成员不存在");
                }

                entity.CheckPassword(model.OldPassword);
                entity.SetPassword(model.Password);

                entity.ModifyByOperator();
                repository.Update(entity);
            }
        }

        public void DeleteMember(Guid memberId, bool isLogic = true)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemMember>>())
            {
                var entity = repository.FindEntity(memberId);
                if (entity.IsNullOrDeleted())
                {
                    throw new KnownException("成员不存在");
                }

                //验证当前登录管理员对 Entity的数据操作权限 抛出 KnownException
                //ManagerLevelService.CheckOperAuthorityByManagerLv(entity.CreatorId.Value);

                if (isLogic)
                {
                    entity.RemoveByOperator();
                    repository.Update(entity);
                }
                else
                {
                    repository.Delete(entity);
                }
            }
        }

        public List<SystemRoleDto> GetRolesByMemberId(Guid memberId)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemRole>>())
            {
                return repository.IQueryable().WhereNotDelete()
                    .WhereByMemberId(memberId)
                    .WhereEnabled(true) //过滤掉被禁用的角色
                    .ProjectTo<SystemRoleDto>()
                    .ToList();
            }
        }

        public List<SystemRoleDto> GetAllRoles(bool? enabled = null)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<SystemRole>>())
            {
                return repository.IQueryable().WhereNotDelete()
                    .WhereEnabled(enabled)
                    .ProjectTo<SystemRoleDto>()
                    .ToList();
            }
        }

        private OperatorModel BuildOperator(SystemMember member)
        {
            return new OperatorModel()
            {
                Id = member.Id,
                ExpiredTime = DateTime.Now.AddMinutes(Configs.Instance.SessionExpireMinute),
                IsSystem = member.IsSuperAdmin,
                LoginTime = DateTime.Now,
                UserName = member.Account,
                //过滤掉 被禁用的角色
                RoleIds = member.SystemMemberRoles.Where(x => x.SystemRole.Disabled == false).Select(x => x.SystemRoleId).ToList(),
            };
        }
    }
}
