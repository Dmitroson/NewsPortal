using NewsPortal.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        [HttpGet]
        public ActionResult Index(string sortOrder = "Date", int page = 1, string parameters = "")
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var articles = session.Query<Article>();

                ParseParams(parameters, out string searchString, out string filterString);
                
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

        // GET: Admin/Details/5
        [HttpGet]
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

        // GET: Admin/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Article article, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Images/"));
                if (!dir.Exists)
                    dir.Create();

                var path = Server.MapPath("~/Images/") + uploadImage.FileName;
                uploadImage.SaveAs(path);
                article.ImageUrl = "/Images/" + uploadImage.FileName;

                if (article.PubDate == null)
                {
                    article.PubDate = DateTime.Now;
                }

                using (ISession session = NHibernateHelper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save("Article", article);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Admin/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
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

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Article article, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                if(uploadImage != null)
                {
                    var path = Server.MapPath("~/Images/") + uploadImage.FileName;
                    uploadImage.SaveAs(path);
                    article.ImageUrl = "/Images/" + uploadImage.FileName;
                }
                
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(article);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Admin/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
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

        // POST: Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var article = session.Get<Article>(id);
                    session.Delete(article);
                    transaction.Commit();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetComments(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var article = session.Get<Article>(id);
                    return PartialView("~/Views/Comments/CommentsForAdmin.cshtml", article.Comments.ToList());
                }
            }
        }

        public ActionResult CreateComment()
        {
            return PartialView("~/Views/Comments/CreateComments.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void CreateComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        //var article = session.Get<Article>(id);
                        comment.PubDate = DateTime.Now;
                        //comment.Article = article;
                        session.Save(comment);
                        //article.Comments.Add(comment);
                        transaction.Commit();
                        Response.Redirect(Request.RawUrl);
                    }
                }
            }
        }

        public RedirectToRouteResult DeleteComment(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {

                    var comment = session.Get<Comment>(id);
                    int articleId = comment.Article.Id;
                    session.Delete(comment);
                    transaction.Commit();
                    return RedirectToRoute(new { controller = "Admin", action = "Details", id = articleId.ToString() });
                }
            }
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
