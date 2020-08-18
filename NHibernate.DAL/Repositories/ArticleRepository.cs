using Business.Interfaces;
using Business.Models;
using System.Linq;

namespace NHibernate.DAL.Repositories
{
    public class ArticleRepository : NHBaseRepository<Article>, IArticleRepository
    {
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

        public override void Update(Article article)
        {
            Article editedArticle = Session.Get<Article>(article.Id);
            editedArticle.Title = article.Title;
            editedArticle.Description = article.Description;
            editedArticle.ImageUrl = article.ImageUrl;
            editedArticle.Visibility = article.Visibility;
            editedArticle.PubDate = article.PubDate;

            Session.Update(editedArticle);
        }

        public override void Delete(int id)
        {
            var comments = Session.Query<Comment>().Where(c => c.ArticleId == id).ToList();
            foreach (var comment in comments)
            {
                Session.Delete(comment);
            }
            base.Delete(id);
        }
    }
}