using Business.Interfaces;
using Business.Models;
using System.Collections.Generic;

namespace Business.Services
{
    public class ArticleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IArticleRepository articleRepository;

        public ArticleService()
        {
            unitOfWork = ServiceManager.GetUnitOfWork();
            articleRepository = ServiceManager.GetArticleRepository();
        }

        public IEnumerable<Article> Articles
        {
            get
            {
                return articleRepository.GetAll();
            }
        }

        public ArticleCollection GetArticlesBy(Criteria criteria, bool onlyVisible = false)
        {
            var articlesIndex = articleRepository.GetArticlesBy(criteria, onlyVisible);
            return articlesIndex;
        }

        public Article GetArticle(int id)
        {
            return articleRepository.Get(id);
        }

        public void CreateArticle(Article article)
        {
            articleRepository.Create(article);
            unitOfWork.Commit();
        }

        public void DeleteArticle(int id)
        {
            articleRepository.Delete(id);
            unitOfWork.Commit();
        }

        public void UpdateArticle(Article article)
        {
            articleRepository.Update(article);
            unitOfWork.Commit();
        }
    }
}