﻿using AutoMapper;
using Business.Models;
using Business.Services;
using NewsPortal.ViewModels;
using NHibernate.DAL.Repositories;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class ArticleController : Controller
    {
        private Service service;

        public ArticleController()
        {
            service = new Service(new UnitOfWork());
        }

        // GET: Article
        public ActionResult Index(string sortOrder = "Date", int page = 1, string parameters = "")
        {
            var articles = service.Articles.Where(a => a.PubDate <= DateTime.Now.AddHours(3) && a.Visibility == true);

            service.ParseParams(parameters, out string searchString, out string filterString);

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
        public ActionResult CreateComment(Comment comment, int id)
        {
            if (ModelState.IsValid)
            {
                service.CreateComment(comment, id);
                Response.Redirect(Request.RawUrl);
            }
            return View(comment);
        }
    }
}