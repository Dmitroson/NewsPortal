using Business.Interfaces;
using Business.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHibernate.DAL.Repositories
{
    public class CommentRepository : NHBaseRepository<Comment>, ICommentRepository
    {
        public IEnumerable<Comment> GetCommentsBy(int articleId)
        {
            var comments = Session.Query<Comment>().Where(c => c.ArticleId == articleId).ToList();
            return comments;
        }
    }
}