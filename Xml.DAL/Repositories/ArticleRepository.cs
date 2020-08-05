using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace NHibernate.DAL.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private Stream stream;

        public ArticleRepository(Stream stream)
        {
            this.stream = stream;
        }

        public IEnumerable<Article> GetAll()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(IEnumerable<Article>));
            var articles = (IEnumerable<Article>)formatter.Deserialize(stream);
            return articles;
        }

        public ArticleCollection GetArticlesBy(Criteria criteria, int articlesPerPage, bool onlyVisible)
        {
            var articles = new ArticleCollection();

            var articlesQuery = session.Query<Article>();
            articlesQuery = QueriesLogic.Filter(articlesQuery, criteria.FilterString, onlyVisible);
            articlesQuery = QueriesLogic.Search(articlesQuery, criteria.SearchString);
            articlesQuery = QueriesLogic.Sort(articlesQuery, criteria.SortOrder);

            articles.TotalItems = articlesQuery.Count();

            articlesQuery = articlesQuery.Skip((criteria.Page) * articlesPerPage).Take(articlesPerPage);

            articles.AddItems(articlesQuery);
            return articles;
        }

        public Article Get(int id)
        {
            var article = session.Get<Article>(id);
            return article;
        }

        public void Create(Article article)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(article);
                transaction.Commit();
            }
        }

        public void Update(Article article)
        {
            Article editedArticle = session.Get<Article>(article.Id);
            editedArticle.Title = article.Title;
            editedArticle.Description = article.Description;
            editedArticle.ImageUrl = article.ImageUrl;
            editedArticle.Visibility = article.Visibility;
            editedArticle.PubDate = article.PubDate;

            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(editedArticle);
                transaction.Commit();
            }

        }

        public void Delete(int id)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var article = session.Get<Article>(id);
                var comments = session.Query<Comment>().Where(c => c.ArticleId == id).ToList();
                foreach (var comment in comments)
                {
                    session.Delete(comment);
                }
                session.Delete(article);
                transaction.Commit();
            }
        }

        private void WriteArticleToFile(string filename, Article article)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Article));
            TextWriter writer = new StreamWriter(filename);
            serializer.Serialize(writer, article);
            writer.Close();
        }

        private Article ReadArticleFromFile(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Article));
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate);
            var article = new Article();
            article = (Article)serializer.Deserialize(fs);
            return article;
        }
    }
}