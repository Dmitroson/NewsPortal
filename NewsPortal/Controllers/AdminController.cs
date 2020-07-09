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
        public ActionResult Index(string filterString, string sortOrder = "Date", int page = 1, string keywords = "")
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var articles = session.Query<Article>();

                FilterByDate filter = new FilterByDate(filterString);


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

                //switch (filter)
                //{
                //    case "today":
                //        articles = articles.Where(a => a.PubDate == DateTime.Today);
                //        break;
                //    case "yesterday":
                //        articles = articles.Where(a => a.PubDate == DateTime.Today.AddDays(-1));
                //        break;
                //    case "last week":
                //        articles = articles.Where(a => (a.PubDate >= DateTime.Today.AddDays(-7) && a.PubDate <= DateTime.Today));
                //        break;
                //}

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

        private string GetFilterString()
        {
            string[] filterString = Request.Form.GetValues("filterString");
            string result = "";
            return result;
        }

        public ActionResult GetComments(int? id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var article = session.Get<Article>(id);
                    return PartialView("~/Views/Comments/PartialViewComments.cshtml", article.Comments);
                }
            }
        }
    }
}
