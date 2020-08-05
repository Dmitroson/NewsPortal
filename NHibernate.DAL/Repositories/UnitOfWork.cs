using Business.Interfaces;
using Business.Models;

namespace NHibernate.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ISession session;
        private ArticleRepository articleRepository;
        private CommentRepository commentRepository;

        private bool disposed;

        public UnitOfWork()
        {
            session = NHibernateHelper.OpenSession();
            articleRepository = new ArticleRepository(session);
            commentRepository = new CommentRepository(session);
        }

        public IArticleRepository Articles
        {
            get
            {
                return articleRepository;
            }
        }

        public ICommentRepository Comments
        {
            get
            {
                return commentRepository;
            }
        }

        public void Save()
        {
            using(ITransaction transaction = session.BeginTransaction())
            {
                transaction.Commit();
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                session.Close();
                disposed = true;
            }
        }
    }
}
