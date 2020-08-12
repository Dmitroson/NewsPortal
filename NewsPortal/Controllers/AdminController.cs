using Business.Models;
using Business.Services;
using NewsPortal.Attributes;
using NewsPortal.Helpers;
using NewsPortal.ViewModels;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    [Culture]
    [ExceptionLogger]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ArticleService service;

        public AdminController()
        {
            service = new ArticleService();
        }

        // GET: Admin
        [HttpGet]
        public ActionResult Index(Criteria criteria)
        {
            var articles = service.GetArticlesBy(criteria);

            var pageInfo = new PageInfo
            {
                PageNumber = criteria.Page,
                PageSize = criteria.ArticlesPerPage,
                TotalItems = articles.TotalItems
            };

            var articlesViewModel = new ArticleIndexViewModel
            {
                Articles = articles,
                PageInfo = pageInfo
            };

            return View(articlesViewModel);
        }

        // GET: Admin/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            var article = service.GetArticle(id);
            return View(article);
        }

        // GET: Admin/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(ArticleViewModel articleViewModel, HttpPostedFileBase uploadImage)
        {
            if (articleViewModel.PubDate == null)
            {
                articleViewModel.PubDate = DateTime.Now;
            }

            if (ModelState.IsValid )
            {
                var article = new Article
                {
                    Id = articleViewModel.Id,
                    Title = articleViewModel.Title,
                    Description = articleViewModel.Description,
                    ImageUrl = articleViewModel.ImageUrl,
                    Visibility = articleViewModel.Visibility,
                    PubDate = articleViewModel.PubDate
                };
                if(uploadImage != null)
                {
                    var imageUrl = BuildImageUrl(article);
                    var path = Server.MapPath(imageUrl);

                    DirectoryInfo dir = new DirectoryInfo(path);
                    if (!dir.Exists)
                        dir.Create();

                    path += uploadImage.FileName;
                    uploadImage.SaveAs(path);
                    article.ImageUrl = imageUrl + uploadImage.FileName;
                }

                service.CreateArticle(article);

                return RedirectToAction("Index");
            }
            return View(articleViewModel);
        }

        // GET: Admin/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var article = service.GetArticle(id);
            var articleViewModel = new ArticleViewModel(article);
            return View(articleViewModel);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ArticleViewModel articleViewModel, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                var article = new Article
                {
                    Id = articleViewModel.Id,
                    Title = articleViewModel.Title,
                    Description = articleViewModel.Description,
                    ImageUrl = articleViewModel.ImageUrl,
                    Visibility = articleViewModel.Visibility,
                    PubDate = articleViewModel.PubDate
                };
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
            return View(articleViewModel);
        }

        // GET: Admin/Delete/5
        [HttpGet]
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

        string BuildImageUrl(Article article)
        {
            //Path.GetInvalidFileNameChars();
            //Path.Combine()
            var articlePubDateString = article.PubDate.ToString().Replace(".", string.Empty);
            articlePubDateString = articlePubDateString.Replace(":", string.Empty);
            articlePubDateString = articlePubDateString.Replace(" ", string.Empty);
            articlePubDateString = articlePubDateString.Remove(articlePubDateString.Length - 2);

            var imageUrl = "/Images/" + article.Title + "_" + articlePubDateString + "/";
            return imageUrl;
        }
    }
}