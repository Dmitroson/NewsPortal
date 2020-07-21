using Business.Models;
using NewsPortal.ViewModels;
using NHibernate.DAL.Repositories;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Business.Services;
using AutoMapper;
using MultilingualSite.Filters;
using System.Collections.Generic;
using System;

namespace NewsPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    [Culture]
    public class AdminController : Controller
    {
        private Service service;

        public AdminController()
        {
            service = new Service(new UnitOfWork());
        }

        public ActionResult ChangeCulture(string lang)
        {
            List<string> cultures = new List<string>() { "ru", "en" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
            {
                cookie.Value = lang;
            }
            else
            {
                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("", new { cult = lang });
        }

        private void checkLang(string req)
        {
            if (req == "en")
            {
                ChangeCulture("en");
            }
        }

        // GET: Admin
        public ActionResult Index(string searchString = "", int sortOrder = 1, string filterString = "", int page = 1)
        {
            checkLang(Request.QueryString["cult"]);
            var articles = service.Articles;

            articles = service.Filter(articles, filterString);
            articles = service.Search(articles, searchString);
            articles = service.Sort(articles, sortOrder);

            var articlesIndex = service.MakePaging(articles, page);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ArticlesIndex, ArticleIndexViewModel>());
            var mapper = new Mapper(config);
            var articlesViewModel = mapper.Map<ArticleIndexViewModel>(articlesIndex);

            return View(articlesViewModel);
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            checkLang(Request.QueryString["cult"]);
            var article = service.GetArticle(id);
            return View(article);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            checkLang(Request.QueryString["cult"]);
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
                var imageUrl = "/Images/";
                var path = Server.MapPath(imageUrl);

                DirectoryInfo dir = new DirectoryInfo(path);
                if (!dir.Exists)
                    dir.Create();

                path += uploadImage.FileName;
                uploadImage.SaveAs(path);
                article.ImageUrl = imageUrl + uploadImage.FileName;
                
                service.CreateArticle(article);

                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            checkLang(Request.QueryString["cult"]);
            var article = service.GetArticle(id);
            return View(article);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Article article, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                if (uploadImage != null)
                {
                    var imageUrl = "/Images/" + uploadImage.FileName;
                    var path = Server.MapPath(imageUrl);
                    uploadImage.SaveAs(path);
                    article.ImageUrl = imageUrl;
                }

                service.UpdateArticle(article);

                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            checkLang(Request.QueryString["cult"]);
            var article = service.GetArticle(id);
            return View(article);
        }

        // POST: Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            service.DeleteArticle(id);
            return RedirectToAction("Index");
        }

        public ActionResult GetComments(int articleId)
        {
            var comments = service.GetComments(articleId);
            return PartialView("~/Views/Comments/CommentsForAdmin.cshtml", comments);
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
                service.CreateComment(comment, id);
                Response.Redirect(Request.RawUrl);
            }
            return View(comment);
        }

        public ActionResult DeleteComment(int id)
        {
            var articleId = service.GetArticleIdByCommentId(id);
            service.DeleteComment(id);
            return RedirectToRoute(new { controller = "Admin", action = "Details", id = articleId.ToString() });
        }
    }
}
