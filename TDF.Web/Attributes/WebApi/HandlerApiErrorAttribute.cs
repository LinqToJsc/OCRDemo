using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;
using TDF.Core.Exceptions;
using TDF.Core.Log;
using TDF.Core.Models;

namespace TDF.Web.Attributes.WebApi
{
    /// <summary>
    /// WebApi异常拦截器
    /// </summary>
    public class HandlerApiErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var knownException = actionExecutedContext.Exception as KnownException;
            if (knownException != null)
            {
                actionExecutedContext.Response = new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new ObjectContent<ApiResult>(new ApiResult { Success = false, Message = knownException.Message,Code = knownException.ErrorCode }, new JsonMediaTypeFormatter())
                };
                base.OnException(actionExecutedContext);
            }
            else
            {
                var message = actionExecutedContext.Exception.GetAllMessages();
                actionExecutedContext.Response = new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Content = new ObjectContent<ApiResult>(new ApiResult { Success = false, Message = message,Code = -1}, new JsonMediaTypeFormatter())
                };
                WriteLog(actionExecutedContext);
            }
        }

        private void WriteLog(HttpActionExecutedContext context)
        {
            if (context.Exception == null)
                return;
            var log = LogFactory.GetLogger(context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName);
            log.Error(context.Exception);
        }
    }
}
