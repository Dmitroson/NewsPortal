using Business.Models;
using NewsPortal.Attributes;
using NewsPortal.ViewModels;
using NewsPortal.Helpers;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Cache.Services;
using System.Linq;

namespace NewsPortal.Controllers
{
    [Culture]
    [ExceptionLogger]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ArticleServiceWeb service;

        public AdminController()
        {
            service = new ArticleServiceWeb();
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
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }

            var article = service.GetArticle(id.Value);
            if(article == null)
            {
                return HttpNotFound();
            }

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
                    SaveArticleImage(article, uploadImage);
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
                    SaveArticleImage(article, uploadImage);
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

        private void SaveArticleImage(Article article, HttpPostedFileBase uploadImage)
        {
            var imageUrl = BuildImageUrl(article);
            var path = Server.MapPath(imageUrl);

            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
                dir.Create();

            path = Path.Combine(path, uploadImage.FileName);
            uploadImage.SaveAs(path);
            article.ImageUrl = Path.Combine(imageUrl, uploadImage.FileName);
        }

        private string BuildImageUrl(Article article)
        {
            var articlePubDateString = article.PubDate.ToString();
            articlePubDateString = articlePubDateString.Replace(" ", "-");

            var imageFolderName = string.Concat(article.Title, "_", articlePubDateString);

            var invalidChars = Path.GetInvalidFileNameChars();
            foreach(var invalidChar in invalidChars)
            {
                imageFolderName = imageFolderName.Replace(invalidChar, '-');
            }

            var imageUrl = Path.Combine(@"\Images", imageFolderName);
            return imageUrl;
        }
    }
}