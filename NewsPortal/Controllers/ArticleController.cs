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

        // GET: Article
        public ActionResult Index(string searchString = "", int sortOrder = 1, string filterString = "", int page = 1)
        {
            WriteLogs("user entered the site");
            ChangeCulture(Request.RequestContext.RouteData.Values["lang"].ToString());

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
            ChangeCulture(Request.RequestContext.RouteData.Values["lang"].ToString());
            var article = service.GetArticle(id);
            return View(article);
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