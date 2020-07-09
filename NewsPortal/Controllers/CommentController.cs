using NewsPortal.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class CommentController : Controller
    {
        
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Comment comment, int id)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var article = session.Get<Article>(id);
                        article.Comments.Add(comment);
                        session.Save("Comment", comment);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(comment);
        }
    }
}