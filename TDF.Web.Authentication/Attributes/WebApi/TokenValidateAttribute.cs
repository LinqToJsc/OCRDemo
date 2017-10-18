using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TDF.Core.Ioc;
using TDF.Core.Models;
using TDF.Web.Authentication.Models;
using TDF.Web.Authentication.Services;

namespace TDF.Web.Authentication.Attributes.WebApi
{
    /// <summary>
    /// WebApiToken拦截处理器
    /// </summary>
    public class TokenValidateAttribute : AuthorizationFilterAttribute
    {
        public const string TokenKeyName = "TokenKey";
        public const string LogonUserName = "LogonUser";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }
            if (actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }
            var qs = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
            var tokenKey = qs[TokenKeyName];
            if (string.IsNullOrEmpty(tokenKey))
            {
                var authorization = actionContext.Request.Headers.Authorization;
                if (authorization != null)
                {
                    tokenKey = authorization.Scheme;
                }
                if (string.IsNullOrEmpty(tokenKey))
                {
                    IEnumerable<string> list = null;
                    if (actionContext.Request.Headers.TryGetValues(TokenKeyName, out list))
                    {
                        tokenKey = list.FirstOrDefault();
                    }
                }
            }
            if (string.IsNullOrEmpty(tokenKey))
            {
                actionContext.Response = new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.Unauthorized,
                    Content = new ObjectContent<ApiResult>(new ApiResult { Success = false, Message = "Token无效",Code = 1001 }, new JsonMediaTypeFormatter())
                };
                return;
            }
            HttpContext.Current.Items.Add(TokenKeyName, tokenKey);
            var user = Ioc.Resolve<IIdentityProvider>().GetCurrent();
            if (user == null || user.ExpiredTime < DateTime.Now)
            {
                actionContext.Response = new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.Unauthorized,
                    Content = new ObjectContent<ApiResult>(new ApiResult { Success = false, Message = "Token已过期",Code = 1002 }, new JsonMediaTypeFormatter())
                };
                return;
            }
            var logonUser = new UserIdentity(user);
            actionContext.ControllerContext.RouteData.Values[LogonUserName] = logonUser;
            SetPrincipal(new UserPrincipal(logonUser));
        }

        private void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
    }
}
