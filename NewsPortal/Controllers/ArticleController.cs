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
        public ActionResult Index(string sortOrder = "Date", int page = 1, string parameters = "")
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ParseParams(parameters, out string searchString, out string filterString);

                var articles = session.Query<Article>().Where(a => a.PubDate <= DateTime.Now && a.Visibility == true);

                DateFilter filter = new DateFilter(filterString);
                articles = filter.FilterByDate(articles);

                if (searchString != "")
                {
                    articles = articles.Where(a => a.Title.Contains(searchString)
                                                || a.Description.Contains(searchString));
                }

                articles = Sort(articles, sortOrder);

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


        public ActionResult GetComments(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var article = session.Get<Article>(id);
                    return PartialView("~/Views/Comments/CommentsList.cshtml", article.Comments.ToList());
                }
            }
        }

        public ActionResult CreateComment()
        {
            return PartialView("~/Views/Comments/CreateComments.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(Comment comment, int id)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var article = session.Get<Article>(id);
                        comment.PubDate = DateTime.Now;
                        comment.Article = article;
                        session.Save(comment);
                        article.Comments.Add(comment);
                        transaction.Commit();
                        Response.Redirect(Request.RawUrl);
                        //return View("Details", article);                        
                    }
                }
            }
            return View(comment);
        }

        private IQueryable<Article> Sort(IQueryable<Article> articles, string order)
        {
            switch (order)
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
            return articles;
        }

        private void ParseParams(string paramsString, out string searchString, out string filterString)
        {
            searchString = "";
            filterString = "";
            string[] paramsArray = paramsString.Split('&');

            switch (paramsArray.Length)
            {
                case 1:
                    int foundIndex = paramsArray[0].IndexOf("=");
                    if (paramsArray[0].Contains("searchString"))
                    {
                        searchString = paramsArray[0].Substring(foundIndex + 1);
                        filterString = "";
                    }
                    else
                    {
                        filterString = paramsArray[0].Substring(foundIndex + 1);
                        searchString = "";
                    }
                    break;
                case 2:
                    int foundIndex1 = paramsArray[0].IndexOf("=");
                    searchString = paramsArray[0].Substring(foundIndex1 + 1);

                    int foundIndex2 = paramsArray[1].IndexOf("=");
                    filterString = paramsArray[1].Substring(foundIndex2 + 1);
                    break;
            }
        }
    }
}