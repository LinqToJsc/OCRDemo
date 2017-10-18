using System.Web.Mvc;
using TDF.Web.Extensions;

namespace TDF.Web.Attributes.Mvc
{
    /// <summary>
    /// MVC基础数据验证
    /// </summary>
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewData.ModelState.Validate();
            base.OnActionExecuting(filterContext);
        }
    }
}
