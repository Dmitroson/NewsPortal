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
        public ActionResult Index(string searchString = "", int sortOrder = 2, string filterString = "", int page = 1)
        {
            WriteLogs("user entered the site");

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
            var article = service.GetArticle(id);
            return View(article);
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