using Business.Interfaces;
using Business.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHibernate.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ISession session;

        public CommentRepository(ISession session)
        {
            this.session = session;
        }

        public IEnumerable<Comment> GetAll()
        {
            var comments = session.Query<Comment>();
            return comments;
        }

        public Comment Get(int id)
        {
            var comment = session.Get<Comment>(id);
            return comment;
        }

        public IEnumerable<Comment> GetCommentsBy(int articleId)
        {
            var comments = session.Query<Comment>().Where(c => c.ArticleId == articleId).ToList();
            return comments;
        }
        
        public void Create(Comment comment)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(comment);
                transaction.Commit();
            }
        }

        public void Update(Comment comment)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(comment);
                transaction.Commit();
            }
        }

        public void Delete(int id)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var comment = session.Get<Comment>(id);
                session.Delete(comment);
                transaction.Commit();
            }
        }
    }
}