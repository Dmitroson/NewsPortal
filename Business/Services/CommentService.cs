using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class CommentService
    {
        private IUnitOfWork unit;

        public CommentService(IUnitOfWork unitOfWork)
        {
            unit = unitOfWork;
        }

        public void CreateComment(Comment comment, int articleId)
        {
            comment.PubDate = DateTime.Now;
            comment.ArticleId = articleId;
            unit.Comments.Create(comment);
        }

        public void DeleteComment(int id)
        {
            unit.Comments.Delete(id);
        }

        public IEnumerable<Comment> GetComments(int articleId)
        {
            var comments = unit.Comments.GetAll().Where(c => c.ArticleId == articleId);
            return comments;
        }

        public int GetArticleIdByCommentId(int id)
        {
            var comment = unit.Comments.Get(id);
            var articleId = comment.ArticleId;
            return articleId;
        }
    }
}