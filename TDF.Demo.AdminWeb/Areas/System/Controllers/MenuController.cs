using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDF.Core.Ioc;
using TDF.Core.Json;
using TDF.Core.Operator;
using TDF.Demo.AdminWeb.Areas.System.Models;
using TDF.Demo.AdminWeb.Attributes;
using TDF.Demo.AdminWeb.Controllers;
using TDF.Demo.AdminWeb.Models;
using TDF.Demo.Domain.Entities.Partial.SystemManage.Enums;
using TDF.Demo.Service.Dtos.SystemManage;
using TDF.Demo.Service.SystemManage;
using TDF.Web.Attributes.Mvc;

namespace TDF.Demo.AdminWeb.Areas.System.Controllers
{
    public class MenuController : AdminControllerBase
    {
        public override ActionResult Index()
        {
            var result = Ioc.Resolve<ISystemModuleService>().GetModuleList()
                .Where(x => x.ParentId == Guid.Empty)//只需要父级菜单
                .ToList();
            return View(result);
        }

        /// <summary>
        /// 设置是否启用模块
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Enable(Guid moduleId, bool enabled)
        {
            Ioc.Resolve<ISystemModuleService>().EnableModule(moduleId, enabled);
            return Success();
        }

        /// <summary>
        /// 资源列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Resources()
        {
            var result = Ioc.Resolve<ISystemModuleService>().GetModuleList()
                .Where(x => x.ParentId != Guid.Empty)//只需要父级菜单
                .ToList();
            return View(result);
        }

        [TdfHandlerAuthorize(true)]
        public ActionResult GetResourcesPagedList()
        {
            return Ioc.Resolve<ISystemModuleService>().GetActionTableList().ToJqueryDataTableModel(0);
        }

        public ActionResult GetResourcesPagesByCommbox()
        {
            var data = Ioc.Resolve<ISystemModuleService>().GetActionByActionType(SystemActionType.Page);
            return Success(data);
        }

        /// <summary>
        /// 保存模块信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelValidation]
        public ActionResult SaveModule(SystemModuleDto model)
        {
            if (model.Id == null)
            {
                Ioc.Resolve<ISystemModuleService>().AddMoudule(model);
            }
            else
            {
                Ioc.Resolve<ISystemModuleService>().UpdateModule(model);
            }
            return Success();
        }

        [HttpPost]
        [ModelValidation]
        public ActionResult SaveAction(SystemActionDto model)
        {
            if (model.ActionType == SystemActionType.Function)
            {
                var ids = model.ActionPageCommbox.Split(',');
                model.ActionParentId = new Guid(ids[0]);
                model.ModuleId = new Guid(ids[1]);
            }
            
            if (model.Id == null)
            {
                Ioc.Resolve<ISystemModuleService>().AddAction(model);
            }
            else
            {
                Ioc.Resolve<ISystemModuleService>().UpdateAction(model);
            }

            return Success();
        }

        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAction(Guid id)
        {
            Ioc.Resolve<ISystemModuleService>().DeleteActionById(id);
            return Success();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [TdfHandlerAuthorize(true)]
        public ActionResult GetModulePagedList(ModuleCriteria criteria)
        {
            return Ioc.Resolve<ISystemModuleService>().GetModulePagedList(criteria)
                .ToJqueryDataTableModel();
        }

        [TdfHandlerAuthorize(true)]
        public ActionResult GetMenusTree()
        {
            var oper = Ioc.Resolve<IOperatorProvider>().GetCurrent<OperatorModel>();
            List<SystemModuleDto> modules;
            if (oper.IsSystem)
            {
                modules = Ioc.Resolve<ISystemModuleService>().GetModuleIncludeActionList();
            }
            else
            {
                modules = Ioc.Resolve<ISystemModuleService>().GetModuleIncludeActionList(oper.Id, oper.RoleIds);
            }
            FixMenuUrl(modules);
            return Success(modules.ToMenuJsTrees());
        }


        /// <summary>
        /// 将资源类型为“显示到菜单”的Url中取第一项Url为展示菜单地址
        /// </summary>
        /// <param name="modules"></param>
        private void FixMenuUrl(List<SystemModuleDto> modules)
        {
            modules.ForEach(x => x.SystemActionDtos.Where(y => y.Displayed).ToList().ForEach(y =>
            {
                if (y.Url.Contains(","))
                {
                    y.Url = y.Url.Split(',')[0];
                }
            }));
        }
    }
}