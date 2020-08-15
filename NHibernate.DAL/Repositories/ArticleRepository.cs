using Business.Interfaces;
using Business.Models;
using Business.Services;
using System.Collections.Generic;
using System.Linq;

namespace NHibernate.DAL.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private ISession Session
        {
            get
            {
                return NHibernateHelper.GetSession();
            }
        }

        public IEnumerable<Article> GetAll()
        {
            var articles = Session.Query<Article>();
            return articles;
        }

        public ArticleCollection GetArticlesBy(Criteria criteria, bool onlyVisible)
        {
            var articlesQuery = Session.Query<Article>();
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

        public Article Get(int id)
        {
            var article = Session.Get<Article>(id);
            return article;
        }

        public void Create(Article article)
        {
            Session.Save(article);
        }

        public void Update(Article article)
        {
            Article editedArticle = Session.Get<Article>(article.Id);
            editedArticle.Title = article.Title;
            editedArticle.Description = article.Description;
            editedArticle.ImageUrl = article.ImageUrl;
            editedArticle.Visibility = article.Visibility;
            editedArticle.PubDate = article.PubDate;

            Session.Update(editedArticle);
        }

        public void Delete(int id)
        {
            var article = Session.Get<Article>(id);
            var comments = Session.Query<Comment>().Where(c => c.ArticleId == id).ToList();
            foreach (var comment in comments)
            {
                Session.Delete(comment);
            }
            Session.Delete(article);
        }
    }
}