using Business.CacheRepositories;
using Business.Models;
using Business.Services;
using System.Collections.Generic;
using System.Linq;

namespace Cache.Services
{
    public class CommentServiceWeb
    {
        private readonly CommentService commentService;
        private readonly ICacheRepository<Comment> commentCacheRepository;

        public CommentServiceWeb()
        {
            commentService = new CommentService();
            commentCacheRepository = ServiceManager.GetCommentCacheRepository();
        }

        public void CreateComment(Comment comment, int articleId)
        {
            commentCacheRepository.Create(comment);
            commentService.CreateComment(comment, articleId);
        }

        public void DeleteComment(int id)
        {
            commentCacheRepository.Delete(id.ToString());
            commentService.DeleteComment(id);
        }

        public IEnumerable<Comment> GetComments(int articleId)
        {
            var comments = commentCacheRepository.GetItems();
            if (comments.Count() == 0)
            {
                var commentsList = commentService.GetComments(articleId);
                foreach (var comment in commentsList)
                {
                    commentCacheRepository.Create(comment);
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
            var comment = commentCacheRepository.Get(id.ToString());
            var articleId = comment.ArticleId;
            return articleId;
        }
    }
}
