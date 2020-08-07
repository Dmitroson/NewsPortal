using Business.Models;
using Business.Services;
using NewsPortal.CustomModelBinders;
using NHibernate.DAL.Repositories;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NewsPortal
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            NHibernateHelper.ConnectionString = ConfigurationManager.ConnectionStrings["NewsPortalDbConnection"].ConnectionString;
            ModelBinders.Binders.Add(typeof(Criteria), new CriteriaModelBinder());

            ServiceManager.SetUnitOfWork(new NHUnitOfWork());
            ServiceManager.SetArticleRepository(new ArticleRepository());
            ServiceManager.SetCommentRepository(new CommentRepository());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
