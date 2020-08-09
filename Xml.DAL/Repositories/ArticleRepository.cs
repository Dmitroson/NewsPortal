using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Xml.DAL.Repositories
{
    public class ArticleRepository
    {
        private string path;

        public ArticleRepository()
        {
            path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
        }

        public IEnumerable<Article> GetAll()
        {
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
            return articles;
        }

        public Article Get(int id)
        {
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
            return article;
        }

        public ArticleCollection GetArticlesBy(Criteria criteria, bool onlyVisible)
        {
            var articlesQuery = (IQueryable <Article>) GetAll();
            articlesQuery = QueriesLogic.Filter(articlesQuery, criteria.FilterString, onlyVisible);
            articlesQuery = QueriesLogic.Search(articlesQuery, criteria.SearchString);
            articlesQuery = QueriesLogic.Sort(articlesQuery, criteria.SortOrder);

            var articles = new ArticleCollection
            {
                TotalItems = articlesQuery.Count()
            };

            articlesQuery = articlesQuery.Skip((criteria.Page) * criteria.ArticlesPerPage).Take(criteria.ArticlesPerPage);

            articles.AddItems(articlesQuery);
            return articles;
        }

        public void Create(Article article)
        {
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
        }

            public void Update(Article article)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(path);
            XmlNode node = xdoc.SelectSingleNode($@"/articles/article[id={article.Id}]");
            node["title"].InnerText = article.Title;
            node["description"].InnerText = article.Description;
            node["imageUrl"].InnerText = article.ImageUrl;
            node["visibility"].InnerText = article.Visibility.ToString();
            node["pubDate"].InnerText = article.PubDate.ToString();
            xdoc.Save(path);
        }

        public void Delete(int id)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(path);
            XmlNode node = xmldoc.SelectSingleNode($@"/articles/article[id={id}]");
            XDocument xdoc = XDocument.Load(path);
            XElement xComments = xdoc.Element("comments");
            var comments = from item in xComments.Elements("comment")
                           where int.Parse(item.Element("articleId").Value) == id
                           select item;
            foreach (var comment in comments)
            {
                comment.Remove();
            }
            node.ParentNode.RemoveChild(node);
            xdoc.Save(path);
        }
    }
}