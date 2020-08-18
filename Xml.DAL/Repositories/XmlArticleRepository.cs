using Business.Interfaces;
using Business.Models;
using Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Xml.DAL.Repositories
{
    public class XmlArticleRepository : IArticleRepository
    {
        private const string ISOFormat = "yyyy-MM-dd\\THH:mm:ss";

        private readonly XmlUnitOfWork unitOfWork;

        public XmlArticleRepository()
        {
            unitOfWork = ServiceManager.GetUnitOfWork() as XmlUnitOfWork;
        }

        public IEnumerable<Article> GetAll()
        {
            XElement root = unitOfWork.Document.Element("articles");
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
            XElement xArticle = null;
            foreach(var item in unitOfWork.Document.Element("articles").Elements("article"))
            {
                if(item.Element("id").Value == id.ToString())
                {
                    xArticle = item;
                    break;
                }
            }

            Article article = new Article
            {
                Id = int.Parse(xArticle.Element("id").Value),
                Title = xArticle.Element("title").Value,
                Description = xArticle.Element("description").Value,
                ImageUrl = xArticle.Element("imageUrl").Value,
                Visibility = bool.Parse(xArticle.Element("visibility").Value),
                PubDate = DateTime.Parse(xArticle.Element("pubDate").Value),
            };

            return article;
        }

        public ArticleCollection GetArticlesBy(Criteria criteria, bool onlyVisible)
        {
            var articlesQuery = GetAll();
            articlesQuery = QueriesLogic.Filter(articlesQuery, criteria.FilterRange, onlyVisible);
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
            XElement root = unitOfWork.Document.Element("articles");
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
                new XElement("pubDate", article.PubDate.Value.ToString(ISOFormat))));
        }

        public void Update(Article article)
        {
            XElement xArticle = null;
            foreach (var item in unitOfWork.Document.Element("articles").Elements("article"))
            {
                if (item.Element("id").Value == article.Id.ToString())
                {
                    xArticle = item;
                    break;
                }
            }

            xArticle.Element("title").Value = article.Title;
            xArticle.Element("description").Value = article.Description;
            xArticle.Element("imageUrl").Value = article.ImageUrl;
            xArticle.Element("visibility").Value = article.Visibility.ToString();
            xArticle.Element("pubDate").Value = article.PubDate.Value.ToString(ISOFormat);
        }

        public void Delete(int id)
        {
            XElement xArticle = null;
            foreach (var item in unitOfWork.Document.Element("articles").Elements("article"))
            {
                if (item.Element("id").Value == id.ToString())
                {
                    xArticle = item;
                    break;
                }
            }

            var comments = new List<XElement>();
            foreach(var comment in unitOfWork.Document.Element("comments").Elements("comment"))
            {
                if (comment.Element("articleId").Value == id.ToString())
                {
                    comments.Add(comment);
                }
            }
            for (int i = 0; i < comments.Count; i++)
            {
                comments[i].Remove();
            }

            xArticle.Remove();
        }
    }
}