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
        private ArticleService service;
         
        public ArticleController()
        {
            service = new ArticleService(new UnitOfWork());
        }

        // GET: Article
        public ActionResult Index(string searchString = "", int sortOrder = 1, string filterString = "", int page = 1)
        {
            WriteLogs("user entered the site");
            ChangeLanguage();

            var articlesPerPage = 10;
            var articlesIndex = service.GetArticlesBy(searchString, sortOrder, filterString, page, articlesPerPage, true);

            var articlesViewModel = new ArticleIndexViewModel
            {
                Articles = articlesIndex.Articles,
                PageInfo = articlesIndex.PageInfo
            };
            
            return View(articlesViewModel);
        }

        // GET: Article/Details/5
        public ActionResult Details(int id)
        {
            ChangeLanguage();
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

        public ArticleIndexViewModel MakePaging(IQueryable<Article> articles, int page)
        {
            var articlesList = articles.ToList();
            int pageSize = 10;

            IEnumerable<Article> articlesPerPages = articlesList.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = articlesList.Count
            };
            ArticleIndexViewModel articlesIndex = new ArticleIndexViewModel
            {
                Articles = articlesPerPages,
                PageInfo = pageInfo
            };
            return articlesIndex;
        }
    }
}