using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NewsPortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Login",
                url: "Account/Login",
                defaults: new { lang = "ru", controller = "Account", action = "Login" }
            );

            //routes.MapRoute(
            //    name: "Admin",
            //    url: "Admin",
            //    defaults: new { lang = "ru", controller = "Admin", action = "Index" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { lang = "ru", controller = "Test", action = "Index", id = UrlParameter.Optional }
            );
 
        }
    }
}
