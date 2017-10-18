using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TDF.Module.Images.Mvc
{
    public class ImagesAreaRegistration : AreaRegistration
    {
        public const string Name = "Images";

        public override string AreaName => Name;

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var ns = new[] { "TDF.Module.Images.Mvc.Controllers.*" };
            context.MapRoute(
               "ImagesDefault",
               "module/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional }
           );
            context.MapRoute("imageRoute", "img/{size}/t{imageType}t{yearMonth}-{id}.{format}", new { controller = "image", action = "index" }, ns);
        }
    }
}
