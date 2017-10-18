using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDF.Core.Ioc;
using TDF.Core.Operator;
using TDF.Web.Authentication.Services;

namespace TDF.Web.Authentication.Attributes.Mvc
{
    /// <summary>
    /// 权限过滤器
    /// </summary>
    public class HandlerAuthorizeAttribute : ActionFilterAttribute
    {
        public bool Ignore { get; set; }

        public HandlerAuthorizeAttribute(bool ignore = false)
        {
            Ignore = ignore;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any() ||
                filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any())
            {
                return;
            }
            if (Ioc.Resolve<IOperatorProvider>().GetCurrent().IsSystem || Ignore)
            {
                return;
            }
            if (ActionAuthorize(filterContext)) return;
            UnauthorizedHandle(filterContext);
        }

        /// <summary>
        /// 权限不足时的处理方法
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual void UnauthorizedHandle(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new Web.Models.JsonResult()
                {
                    Success = false,
                    Message = "很抱歉！您的权限不足，访问被拒绝！"
                };
                return;
            }
            filterContext.Result = new ContentResult()
            {
                Content = "<script type='text/javascript'>alert('很抱歉！您的权限不足，访问被拒绝！');</script>"
            };
        }

        private bool ActionAuthorize(ActionExecutingContext filterContext)
        {
            var operatorProvider = Ioc.Resolve<IOperatorProvider>().GetCurrent();
            var roleIds = operatorProvider.RoleIds;
            var action = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
            return ActionAuthorize(operatorProvider.Id, roleIds, action);
        }

        /// <summary>
        /// 验证是否有权限访问
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="roleIds"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected virtual bool ActionAuthorize(Guid memberId, List<Guid> roleIds, string action)
        {
            return Ioc.Resolve<IRoleAuthorizeService>().ActionValidate(memberId, roleIds, action);
        }
    }
}
