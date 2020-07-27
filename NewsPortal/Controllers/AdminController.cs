using AutoMapper;
using Business.Models;
using Business.Services;
using MultilingualSite.Filters;
using NewsPortal.Helpers;
using NewsPortal.ViewModels;
using NHibernate.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    [Culture]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ArticleService service;

        public AdminController()
        {
            service = new ArticleService(new UnitOfWork());
        }

        // GET: Admin
        public ActionResult Index(string searchString = "", int sortOrder = 1, string filterString = "", int page = 1)
        {
            WriteLogs("admin entered the site");
            ChangeLanguage();
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
            ChangeLanguage();
            var article = service.GetArticle(id);
            return View(article);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            ChangeLanguage();
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
            ChangeLanguage();
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
            ChangeLanguage();
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

        public ActionResult ChangeCulture(string language)
        {
            List<string> cultures = new List<string>() { "ru", "en" };
            if (!cultures.Contains(language))
            {
                language = "ru";
            }
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
            {
                cookie.Value = language;
            }
            else
            {
                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = language;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("", new { lang = language });
        }

        public void ChangeLanguage()
        {
            var currentLanguage = Request.RequestContext.RouteData.Values["lang"].ToString();
            if (currentLanguage == "en")
            {
                ChangeCulture("en");
            }
            else
            {
                ChangeCulture("ru");
            }
        }

        void WriteLogs(string message)
        {
            try
            {
                LoggerHelper.WriteDebug(null, message);
            }
            catch (Exception e)
            {
                LoggerHelper.WriteError(e, "When" + message);
                LoggerHelper.WriteFatal(e, "When" + message);
                LoggerHelper.WriteVerbose(e, "When" + message);
                throw;
            }
        }
    }
}