using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class CommentService
    {
        private IUnitOfWork unity;

        public CommentService(IUnitOfWork unitOfWork)
        {
            unity = unitOfWork;
        }

        public void CreateComment(Comment comment, int articleId)
        {
            comment.PubDate = DateTime.Now;
            comment.ArticleId = articleId;
            unity.Comments.Create(comment);
        }

        public void DeleteComment(int id)
        {
            unity.Comments.Delete(id);
        }

        public IEnumerable<Comment> GetComments(int articleId)
        {
            var comments = unity.Comments.GetAll().Where(c => c.ArticleId == articleId);
            return comments;
        }

        public int GetArticleIdByCommentId(int id)
        {
            var comment = unity.Comments.Get(id);
            var articleId = comment.ArticleId;
            return articleId;
        }
    }
}