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

        public ArticleServiceWeb()
        {
            articleService = new ArticleService();
            articleCacheRepository = ServiceManager.GetArticleCacheRepository();
        }

        public Article GetArticle(int id)
        {
            Article article = articleCacheRepository.Get(id.ToString());

            if(article == null)
            {
                article = articleService.GetArticle(id);
                articleCacheRepository.Add(article, $"News-{article.Id}");
            }

            return article;
        }

        public void DeleteArticle(int id)
        {
            articleCacheRepository.Delete($"News-{id}");
            articleService.DeleteArticle(id);
        }

        public void UpdateArticle(Article article)
        {
            articleService.UpdateArticle(article);
            articleCacheRepository.Update(article, $"News-{article.Id}");
        }

        public void CreateArticle(Article article)
        {
            articleCacheRepository.Add(article, $"News-{article.Id}");
            articleService.CreateArticle(article);
        }

        public ArticleCollection GetArticlesBy(Criteria criteria, bool onlyVisible = false)
        {
            var articles = articleCacheRepository.GetItems($"NewsByCriteria-{criteria.FilterRange}-{criteria.SearchString}-{criteria.SortOrder}-{criteria.Page}");
            var articleCollection = new ArticleCollection();
            if (articles == null)
            {
                articleCollection = articleService.GetArticlesBy(criteria, onlyVisible);
                articleCacheRepository.Add(articleCollection.ToList(), $"NewsByCriteria-{criteria.FilterRange}-{criteria.SearchString}-{criteria.SortOrder}-{criteria.Page}-{onlyVisible}");
            }
            else
            {
                articleCollection.AddItems(articles);
            }

            return articleCollection;
        }
    }
}