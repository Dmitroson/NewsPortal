using Business.Models;
using Business.Services;
using Lucene;
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

            var typeSource = ConfigurationManager.AppSettings["typeSource"];
            string luceneDirectoryPath = "";
            switch (typeSource)
            {
                case "xml":
                    var xmlConnectionString = Server.MapPath(ConfigurationManager.ConnectionStrings["NewsPortalXmlConnection"].ConnectionString);
                    ServiceManager.SetUnitOfWork(new XmlUnitOfWork(xmlConnectionString));
                    ServiceManager.SetArticleRepository(new XmlArticleRepository());
                    ServiceManager.SetCommentRepository(new XmlCommentRepository());
                    luceneDirectoryPath = ConfigurationManager.ConnectionStrings["LuceneDirectoryForXml"].ConnectionString;
                    break;
                case "nhibernate":
                    NHibernateHelper.ConnectionString = ConfigurationManager.ConnectionStrings["NewsPortalDbConnection"].ConnectionString;
                    ServiceManager.SetUnitOfWork(new NHUnitOfWork());
                    ServiceManager.SetArticleRepository(new ArticleRepository());
                    ServiceManager.SetCommentRepository(new CommentRepository());
                    luceneDirectoryPath = ConfigurationManager.ConnectionStrings["LuceneDirectoryForNHibernate"].ConnectionString;
                    break;
            }

            luceneDirectoryPath = Server.MapPath(luceneDirectoryPath);

            ServiceManager.SetLuceneSearcher(new LuceneSearcher(luceneDirectoryPath));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
