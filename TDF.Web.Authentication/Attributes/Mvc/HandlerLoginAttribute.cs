using System.Linq;
using System.Web.Mvc;
using TDF.Core.Configuration;
using TDF.Core.Ioc;
using TDF.Core.Operator;

namespace TDF.Web.Authentication.Attributes.Mvc
{
    /// <summary>
    /// 登录过滤器
    /// </summary>
    public class HandlerLoginAttribute : AuthorizeAttribute
    {
        public bool Ignore;

        public HandlerLoginAttribute(bool ignore = false)
        {
            Ignore = ignore;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any() ||
               filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any())
            {
                return;
            }
            if (Ignore)
            {
                return;
            }
            if (Ioc.Resolve<IOperatorProvider>().GetCurrent() == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
                {
                    var loginUrl = Configs.Instance.GetValue("LoginUrl");
                    filterContext.Result = new RedirectResult(string.IsNullOrEmpty(loginUrl) ? "~/login" : loginUrl);
                }
            }
        }
    }
}
