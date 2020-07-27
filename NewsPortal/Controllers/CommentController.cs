using AutoMapper;
using Business.Models;
using Business.Services;
using NewsPortal.ViewModels;
using NHibernate.DAL.Repositories;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class CommentController : Controller
    {
        private CommentService service;

        public CommentController()
        {
            service = new CommentService(new UnitOfWork());
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
                var config = new MapperConfiguration(cfg => cfg.CreateMap<CommentViewModel, Comment>());
                var mapper = new Mapper(config);
                var comment = mapper.Map<Comment>(commentViewModel);
                service.CreateComment(comment, articleId);
                Response.Redirect(Request.RawUrl);
            }
            return View(commentViewModel);
        }

        public ActionResult SureDelete(Comment comment)
        {
            if (comment != null)
                return PartialView("Delete", comment);
            return HttpNotFound();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var articleId = service.GetArticleIdByCommentId(id);
            service.DeleteComment(id);
            return RedirectToRoute(new { controller = "Admin", action = "Details", id = articleId.ToString() });
        }
    }
}