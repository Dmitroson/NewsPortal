using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class CommentService
    {
        private IUnitOfWork unitOfWork;

        public CommentService()
        {
            unitOfWork = UnitOfWorkManager.GetUnitOfWork();
        }

        public IEnumerable<Comment> Comments
        {
            get
            {
                return unitOfWork.Comments.GetAll();
            }
        }

        public void CreateComment(Comment comment, int articleId)
        {
            comment.PubDate = DateTime.Now;
            comment.ArticleId = articleId;
            unitOfWork.Comments.Create(comment);
            unitOfWork.Save();
        }

        public void DeleteComment(int id)
        {
            unitOfWork.Comments.Delete(id);
            unitOfWork.Save();
        }

        public IEnumerable<Comment> GetComments(int articleId)
        {
            var comments = unitOfWork.Comments.GetCommentsBy(articleId);
            return comments;
        }

        public int GetArticleIdByCommentId(int id)
        {
            var comment = unitOfWork.Comments.Get(id);
            var articleId = comment.ArticleId;
            return articleId;
        }
    }
}