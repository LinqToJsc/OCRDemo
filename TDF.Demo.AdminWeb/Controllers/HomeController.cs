using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TDF.Demo.AdminWeb.Controllers
{
    public class HomeController : AdminControllerBase
    {
        // GET: Home
        public override ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}