using Business.Models;
using Business.Services;
using MultilingualSite.Filters;
using NewsPortal.Attributes;
using NewsPortal.Helpers;
using NewsPortal.ViewModels;
using NHibernate.DAL.Repositories;
using System;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    [Culture]
    [ExceptionLogger]
    public class ArticleController : Controller
    {
        private ArticleService service;
         
        public ArticleController()
        {
            service = new ArticleService();
        }

        // GET: Article
        public ActionResult Index(Criteria criteria)
        {
            var articlesPerPage = 10;
            var articles = service.GetArticlesBy(criteria, articlesPerPage, true);

            var pageInfo = new PageInfo
            {
                PageNumber = criteria.Page,
                PageSize = articlesPerPage,
                TotalItems = articles.TotalItems
            };

            var articlesViewModel = new ArticleIndexViewModel
            {
                Articles = articles,
                PageInfo = pageInfo
            };

            return View(articlesViewModel);
        }

        // GET: Article/Details/5
        public ActionResult Details(int id)
        {
            var article = service.GetArticle(id);
            return View(article);
        }
    }
}