using Business.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NewsPortal.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            var path = Server.MapPath("~/App_Data/Data.xml");
            XDocument xdoc = XDocument.Load(path);
            XElement root = xdoc.Element("articles");
            IEnumerable<XElement> xArticles = root.Elements();

            var articles = new List<Article>();
            foreach (var item in xArticles)
            {
                var article = new Article
                {
                    Id = int.Parse(item.Element("id").Value),
                    Title = item.Element("title").Value,
                    Description = item.Element("description").Value,
                    ImageUrl = item.Element("imageUrl").Value,
                    Visibility = bool.Parse(item.Element("visibility").Value),
                    PubDate = DateTime.Parse(item.Element("pubDate").Value),
                };
                articles.Add(article);
            }
            return View(articles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/App_Data/Data.xml");
                XDocument xdoc = XDocument.Load(path);
                XElement root = xdoc.Element("articles");
                if (root.Attribute("lastId") == null)
                {
                    root.Add(new XAttribute("lastId", 0));
                }

                int lastId = int.Parse(root.Attribute("lastId").Value);

                root.Attribute("lastId").Value = (++lastId).ToString();

                root.Add(new XElement("article",
                    new XElement("id", lastId),
                    new XElement("title", article.Title),
                    new XElement("description", article.Description),
                    new XElement("imageUrl", article.ImageUrl),
                    new XElement("visibility", article.Visibility),
                    new XElement("pubDate", article.PubDate)));

                xdoc.Save(path);

                return RedirectToAction("Index");
            }

            return View(article);
        }

        public ActionResult Details(int id)
        {
            var path = Server.MapPath("~/App_Data/Data.xml");
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(path);
            XmlNode node = xdoc.SelectSingleNode($@"/articles/article[id={id}]");
            Article article = new Article
            {
                Id = int.Parse(node["id"].InnerText),
                Title = node["title"].InnerText,
                Description = node["description"].InnerText,
                ImageUrl = node["imageUrl"].InnerText,
                Visibility = bool.Parse(node["visibility"].InnerText),
                PubDate = DateTime.Parse(node["pubDate"].InnerText),
            };
            return View(article);
        }

        public ActionResult Delete(int id)
        {
            var path = Server.MapPath("~/App_Data/Data.xml");
            XDocument xdoc = XDocument.Load(path);
            var article = from item in xdoc.Element("articles").Elements("article")
                          where int.Parse(item.Element("id").Value) == id
                          select new Article
                          {
                              Id = int.Parse(item.Element("id").Value),
                              Title = item.Element("title").Value,
                              Description = item.Element("description").Value,
                              ImageUrl = item.Element("imageUrl").Value,
                              Visibility = bool.Parse(item.Element("visibility").Value),
                              PubDate = DateTime.Parse(item.Element("pubDate").Value),
                          };
            return View(article.First());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var path = Server.MapPath("~/App_Data/Data.xml");
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(path);
            XmlNode node = xdoc.SelectSingleNode($@"/articles/article[id={id}]");
            node.ParentNode.RemoveChild(node);
            xdoc.Save(path);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var path = Server.MapPath("~/App_Data/Data.xml");
            XDocument xdoc = XDocument.Load(path);
            var article = from item in xdoc.Element("articles").Elements("article")
                          where int.Parse(item.Element("id").Value) == id
                          select new Article
                          {
                              Id = int.Parse(item.Element("id").Value),
                              Title = item.Element("title").Value,
                              Description = item.Element("description").Value,
                              ImageUrl = item.Element("imageUrl").Value,
                              Visibility = bool.Parse(item.Element("visibility").Value),
                              PubDate = DateTime.Parse(item.Element("pubDate").Value),
                          };
            return View(article.First());
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Article articleView, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/App_Data/Data.xml");
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(path);
                XmlNode node = xdoc.SelectSingleNode($@"/articles/article[id={articleView.Id}]");
                node["title"].InnerText = articleView.Title;
                node["description"].InnerText = articleView.Description;
                node["imageUrl"].InnerText = articleView.ImageUrl;
                node["visibility"].InnerText = articleView.Visibility.ToString();
                node["pubDate"].InnerText = articleView.PubDate.ToString();
                xdoc.Save(path);
                
                return RedirectToAction("Index");
            }
            return View(articleView);
        }

    }
}