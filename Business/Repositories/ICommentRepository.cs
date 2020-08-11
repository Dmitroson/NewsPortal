using Business.Models;
using System.Collections.Generic;

namespace Business.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IEnumerable<Comment> GetCommentsBy(int articleId);
    }
}
