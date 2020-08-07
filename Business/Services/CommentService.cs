using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;

namespace Business.Services
{
    public class CommentService
    {
        private readonly IUnitOfWork unitOfWork;
        private ICommentRepository commentRepository;

        public CommentService()
        {
            unitOfWork = ServiceManager.GetUnitOfWork();
            commentRepository = ServiceManager.GetCommentRepository();
        }

        public IEnumerable<Comment> Comments
        {
            get
            {
                return commentRepository.GetAll();
            }
        }

        public void CreateComment(Comment comment, int articleId)
        {
            comment.PubDate = DateTime.Now;
            comment.ArticleId = articleId;
            commentRepository.Create(comment);
            unitOfWork.Commit();
        }

        public void DeleteComment(int id)
        {
            commentRepository.Delete(id);
            unitOfWork.Commit();
        }

        public IEnumerable<Comment> GetComments(int articleId)
        {
            var comments = commentRepository.GetCommentsBy(articleId);
            return comments;
        }

        public int GetArticleIdByCommentId(int id)
        {
            var comment = commentRepository.Get(id);
            var articleId = comment.ArticleId;
            return articleId;
        }
    }
}