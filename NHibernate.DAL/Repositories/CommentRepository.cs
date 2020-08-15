using Business.Interfaces;
using Business.Models;
using Business.Services;
using System.Collections.Generic;
using System.Linq;

namespace NHibernate.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private ISession Session
        {
            get
            {
                return NHibernateHelper.GetSession();
            }
        }

        public IEnumerable<Comment> GetAll()
        {
            var comments = Session.Query<Comment>();
            return comments;
        }

        public Comment Get(int id)
        {
            var comment = Session.Get<Comment>(id);
            return comment;
        }

        public IEnumerable<Comment> GetCommentsBy(int articleId)
        {
            var comments = Session.Query<Comment>().Where(c => c.ArticleId == articleId).ToList();
            return comments;
        }
        
        public void Create(Comment comment)
        {
            Session.Save(comment);  
        }

        public void Update(Comment comment)
        {
            Session.Update(comment);
        }

        public void Delete(int id)
        {
            var comment = Session.Get<Comment>(id);
            Session.Delete(comment);
        }
    }
}