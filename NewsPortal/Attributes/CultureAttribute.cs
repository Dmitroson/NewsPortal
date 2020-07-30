using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MultilingualSite.Filters
{
    public class CultureAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string language = "ru";
            if (filterContext.HttpContext.Request.RequestContext.RouteData.Values["lang"] != null)
            {
                language = filterContext.HttpContext.Request.RequestContext.RouteData.Values["lang"].ToString();
            }
            List<string> cultures = new List<string>() { "ru", "en" };
            if (!cultures.Contains(language))
            {
                language = "ru";
            }
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies["lang"];
            if (cookie != null)
            {
                cookie.Value = language;
            }
            else
            {
                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = language;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            filterContext.HttpContext.Response.Cookies.Add(cookie);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);

        }
    }
}