using Business.CacheRepositories;
using Business.Models;
using Business.Services;
using System.Collections.Generic;
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
            articleService.UpdateArticle(article);
            articleCacheRepository.Update(article);
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
            var articleCollection = new ArticleCollection();
            var articles = articleCacheRepository.GetItems();
            if (articles.Count() == 0)
            {
                articles.Concat(articleService.Articles);
                foreach (var article in articles)
                {
                    articleCacheRepository.Create(article);
                }
                articleCollection = articleService.GetArticlesBy(criteria, onlyVisible);
                return articleCollection;
            }
            else
            {
                articleCollection.TotalItems = articles.Count();
                articles = articles.Skip((criteria.Page) * criteria.ArticlesPerPage).Take(criteria.ArticlesPerPage);
                articleCollection.AddItems(articles);
                return articleCollection;
            }
        }

    }
}