using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TDF.Demo.AdminWeb.Controllers
{
    public class DemoController : AdminControllerBase
    {
       
        public override ActionResult Index()
        {
            return View();
        }

        public  ActionResult IndexA()
        {
            return View();
        }
        public ActionResult IndexB()
        {
            return View();
        }
    }
}