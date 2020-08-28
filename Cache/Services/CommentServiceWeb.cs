using Business.Models;
using Business.Services;
using Cache.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cache.Services
{
    public class CommentServiceWeb
    {
        private CommentService commentService;
        private CacheRepository<Comment> cacheRepository;

        public CommentServiceWeb()
        {
            commentService = new CommentService();
            cacheRepository = new CacheRepository<Comment>();
        }

        public void CreateComment(Comment comment, int articleId)
        {
            cacheRepository.Create(comment);
            commentService.CreateComment(comment, articleId);
        }

        public void DeleteComment(int id)
        {
            cacheRepository.Delete(id.ToString());
            commentService.DeleteComment(id);
        }

        public IEnumerable<Comment> GetComments(int articleId)
        {
            var comments = cacheRepository.GetItems();
            if (comments == null)
            {
                var commentsList = commentService.GetComments(articleId);
                foreach (var comment in commentsList)
                {
                    cacheRepository.Create(comment);
                }
                return commentsList;
            }
            else
            {
                comments = comments.Where(comment => comment.ArticleId == articleId);
                return comments;
            }
        }

        public int GetArticleIdByCommentId(int id)
        {
            var comment = cacheRepository.Get(id.ToString());
            var articleId = comment.ArticleId;
            return articleId;
        }
    }
}
