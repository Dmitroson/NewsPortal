using Business.Interfaces;
using Business.Models;
using System.Linq;

namespace NHibernate.DAL.Repositories
{
    public class ArticleRepository : IRepository<Article>
    {
        private ISession session;

        public ArticleRepository(ISession session)
        {
            this.session = session;
        }

        public IQueryable<Article> GetAll()
        {
            var articles = session.Query<Article>();
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
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(article);
                transaction.Commit();
            }
        }

        public void Delete(int id)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var article = session.Get<Article>(id);
                session.Delete(article);
                transaction.Commit();
            }
        }

        public void Save() { }
    }
}