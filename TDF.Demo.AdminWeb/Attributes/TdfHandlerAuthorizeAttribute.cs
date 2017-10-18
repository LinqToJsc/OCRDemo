using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TDF.Core.Ioc;
using TDF.Core.Operator;
using TDF.Web.Authentication.Attributes.Mvc;

namespace TDF.Demo.AdminWeb.Attributes
{
    public class TdfHandlerAuthorizeAttribute : HandlerAuthorizeAttribute
    {
        public TdfHandlerAuthorizeAttribute(bool ignore = false) : base(ignore)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any() ||
                filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any())
            {
                return;
            }
            if (Ioc.Resolve<IOperatorProvider>().GetCurrent<OperatorModel>().IsSystem || Ignore)
            {
                return;
            }
            if (!this.ActionAuthorize(filterContext))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new TDF.Web.Models.JsonResult()
                    {
                        Success = false,
                        Message = "很抱歉！您的权限不足，访问被拒绝！"
                    };
                    return;
                }
                var sbScript = new StringBuilder();
                sbScript.Append("<script type='text/javascript'>alert('很抱歉！您的权限不足，访问被拒绝！');history.go(-1);</script>");
                filterContext.Result = new ContentResult() { Content = sbScript.ToString() };
            }
        }

        private bool ActionAuthorize(ActionExecutingContext filterContext)
        {
            var operatorProvider = Ioc.Resolve<IOperatorProvider>().GetCurrent<OperatorModel>();
            var roleIds = operatorProvider.RoleIds;
            var action = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];

            //LogFactory.GetLogger(GetType()).Debug("后台资源权限验证=>SCRIPT_NAME=" + action+ "----ServerVariablesAllKeys=>"+ HttpContext.Current.Request.ServerVariables.AllKeys.ToJson());
            return ActionAuthorize(operatorProvider.Id, roleIds, action);
        }
    }
}