using Business.Models;
using Business.Services;
using NewsPortal.CustomModelBinders;
using NHibernate.DAL.Repositories;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Xml.DAL.Repositories;

namespace NewsPortal
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ModelBinders.Binders.Add(typeof(Criteria), new CriteriaModelBinder());

            var typeConnection = ConfigurationManager.AppSettings["typeConnection"];
            switch (typeConnection)
            {
                case "xml":
                    var xmlConnectionString = Server.MapPath(ConfigurationManager.ConnectionStrings["NewsPortalXmlConnection"].ConnectionString);
                    ServiceManager.SetUnitOfWork(new XmlUnitOfWork(xmlConnectionString));
                    ServiceManager.SetArticleRepository(new XmlArticleRepository());
                    ServiceManager.SetCommentRepository(new XmlCommentRepository());
                    break;
                default:
                    NHibernateHelper.ConnectionString = ConfigurationManager.ConnectionStrings["NewsPortalDbConnection"].ConnectionString;
                    ServiceManager.SetUnitOfWork(new NHUnitOfWork());
                    ServiceManager.SetArticleRepository(new ArticleRepository());
                    ServiceManager.SetCommentRepository(new CommentRepository());
                    break;
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
