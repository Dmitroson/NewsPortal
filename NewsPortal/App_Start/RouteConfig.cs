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
                name: "DefaultForRULanguage",
                url: "ru/{controller}/{action}/{id}",
                defaults: new { cult = "ru", controller = "Article", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DefaultForENLanguage",
                url: "en/{controller}/{action}/{id}",
                defaults: new { cult = "en", controller = "Article", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Article", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
