﻿using Business.Models;
using Business.Services;
using NewsPortal.Attributes;
using NewsPortal.ViewModels;
using NHibernate.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    [ExceptionLogger]
    public class CommentController : Controller
    {
        private CommentService service;

        public CommentController()
        {
            service = new CommentService();
        }

        // GET: Comment
        public ActionResult CommentsList(int articleId)
        {
            var comments = service.GetComments(articleId);
            return PartialView("CommentsList", comments);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentViewModel commentViewModel, int articleId)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    UserName = commentViewModel.UserName,
                    Text = commentViewModel.Text
                };
                service.CreateComment(comment, articleId);
                Response.Redirect(Request.RawUrl);
            }
            return View(commentViewModel);
        }

        public ActionResult SureDelete(Comment comment)
        {
            ChangeLanguage();
            if (comment != null)
                return PartialView("Delete", comment);
            return HttpNotFound();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            ChangeLanguage();
            var articleId = service.GetArticleIdByCommentId(id);
            service.DeleteComment(id);
            return RedirectToRoute(new { controller = "Admin", action = "Details", id = articleId.ToString() });
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
    }
}