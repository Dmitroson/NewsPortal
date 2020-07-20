using Business.Models;
using System.Collections.Generic;
using System.Linq;

namespace Business.Interfaces
{
    public interface IService
    {
        IQueryable<Article> Articles { get; }
        Article GetArticle(int id);
        void CreateArticle(Article article);
        void UpdateArticle(Article article);
        void DeleteArticle(int id);
        IEnumerable<Comment> GetComments(int articleId);
        void CreateComment(Comment comment, int articleId);
        void DeleteComment(int id);
        int GetArticleIdByCommentId(int id);
    }
}
