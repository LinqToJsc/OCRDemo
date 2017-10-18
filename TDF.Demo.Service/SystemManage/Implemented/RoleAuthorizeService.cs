using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Caching;
using TDF.Core.Exceptions;
using TDF.Core.Ioc;
using TDF.Data.EntityFramework.Repository;
using TDF.Data.Repository;
using TDF.Demo.Domain.Entities;
using TDF.Demo.Domain.Entities.Extensions;
using TDF.Demo.Domain.Entities.Partial.SystemManage.Extensions;
using TDF.Demo.Domain.Infrastructure;
using TDF.Demo.Service.Consumer;
using TDF.Demo.Service.Dtos.SystemManage.Role;
using TDF.Web.Authentication.Services;

namespace TDF.Demo.Service.SystemManage.Implemented
{
    public class RoleAuthorizeService: IRoleAuthorizeService
    {
        public bool ActionValidate(Guid memberId, List<Guid> roleIds, string action)
        {
            var authorizeData = Ioc.Resolve<ICacheProvider>()
                .Get(string.Format(AuthorizeCacheEventConsumer.AUTHORIZE_KEY, memberId), () =>
                {
                    using (var repository = Ioc.Resolve<IRepositoryBase>())
                    {
                        //还需要判断资源所属的 菜单是否没禁用
                        var authorizeActionList = repository.IQueryable<SystemAction>(x => x.SystemActionRoles.Any(y => roleIds.Contains(y.RoleId)))
                            .WhereNotDelete()
                            .Select(x => new AuthorizeActionModel()
                            {
                                Url = x.Url,
                                Id = x.ModuleId
                            }).ToList();
                        //找出被禁用的菜单 然后从权限资源中过滤掉
                        var disabledModuleIds = repository.IQueryable<SystemModule>().WhereByEnabled(false).Select(x => x.Id).ToList();
                        if (authorizeActionList.Count > 0 && disabledModuleIds.Count > 0)
                        {
                            authorizeActionList = authorizeActionList.Where(x => !disabledModuleIds.Contains(x.Id)).ToList();
                        }
                        return authorizeActionList;
                    }
                });
            action = action.ToLower().TrimEnd('/');
            return authorizeData.Any(x => x.Url.ToLower().Split(',').Select(y => y.TrimEnd('/')).Contains(action));
        }

        public void Assignment(Guid memberId, List<Guid> roleIds)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase>().BeginTrans())
            {
                var member = repository.FindEntity<SystemMember>(memberId);
                if (member.IsNullOrDeleted())
                {
                    throw new KnownException("系统用户已删除");
                }
                member.SystemMemberRoles.ToList().ForEach(x => repository.Delete(x));
                if (roleIds != null && roleIds.Count > 0)
                    member.AssignmentRole(roleIds);
                repository.Commit();
            }
        }
    }
}
