using System.Web.Mvc;
using TDF.Core.Exceptions;
using TDF.Core.Log;

namespace TDF.Web.Attributes.Mvc
{
    /// <summary>
    /// MVC异常拦截器
    /// </summary>
    public class HandlerErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is KnownException)
            {
                base.OnException(context);
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = 200;
                var result = new Models.JsonResult();
                result.Fail(context.Exception.Message);
                context.Result = result;
            }
            else
            {
                var message = context.RequestContext.HttpContext.Request.IsLocal
                    ? context.Exception.GetAllMessages()
                    : "服务器未知错误，请重试。如果该问题一直存在，请联系管理员。感谢您的支持。";
                base.OnException(context);
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = 200;
                var result = new Models.JsonResult();
                result.Fail(message);
                WriteLog(context);
                context.Result = result;
            }

        }

        private void WriteLog(ExceptionContext context)
        {
            if (context == null)
                return;
            var log = LogFactory.GetLogger(context.Controller.ToString());
            log.Error(context.Exception);
        }
    }
}
