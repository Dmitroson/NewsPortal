using NewsPortal.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index(string sortOrder = "Date", int page = 1, string keywords = "", string filter = "all")
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var articles = session.Query<Article>();

                if (keywords != "")
                {
                    articles = articles.Where(a => a.Title.Contains(keywords)
                                                || a.Description.Contains(keywords));
                }

                switch (sortOrder)
                {
                    case "Title":
                        articles = articles.OrderBy(a => a.Title);
                        break;
                    case "Description":
                        articles = articles.OrderBy(a => a.Description);
                        break;
                    default:
                        articles = articles.OrderByDescending(a => a.PubDate);
                        break;
                }

                switch (filter)
                {
                    case "today":
                        articles = articles.Where(a => a.PubDate == DateTime.Today);
                        break;
                    case "yesterday":
                        articles = articles.Where(a => a.PubDate == DateTime.Today.AddDays(-1));
                        break;
                    case "last week":
                        articles = articles.Where(a => (a.PubDate >= DateTime.Today.AddDays(-7) && a.PubDate <= DateTime.Today));
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
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var article = session.Get<Article>(id);
                    transaction.Commit();
                    return View(article);
                }
            }
        }

        
        //public ActionResult GetComments(int articleId)
        //{
        //    using (ISession session = NHibernateHelper.OpenSession())
        //    {
        //        using (ITransaction transaction = session.BeginTransaction())
        //        {
        //            var comments = session.Query<Comment>()
        //                                  .Where(c => c.Article.Id == articleId)
        //                                  .ToList();
        //            transaction.Commit();
        //            return PartialView("Comments", comments);
        //        }
        //    }
        //}
    }
}