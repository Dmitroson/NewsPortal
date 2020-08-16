using Business.Models;
using Business.Services;
using NewsPortal.Attributes;
using NewsPortal.Helpers;
using NewsPortal.ViewModels;
using System.Web;
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
        [OutputCache(CacheProfile = "CacheWithCriteria")]
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
        [OutputCache(CacheProfile = "CacheWithId")]
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
    }
}