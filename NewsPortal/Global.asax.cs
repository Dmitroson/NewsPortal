using Business.Models;
using Business.Services;
using Cache.Repositories;
using Cache.Services;
using Lucene;
using NewsPortal.CustomModelBinders;
using NHibernate.DAL.Repositories;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Xml.DAL;
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
            string luceneCacheDirectoryPath = "";
            switch (typeSource)
            {
                case "xml":
                    XmlHelper.ConnectionString = Server.MapPath(ConfigurationManager.ConnectionStrings["NewsPortalXmlConnection"].ConnectionString);
                    ServiceManager.SetUnitOfWork(new XmlUnitOfWork());
                    ServiceManager.SetArticleRepository(new XmlArticleRepository());
                    ServiceManager.SetCommentRepository(new XmlCommentRepository());
                    luceneDirectoryPath = ConfigurationManager.ConnectionStrings["LuceneDirectoryForXml"].ConnectionString;
                    luceneCacheDirectoryPath = ConfigurationManager.ConnectionStrings["LuceneDirectoryForXmlCache"].ConnectionString;
                    break;
                case "nhibernate":
                    NHibernateHelper.ConnectionString = ConfigurationManager.ConnectionStrings["NewsPortalDbConnection"].ConnectionString;
                    ServiceManager.SetUnitOfWork(new NHUnitOfWork());
                    ServiceManager.SetArticleRepository(new ArticleRepository());
                    ServiceManager.SetCommentRepository(new CommentRepository());
                    luceneDirectoryPath = ConfigurationManager.ConnectionStrings["LuceneDirectoryForNHibernate"].ConnectionString;
                    luceneCacheDirectoryPath = ConfigurationManager.ConnectionStrings["LuceneDirectoryForNHibernateCache"].ConnectionString;
                    break;
            }

            luceneDirectoryPath = Server.MapPath(luceneDirectoryPath);

            ServiceManager.SetLuceneSearcher(new LuceneSearcher(luceneDirectoryPath));
            new ArticleService().UpdateLuceneIndex();

            ServiceManager.SetArticleCacheRepository(new CacheRepository<Article>());
            ServiceManager.SetCommentCacheRepository(new CacheRepository<Comment>());

            luceneCacheDirectoryPath = Server.MapPath(luceneCacheDirectoryPath);
            ServiceManager.SetCacheLuceneSearcher(new LuceneSearcher(luceneCacheDirectoryPath));
            new ArticleServiceWeb().UpdateCacheLuceneIndex();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
