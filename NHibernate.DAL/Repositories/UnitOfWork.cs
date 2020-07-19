using Business.Interfaces;
using Business.Models;

namespace NHibernate.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ISession session;
        private ArticleRepository articleRepository;
        private CommentRepository commentRepository;

        public UnitOfWork()
        {
            session = NHibernateHelper.OpenSession();
            articleRepository = new ArticleRepository(session);
            commentRepository = new CommentRepository(session);
        }

        public IRepository<Article> Articles
        {
            get
            {
                return articleRepository;
            }
        }

        public IRepository<Comment> Comments
        {
            get
            {
                return commentRepository;
            }
        }

        void IUnitOfWork.Save() { }
    }
}
