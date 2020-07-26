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
            var article = unity.Articles.Get(articleId);
            comment.PubDate = DateTime.Now;
            comment.Article = article;
            article.Comments.Add(comment);
            unity.Comments.Create(comment);
        }

        public void DeleteComment(int id)
        {
            unity.Comments.Delete(id);
        }

        public IEnumerable<Comment> GetComments(int articleId)
        {
            var article = unity.Articles.Get(articleId);
            article.Comments = unity.Comments.GetAll().Where(c => c.Article.Id == articleId).ToList<Comment>();
            var comments = article.Comments;
            return comments;
        }

        public int GetArticleIdByCommentId(int id)
        {
            var comment = unity.Comments.Get(id);
            var articleId = comment.Article.Id;
            return articleId;
        }
    }
}
