using Business.CacheRepositories;
using Business.Models;
using Business.Services;
using System.Collections.Generic;

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
                articleCacheRepository.Create(article);
            }

            return article;
        }

        public void DeleteArticle(int id)
        {
            articleCacheRepository.Delete(id.ToString());
            articleService.DeleteArticle(id);
        }

        public void UpdateArticle(Article article)
        {
            articleCacheRepository.Update(article);
            articleService.UpdateArticle(article);
        }

        public void CreateArticle(Article article)
        {
            articleCacheRepository.Create(article);
            articleService.CreateArticle(article);
        }

        public IEnumerable<Article> GetArticles()
        {
            var articles = articleCacheRepository.GetItems();
            if(articles == null)
            {
                var articleRepository = articleService.Articles;
                foreach (var article in articleRepository)
                {
                    articleCacheRepository.Create(article);
                }
                return articleRepository;
            }
            else
            {
                return articles;
            }
        }

        public ArticleCollection GetArticlesBy(Criteria criteria, bool onlyVisible = false)
        {
            var articles = articleCacheRepository.GetItems();
            if (articles == null)
            {
                var articleCollection = articleService.GetArticlesBy(criteria, onlyVisible);
                foreach(var article in articleCollection)
                {
                    articleCacheRepository.Create(article);
                }
                return articleCollection;
            }
            else
            {
                articles = QueriesLogic.Filter(articles, criteria.FilterRange, onlyVisible);
                articles = QueriesLogic.Search(articles, criteria.SearchString);
                articles = QueriesLogic.Sort(articles, criteria.SortOrder);
                return articles as ArticleCollection;
            }
        }

    }
}