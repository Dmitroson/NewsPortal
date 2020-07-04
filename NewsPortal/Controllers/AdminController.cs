﻿using NewsPortal.Models;
using NHibernate;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var articles = session.Query<Article>().ToList();
                return View(articles);
            }
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var article = session.Get<Article>(id);
                    transaction.Commit();
                    return View(article);
                }
            }
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                using(ISession session = NHibernateHelper.OpenSession())
                {
                    using(ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(article);
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(article);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var article = session.Get<Article>(id);
                    transaction.Commit();
                    return View(article);
                }
            }
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(article);
                        transaction.Commit();
                        return RedirectToAction("Details", article.Id);
                    }
                }
            }
            return View(article);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var article = session.Get<Article>(id);
                    transaction.Commit();
                    return View(article);
                }
            }
        }

        // POST: Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var article = session.Get<Article>(id);
                    session.Delete(article);
                    transaction.Commit();
                    return RedirectToAction("Index");
                }
            }
        }
    }
}