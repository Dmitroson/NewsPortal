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
            comment.ArticleId = articleId;
            commentService.CreateComment(comment, articleId);
            commentCacheRepository.Add(comment, $"Comments-{comment.Id}");
            commentCacheRepository.Delete($"CommentsByArticleId-{articleId}");
        }

        public void DeleteComment(int id)
        {
            var comment = commentService.GetComment(id);
            commentCacheRepository.Delete($"CommentsByArticleId-{comment.ArticleId}");
            if (commentCacheRepository.Get($"Comments-{id}") != null)
            {
                commentCacheRepository.Delete($"Comments-{id}");
            }
            commentService.DeleteComment(id);

        }

        public IEnumerable<Comment> GetComments(int articleId)
        {
            var comments = commentCacheRepository.GetItems($"CommentsByArticleId-{articleId}");
            if (comments == null)
            {
                comments = commentService.GetComments(articleId);
                commentCacheRepository.Add(comments as List<Comment>, $"CommentsByArticleId-{articleId}");
            }
                return comments;            
        }

        public int GetArticleIdByCommentId(int id)
        {
            var comment = commentCacheRepository.Get($"Comments-{id}"); 
            if (comment == null)
            {
                comment = commentService.GetComment(id);
                commentCacheRepository.Add(comment, $"Comments-{id}");
            }
            var articleId = comment.ArticleId;
            return articleId;
        }
    }
}
