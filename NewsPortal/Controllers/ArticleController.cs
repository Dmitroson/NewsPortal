using AutoMapper;
using Business.Models;
using Business.Services;
using MultilingualSite.Filters;
using NewsPortal.Helpers;
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

        public void ChangeLanguage(string currentLanguage)
        {
            if (currentLanguage == "en")
            {
                ChangeCulture("en");
            }
            else
            {
                ChangeCulture("ru");
            }
        }

        // GET: Article
        public ActionResult Index(string searchString = "", int sortOrder = 1, string filterString = "", int page = 1)
        {
            try
            {
                LoggerHelper.WriteDebug(null, "Debug ");
                LoggerHelper.WriteWarning(null, "Warning ");
            }
            catch (Exception e)
            {
                LoggerHelper.WriteError(e, "Error");
                LoggerHelper.WriteFatal(e, "Fatal");
                LoggerHelper.WriteVerbose(e, "Verbose");
                throw; 
            }
            ChangeLanguage(Request.RequestContext.RouteData.Values["lang"].ToString());
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
            ChangeLanguage(Request.RequestContext.RouteData.Values["lang"].ToString());
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