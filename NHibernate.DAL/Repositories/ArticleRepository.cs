using Business.Interfaces;
using Business.Models;
using Business.Services;
using System.Collections.Generic;
using System.Linq;

namespace NHibernate.DAL.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly NHUnitOfWork unitOfWork;

        public ArticleRepository()
        {
            unitOfWork = ServiceManager.GetUnitOfWork() as NHUnitOfWork;
        }

        public IEnumerable<Article> GetAll()
        {
            unitOfWork.OpenSession();

            var articles = unitOfWork.Session.Query<Article>();
            return articles;
        }

        public ArticleCollection GetArticlesBy(Criteria criteria, int articlesPerPage, bool onlyVisible)
        {
            unitOfWork.OpenSession();

            var articlesQuery = unitOfWork.Session.Query<Article>();
            articlesQuery = QueriesLogic.Filter(articlesQuery, criteria.FilterString, onlyVisible);
            articlesQuery = QueriesLogic.Search(articlesQuery, criteria.SearchString);
            articlesQuery = QueriesLogic.Sort(articlesQuery, criteria.SortOrder);

            var articles = new ArticleCollection
            {
                TotalItems = articlesQuery.Count()
            };

            articlesQuery = articlesQuery.Skip((criteria.Page) * articlesPerPage).Take(articlesPerPage);

            articles.AddItems(articlesQuery);
            return articles;
        }

        public Article Get(int id)
        {
            unitOfWork.OpenSession();

            var article = unitOfWork.Session.Get<Article>(id);
            return article;
        }

        public void Create(Article article)
        {
            unitOfWork.OpenSession();
            unitOfWork.BeginTransaction();

            unitOfWork.Session.Save(article);
        }

        public void Update(Article article)
        {
            unitOfWork.OpenSession();
            unitOfWork.BeginTransaction();

            Article editedArticle = unitOfWork.Session.Get<Article>(article.Id);
            editedArticle.Title = article.Title;
            editedArticle.Description = article.Description;
            editedArticle.ImageUrl = article.ImageUrl;
            editedArticle.Visibility = article.Visibility;
            editedArticle.PubDate = article.PubDate;

            unitOfWork.Session.Update(editedArticle);
        }

        public void Delete(int id)
        {
            unitOfWork.OpenSession();
            unitOfWork.BeginTransaction();

            var article = unitOfWork.Session.Get<Article>(id);
            var comments = unitOfWork.Session.Query<Comment>().Where(c => c.ArticleId == id).ToList();
            foreach (var comment in comments)
            {
                unitOfWork.Session.Delete(comment);
            }
            unitOfWork.Session.Delete(article);
        }
    }
}