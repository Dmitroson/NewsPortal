using Business.Models;
using Business.Services;
using NewsPortal.Attributes;
using NewsPortal.ViewModels;
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
        [HttpGet]
        public ActionResult CommentsList(int articleId)
        {
            var comments = service.GetComments(articleId);
            return PartialView(comments);
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

        //rename 
        //antiforgerys
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