using Business.CacheRepositories;
using Business.Interfaces;
using Business.Lucene;
using Business.Models;

namespace Business.Services
{
    public static class ServiceManager
    {
        private static IUnitOfWork unitOfWork;
        private static IArticleRepository articleRepository;
        private static ICommentRepository commentRepository;
        private static ILuceneSearcher<Article> luceneSearcher;
        private static ILuceneSearcher<Article> cacheLuceneSearcher;
        private static ICacheRepository<Article> articleCacheRepository;
        private static ICacheRepository<Comment> commentCacheRepository;

        public static void SetUnitOfWork(IUnitOfWork unitOfWorkInstance)
        {
            unitOfWork = unitOfWorkInstance;
        }

        public static IUnitOfWork GetUnitOfWork()
        {
            return unitOfWork;
        }

        public static void SetArticleRepository(IArticleRepository articleRepositoryInstance)
        {
            articleRepository = articleRepositoryInstance;
        }

        public static IArticleRepository GetArticleRepository()
        {
            return articleRepository;
        }

        public static void SetCommentRepository(ICommentRepository commentRepositoryInstance)
        {
            commentRepository = commentRepositoryInstance;
        }

        public static ICommentRepository GetCommentRepository()
        {
            return commentRepository;
        }

        public static void SetLuceneSearcher(ILuceneSearcher<Article> luceneSearcherInstance)
        {
            luceneSearcher = luceneSearcherInstance;
        }

        public static ILuceneSearcher<Article> GetLuceneSearcher()
        {
            return luceneSearcher;
        }

        public static void SetCacheLuceneSearcher(ILuceneSearcher<Article> luceneSearcherInstance)
        {
            cacheLuceneSearcher = luceneSearcherInstance;
        }

        public static ILuceneSearcher<Article> GetCacheLuceneSearcher()
        {
            return cacheLuceneSearcher;
        }

        public static void SetArticleCacheRepository(ICacheRepository<Article> cacheRepositoryInstance)
        {
            articleCacheRepository = cacheRepositoryInstance;
        }

        public static ICacheRepository<Article> GetArticleCacheRepository()
        {
            return articleCacheRepository;
        }

        public static void SetCommentCacheRepository(ICacheRepository<Comment> cacheRepositoryInstance)
        {
            commentCacheRepository = cacheRepositoryInstance;
        }

        public static ICacheRepository<Comment> GetCommentCacheRepository()
        {
            return commentCacheRepository;
        }
    }
}
