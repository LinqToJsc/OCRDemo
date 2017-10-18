using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TDF.Web.Extensions;

namespace TDF.Web.Attributes.WebApi
{
    /// <summary>
    /// WebApi基础数据验证
    /// </summary>
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.ModelState.Validate();
            base.OnActionExecuting(actionContext);
        }
    }
}
