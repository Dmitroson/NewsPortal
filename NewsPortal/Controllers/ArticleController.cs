using Business.Models;
using Business.Services;
using NewsPortal.Attributes;
using NewsPortal.Helpers;
using NewsPortal.ViewModels;
using System.Web;
using System.IO;
using System.Configuration;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    [Culture]
    [ExceptionLogger]
    public class ArticleController : Controller
    {
        private ArticleService service;
         
        public ArticleController()
        {
            service = new ArticleService();
        }

        // GET: Article
        [HttpGet]
        public ActionResult Index(Criteria criteria)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            var articles = service.GetArticlesBy(criteria, true);

            var pageInfo = new PageInfo
            {
                PageNumber = criteria.Page,
                PageSize = criteria.ArticlesPerPage,
                TotalItems = articles.TotalItems
            };

            var articlesViewModel = new ArticleIndexViewModel
            {
                Articles = articles,
                PageInfo = pageInfo
            };

            return View(articlesViewModel);
        }

        // GET: Article/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var article = service.GetArticle((int)id);
            if (article == null)
            {
                return HttpNotFound();
            }

            return View(article);
        }

        public ActionResult UpdateLuceneIndex()
        {
            var luceneIndexUrl = ConfigurationManager.ConnectionStrings["LuceneDirectory"].ConnectionString;
            var path = Server.MapPath(luceneIndexUrl);

            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
                dir.Create();

            var articles = service.Articles;
            return RedirectToAction("Index");
        }

    }
}