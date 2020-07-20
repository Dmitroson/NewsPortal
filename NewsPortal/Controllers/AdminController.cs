using Business.Models;
using NewsPortal.ViewModels;
using NHibernate.DAL.Repositories;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Business.Services;
using AutoMapper;

namespace NewsPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private Service service;

        public AdminController()
        {
            service = new Service(new UnitOfWork());
        }

        // GET: Admin
        public ActionResult Index(string sortOrder = "Date", int page = 1, string parameters = "")
        {
            var articles = service.Articles;

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
            if (ModelState.IsValid && uploadImage != null)
            {
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Images/"));
                if (!dir.Exists)
                    dir.Create();

                var path = Server.MapPath("~/Images/") + uploadImage.FileName;
                uploadImage.SaveAs(path);
                article.ImageUrl = "/Images/" + uploadImage.FileName;

                
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
                    var path = Server.MapPath("~/Images/") + uploadImage.FileName;
                    uploadImage.SaveAs(path);
                    article.ImageUrl = "/Images/" + uploadImage.FileName;
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

        public ActionResult GetComments(int articleId)
        {
            var comments = service.GetComments(articleId);
            return PartialView("~/Views/Comments/CommentsForAdmin.cshtml", comments);
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

        public ActionResult DeleteComment(int id)
        {
            var articleId = service.GetArticleIdByCommentId(id);
            service.DeleteComment(id);
            return RedirectToRoute(new { controller = "Admin", action = "Details", id = articleId.ToString() });
        }
    }
}
