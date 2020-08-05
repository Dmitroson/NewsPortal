using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHibernate.DAL.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ISession session;

        public ArticleRepository(ISession session)
        {
            this.session = session;
        }

        public IEnumerable<Article> GetAll()
        {
            var articles = session.Query<Article>();
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
    }
}