using Business.Models;
using Lucene.Net.Index;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Attributes
{
    public class ExceptionLoggerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            ExceptionDetails exception = new ExceptionDetails
            {
                ExceptionMessage = filterContext.Exception.Message,
                StackTrace = filterContext.Exception.StackTrace,
                ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                ActionName = filterContext.RouteData.Values["action"].ToString(),
                Date = DateTime.Now
            };

            var logsDirectoryPath = HttpContext.Current.Server.MapPath("~/Logs/");
            var dir = new DirectoryInfo(logsDirectoryPath);
            if (!dir.Exists)
                dir.Create();

            var fileName = "logs" + $"{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year}" + ".txt";
            var filePath = logsDirectoryPath + fileName;

            using(StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                string exceptionInfo = $"Date: {exception.Date}\nExceptionMessage: {exception.ExceptionMessage}\nMethod: {exception.ControllerName}/{exception.ActionName}\nStackTrace: {exception.StackTrace}";
                streamWriter.WriteLine(exceptionInfo);
                streamWriter.WriteLine();
            }
        }
    }

}