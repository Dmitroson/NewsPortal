using Business.Interfaces;
using Business.Models;
using System.Linq;

namespace NHibernate.DAL.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        private ISession session;
        public CommentRepository(ISession session)
        {
            this.session = session;
        }

        IQueryable<Comment> IRepository<Comment>.GetAll()
        {
            var comments = session.Query<Comment>();
            return comments;
        }

        Comment IRepository<Comment>.Get(int id)
        {
            var comment = session.Get<Comment>(id);
            return comment;
        }
        
        void IRepository<Comment>.Create(Comment comment)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(comment);
                transaction.Commit();
            }
        }

        void IRepository<Comment>.Update(Comment comment)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(comment);
                transaction.Commit();
            }
        }

        void IRepository<Comment>.Delete(int id)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var comment = session.Get<Comment>(id);
                session.Delete(comment);
                transaction.Commit();
            }
        }

        void IRepository<Comment>.Save() { }
    }
}