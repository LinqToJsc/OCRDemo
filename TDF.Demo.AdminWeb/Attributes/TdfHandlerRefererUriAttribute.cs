using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TDF.Web.Authentication.Attributes.Mvc;

namespace TDF.Demo.AdminWeb.Attributes
{
    public class TdfHandlerRefererUriAttribute : HandlerAuthorizeAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var refUri = filterContext.HttpContext.Request.UrlReferrer;
            var refUrldDmaiName = "";
            if (refUri == null)
                return;

            refUrldDmaiName = refUri.DnsSafeHost;
            if (!CheckRefUrlDmaiName(refUrldDmaiName))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new TDF.Web.Models.JsonResult()
                    {
                        Success=false,
                        Message = "非法的域名请求，访问被拒绝！"
                    };
                    return;
                }
                var sbScript = new StringBuilder();
                sbScript.Append("<script type='text/javascript'>alert('非法的域名请求，访问被拒绝！');history.go(-1);</script>");
                filterContext.Result = new ContentResult() { Content = sbScript.ToString() };
            }
        }

        /// <summary>
        /// 验证请求域名合法性
        /// </summary>
        /// <param name="refUrldDmaiName"></param>
        /// <returns></returns>
        public bool CheckRefUrlDmaiName(string refUrldDmaiName)
        {
            if (string.IsNullOrEmpty(refUrldDmaiName))
                return true;
            refUrldDmaiName = refUrldDmaiName.ToLower();
            if (RefererUriWhiteListConfig.RefererUriWhiteList.Contains(refUrldDmaiName))
                return true;
            return false;
        }
    }
}