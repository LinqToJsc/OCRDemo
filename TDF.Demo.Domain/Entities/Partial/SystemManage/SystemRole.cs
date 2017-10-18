using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities.Extensions;

namespace TDF.Demo.Domain.Entities
{
    public partial class SystemRole
    {
        /// <summary>
        /// 附加菜单列表
        /// </summary>
        /// <param name="moduleIds"></param>
        public void AttachModules(List<Guid> moduleIds)
        {
            //var duplicateIds =
            //    SystemModuleRoles.Where(x => moduleIds.Contains(x.ModuleId)).Select(x => x.ModuleId).ToList();
            //var ids = moduleIds.Where(x => !duplicateIds.Contains(x)).ToList(); //过滤掉已添加的菜单ID

            foreach (var moduleId in moduleIds)
            {
                var moduleRole = new SystemModuleRole()
                {
                    ModuleId = moduleId,
                    RoleId = Id
                };
                moduleRole.CreateByOperator();
                SystemModuleRoles.Add(moduleRole);
            }
        }

        /// <summary>
        /// 附加资源列表
        /// </summary>
        /// <param name="actionIds"></param>
        public void AttachActions(List<Guid> actionIds)
        {
            //var duplicateIds =
            //    SystemActionRoles.Where(x => actionIds.Contains(x.ActionId)).Select(x => x.ActionId).ToList();
            //var ids = actionIds.Where(x => !duplicateIds.Contains(x)).ToList(); //过滤掉已添加的资源ID
            foreach (var actionId in actionIds)
            {
                var moduleRole = new SystemActionRole()
                {
                    ActionId = actionId,
                    RoleId = Id
                };
                moduleRole.CreateByOperator();
                SystemActionRoles.Add(moduleRole);
            }
        }
    }
}
