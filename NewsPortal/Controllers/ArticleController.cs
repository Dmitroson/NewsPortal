using NewsPortal.Models;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index(string sortOrder = "Date", int page = 1)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var articles = session.Query<Article>();
                switch (sortOrder)
                {
                    case "Title":
                        articles = articles.OrderBy(a => a.Title);
                        break;
                    case "Description":
                        articles = articles.OrderBy(a => a.Description);
                        break;
                    default:
                        articles = articles.OrderBy(a => a.PubDate);
                        break;
                }

                var articlesList = articles.ToList();
                int pageSize = 10;
                IEnumerable<Article> articlesPerPages = articlesList.Skip((page - 1) * pageSize).Take(pageSize);
                PageInfo pageInfo = new PageInfo
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = articlesList.Count
                };
                ArticleIndexViewModel articlesViewModel = new ArticleIndexViewModel
                {
                    Articles = articlesPerPages,
                    PageInfo = pageInfo
                };
                return View(articlesViewModel);
            }
        }

        // GET: Article/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


    }
}