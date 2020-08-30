using Business.CacheRepositories;
using Business.Lucene;
using Business.Models;
using Business.Services;
using System.Linq;

namespace Cache.Services
{
    public class ArticleServiceWeb
    {
        private readonly ArticleService articleService;
        private readonly ICacheRepository<Article> articleCacheRepository;
        private readonly ILuceneSearcher<Article> cacheLuceneSearcher;

        public ArticleServiceWeb()
        {
            articleService = new ArticleService();
            articleCacheRepository = ServiceManager.GetArticleCacheRepository();
            cacheLuceneSearcher = ServiceManager.GetCacheLuceneSearcher();
        }

        public Article GetArticle(int id)
        {
            Article article = articleCacheRepository.Get(id.ToString());

            if(article == null)
            {
                article = articleService.GetArticle(id);
                articleCacheRepository.Add(article);
            }

            return article;
        }

        public void DeleteArticle(int id)
        {
            articleCacheRepository.Delete(id.ToString());
            articleService.DeleteArticle(id);
            cacheLuceneSearcher.Delete(id);
        }

        public void UpdateArticle(Article article)
        {
            articleService.UpdateArticle(article);
            articleCacheRepository.Update(article);
            cacheLuceneSearcher.Save(article);
        }

        public void CreateArticle(Article article)
        {
            articleCacheRepository.Add(article);
            articleService.CreateArticle(article);
            cacheLuceneSearcher.Save(article);
        }

        public ArticleCollection GetArticlesBy(Criteria criteria, bool onlyVisible = false)
        {
            var articleCollection = new ArticleCollection();
            var articles = articleCacheRepository.GetItems();
            if (articles.Count() == 0)
            {
                articles = articleService.Articles;
                foreach (var article in articles)
                {
                    articleCacheRepository.Add(article);
                }
               
                articleCollection = articleService.GetArticlesBy(criteria, onlyVisible);
            }
            else
            {
                articleCollection = cacheLuceneSearcher.GetArticlesBy(articles, criteria, onlyVisible);
            }

            return articleCollection;
        }
    }
}