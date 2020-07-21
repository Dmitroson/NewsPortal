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
                name: "Default",
                url: "{cult}/{controller}/{action}/{id}",
                defaults: new { cult = "ru", controller = "Article", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
