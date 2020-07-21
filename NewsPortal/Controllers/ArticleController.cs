using AutoMapper;
using Business.Models;
using Business.Services;
using MultilingualSite.Filters;
using NewsPortal.ViewModels;
using NHibernate.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    [Culture]
    public class ArticleController : Controller
    {
        private Service service;

        public ArticleController()
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

        // GET: Article
        public ActionResult Index(string searchString = "", int sortOrder = 1, string filterString = "", int page = 1)
        {
            checkLang(Request.QueryString["cult"]);
            var articles = service.Articles.Where(a => a.PubDate <= DateTime.Now.AddHours(3) && a.Visibility == true);

            articles = service.Filter(articles, filterString);
            articles = service.Search(articles, searchString);
            articles = service.Sort(articles, sortOrder);

            var articlesIndex = service.MakePaging(articles, page);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ArticlesIndex, ArticleIndexViewModel>());
            var mapper = new Mapper(config);
            var articlesViewModel = mapper.Map<ArticleIndexViewModel>(articlesIndex);

            return View(articlesViewModel);
        }

        // GET: Article/Details/5
        public ActionResult Details(int id)
        {
            checkLang(Request.QueryString["cult"]);
            var article = service.GetArticle(id);
            return View(article);
        }

        public ActionResult GetComments(int articleId)
        {
            var comments = service.GetComments(articleId);
            return PartialView("~/Views/Comments/CommentsList.cshtml", comments);
        }

        public ActionResult CreateComment()
        {
            return PartialView("~/Views/Comments/CreateComments.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(CommentViewModel commentViewModel, int id)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<CommentViewModel, Comment>());
                var mapper = new Mapper(config);
                var comment = mapper.Map<Comment>(commentViewModel);
                service.CreateComment(comment, id);
                Response.Redirect(Request.RawUrl);
            }
            return View(commentViewModel);
        }
    }
}