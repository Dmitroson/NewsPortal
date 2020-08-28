using Business.Models;
using Business.Services;
using Cache.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Cache.Services
{
    public class ArticleServiceWeb
    {
        private ArticleService articleService;
        private CacheRepository<Article> cacheRepository;

        public ArticleServiceWeb()
        {
            articleService = new ArticleService();
            cacheRepository = new CacheRepository<Article>();
        }

        public Article GetArticle(int id)
        {
            Article article = cacheRepository.Get(id.ToString());

            if(article == null)
            {
                article = articleService.GetArticle(id);
                cacheRepository.Create(article);
            }

            return article;
        }

        public void DeleteArticle(int id)
        {
            cacheRepository.Delete(id.ToString());
            articleService.DeleteArticle(id);
        }

        public void UpdateArticle(Article article)
        {
            cacheRepository.Update(article);
            articleService.UpdateArticle(article);
        }

        public void CreateArticle(Article article)
        {
            cacheRepository.Create(article);
            articleService.CreateArticle(article);
        }

        public IEnumerable<Article> GetArticles()
        {
            var articles = cacheRepository.GetItems();
            if(articles == null)
            {
                var articleRepository = articleService.Articles;
                foreach (var article in articleRepository)
                {
                    cacheRepository.Create(article);
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
            IQueryable<Article> articles;
            articles = cacheRepository.GetItems();
            if (articles == null)
            {
                var articleCollection = articleService.GetArticlesBy(criteria, onlyVisible);
                foreach(var article in articleCollection)
                {
                    cacheRepository.Create(article);
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