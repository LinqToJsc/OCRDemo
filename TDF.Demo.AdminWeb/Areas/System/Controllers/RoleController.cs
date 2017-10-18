using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDF.Core.Ioc;
using TDF.Demo.AdminWeb.Attributes;
using TDF.Demo.AdminWeb.Controllers;
using TDF.Demo.AdminWeb.Models;
using TDF.Demo.Service.Dtos.SystemManage;
using TDF.Demo.Service.SystemManage;
using TDF.Web.Attributes.Mvc;

namespace TDF.Demo.AdminWeb.Areas.System.Controllers
{
    public class RoleController : AdminControllerBase
    {
        // GET: System/Role
        public override ActionResult Index()
        {
            return View();
        }

        public ActionResult Demo()
        {
            return View();
        }

        [TdfHandlerAuthorize(true)]
        public ActionResult GetRolePagedList(RoleCriteria criteria)
        {
            var dataTable = Ioc.Resolve<ISystemMemberService>().GetRolePagedList(criteria)
                .ToJqueryDataTableModel();
            dataTable.draw = criteria.draw;
            return dataTable;
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit(Guid roleId)
        {
            var role = Ioc.Resolve<ISystemMemberService>().GetRoleById(roleId);
            return View(role);
        }

        [HttpPost]
        [ModelValidation]
        [TdfHandlerAuthorize(true)]
        public ActionResult SaveRole(SystemRoleDto model)
        {
            if (model.Id == null)
            {
                Ioc.Resolve<ISystemMemberService>().AddRole(model);
            }
            else
            {
                Ioc.Resolve<ISystemMemberService>().UpdateRole(model);
            }
            return Success();
        }

        /// <summary>
        /// 禁用角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public ActionResult Enable(Guid roleId, bool enabled)
        {
            Ioc.Resolve<ISystemMemberService>().EnabledRole(roleId, enabled);
            return Success();
        }

        /// <summary>
        /// 获得菜单Id集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [TdfHandlerAuthorize(true)]
        public ActionResult GetMenuIds(Guid roleId)
        {
            var actionIds = Ioc.Resolve<ISystemModuleService>().GetActionListByRoleId(roleId)
                .Select(x => x.Id).ToList();
            return Success(actionIds);
        }

        /// <summary>
        /// 给角色赋权
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="moduleIds"></param>
        /// <param name="actionIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Empowerment(Guid roleId, List<Guid> moduleIds, List<Guid> actionIds)
        {
            moduleIds = moduleIds ?? new List<Guid>();
            actionIds = actionIds ?? new List<Guid>();
            Ioc.Resolve<ISystemModuleService>().EmpowermentToRole(roleId, moduleIds, actionIds);
            return Success();
        }

        [HttpPost]
        public ActionResult Delete(Guid roleId)
        {
            Ioc.Resolve<ISystemMemberService>().DeleteRole(roleId);
            return Success();
        }
    }
}