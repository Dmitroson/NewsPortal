﻿using Business.Models;
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
        public ActionResult Index(string searchString = "", int sortOrder = 2, string filterString = "", int page = 1)
        {
            WriteLogs("admin entered the site");
            var articles = service.Articles;

            var articlesPerPage = 10;
            var articlesIndex = service.GetArticlesBy(searchString, sortOrder, filterString, page, articlesPerPage);

            var articlesViewModel = new ArticleIndexViewModel
            {
                Articles = articlesIndex.Articles,
                PageInfo = articlesIndex.PageInfo
            };

            return View(articlesViewModel);
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            var article = service.GetArticle(id);
            return View(article);
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
            if (article.PubDate == null)
            {
                article.PubDate = DateTime.Now;
            }

            if (ModelState.IsValid && uploadImage != null)
            {
                var imageUrl = BuildImageUrl(article);
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
                    var imageUrl = BuildImageUrl(article) + uploadImage.FileName;
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

        string BuildImageUrl(Article article)
        {
            var articlePubDateString = article.PubDate.ToString().Replace(".", string.Empty);
            articlePubDateString = articlePubDateString.Replace(":", string.Empty);
            articlePubDateString = articlePubDateString.Replace(" ", string.Empty);
            articlePubDateString = articlePubDateString.Remove(articlePubDateString.Length - 2);

            var imageUrl = "/Images/" + article.Title + "_" + articlePubDateString + "/";
            return imageUrl;
        }
    }
}