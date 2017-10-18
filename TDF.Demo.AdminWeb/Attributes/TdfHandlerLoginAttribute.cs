using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDF.Core.Configuration;
using TDF.Core.Ioc;
using TDF.Core.Operator;
using TDF.Web.Authentication.Attributes.Mvc;

namespace TDF.Demo.AdminWeb.Attributes
{
    public class TdfHandlerLoginAttribute: HandlerLoginAttribute
    {
        public TdfHandlerLoginAttribute(bool ignore = false) : base(ignore)
        {

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
                    var goUrl = string.Empty;
                    var goUri = filterContext.HttpContext.Request.Url;
                    if (goUri != null)
                    {
                        List<string> urlList = new List<string>() { "/login", "/login/index" };
                        if (!urlList.Contains(goUri.AbsolutePath.ToLower()))// 登录页本身不记录
                            goUrl = System.Web.HttpUtility.UrlEncode(goUri.AbsoluteUri, System.Text.Encoding.UTF8);
                    }
                    if (goUrl != string.Empty)
                        goUrl = "?go=" + goUrl;
                    filterContext.Result = new RedirectResult(Configs.Instance.GetValue("LoginUrl") + goUrl);
                }
            }
        }
    }
}