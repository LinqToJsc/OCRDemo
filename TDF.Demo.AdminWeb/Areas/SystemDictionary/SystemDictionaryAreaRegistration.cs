using System.Web.Mvc;

namespace TDF.Demo.AdminWeb.Areas.SystemDictionary
{
    public class SystemDictionaryAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SystemDictionary";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SystemDictionary_default",
                "SystemDictionary/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}