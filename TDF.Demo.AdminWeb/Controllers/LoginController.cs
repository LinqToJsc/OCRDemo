using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDF.Core.Caching;
using TDF.Core.Configuration;
using TDF.Core.Tools;
using TDF.Demo.AdminWeb.Attributes;
using TDF.Demo.Service;
using TDF.Demo.Service.SystemManage;
using TDF.Web.Attributes.Mvc;
using TDF.Web.Authentication.Attributes.Mvc;
using TDF.Web.Authentication.Models;
using TDF.Web.Infrastructure;

namespace TDF.Demo.AdminWeb.Controllers
{
    /// <summary>
    /// 登录相关
    /// </summary>
    [AllowAnonymous]
    public class LoginController : AdminControllerBase
    {

        /// <summary>
        /// 管理员服务
        /// </summary>
        protected readonly ISystemMemberService SystemService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemService"></param>
        public LoginController(ISystemMemberService systemService)
        {
            SystemService = systemService;
        }

        public override ActionResult Index()
        {
            string go = Request.QueryString["go"];
            ViewBag.GoUrl = "";
            if (!string.IsNullOrEmpty(go))
                ViewBag.GoUrl = go;
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelValidation]
        public ActionResult Login(LoginModel loginModel)
        {
            SystemService.Login(loginModel.UserName.Trim(), loginModel.Password);
            return Success();
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult LoginOut()
        {
            SystemService.LoginOut();
            return Success();
        }

        /// <summary>
        /// 获得验证码图片
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [TdfHandlerRefererUri]
        public ActionResult GetAuthCode()
        {
            string code;
            var imageByte = new VerifyCode().GetVerifyCode(out code);
            var verifykey = Configs.Instance.Verifykey + "_" + Guid.NewGuid();
            WebHelper.WriteCookie(Configs.Instance.Verifykey, verifykey);
            CacheManager.Set(verifykey, MD5Helper.GetMD5(code, 16), 10);
            return File(imageByte, @"image/Gif");
        }
    }
}