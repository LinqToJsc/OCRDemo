using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using TDF.Core.Ioc;
using TDF.Core.Log;
using TDF.Core.Operator;
using TDF.Demo.AdminWeb.Attributes;
using TDF.Web.Authentication.Attributes.Mvc;

namespace TDF.Demo.AdminWeb.Controllers
{
    [TdfHandlerLogin]
    [TdfHandlerAuthorize]
    public abstract class AdminControllerBase : Controller
    {
        private OperatorModel _currentOperator;
        public ILog Log => LogFactory.GetLogger(GetType());

        /// <summary>
        /// 当前的登录用户ff   
        /// </summary>
        protected OperatorModel CurrentOperator => _currentOperator ?? (_currentOperator = Ioc.Resolve<IOperatorProvider>().GetCurrent());

        /// <summary>
        /// 区域Id
        /// </summary>
        protected Guid AreaId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var areadIdStr = string.Empty;
            //var cookie = filterContext.RequestContext.HttpContext.Request.Cookies.Get("AreaId");
            //if (cookie != null)
            //{
            //    areadIdStr = cookie.Value;
            //}

            //if (string.IsNullOrEmpty(areadIdStr))
            //{
            //    areadIdStr = filterContext.RequestContext.HttpContext.Request.Headers.Get("AreaId");
            //}
            //Guid tempId;
            //if (Guid.TryParse(areadIdStr, out tempId))
            //{
            //    AreaId = tempId;
            //}
            base.OnActionExecuting(filterContext);
        }

        [TdfHandlerAuthorize(Ignore = false)]
        public virtual ActionResult Index()
        {
            return View(); 
        }

        [NonAction]
        protected virtual ActionResult Success(string message = "")
        {
            var result = new TDF.Web.Models.JsonResult();
            result.Message = message;
            result.Succeed();
            return result;
        }

        [NonAction]
        protected virtual ActionResult Fail(string message = "")
        {
            var result = new TDF.Web.Models.JsonResult();
            result.Message = message;
            result.Fail();
            return result;
        }

        [NonAction]
        protected virtual ActionResult Success(object data, string message = "")
        {
            var result = new TDF.Web.Models.JsonResult<object>();
            result.Value = data;
            result.Message = message;
            result.Succeed();
            return result;
        }

        [NonAction]
        protected virtual ActionResult Error(string message)
        {
            var result = new TDF.Web.Models.JsonResult();
            result.Message = message;
            result.Succeed();
            return result;
        }
    }
}