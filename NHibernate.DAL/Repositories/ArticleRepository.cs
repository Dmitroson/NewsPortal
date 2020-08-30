using Business.Models;
using System.Linq;

namespace NHibernate.DAL.Repositories
{
    public class ArticleRepository : NHBaseRepository<Article>
    {
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