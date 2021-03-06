﻿using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace NewsPortal.Attributes
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

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
        }
    }
}