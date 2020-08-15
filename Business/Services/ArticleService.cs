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
                unitOfWork.OpenSession();
                return articleRepository.GetAll();
            }
        }

        public ArticleCollection GetArticlesBy(Criteria criteria, bool onlyVisible = false)
        {
            unitOfWork.OpenSession();
            var articlesIndex = articleRepository.GetArticlesBy(criteria, onlyVisible);
            return articlesIndex;
        }

        public Article GetArticle(int id)
        {
            unitOfWork.OpenSession();
            return articleRepository.Get(id);
        }

        public void CreateArticle(Article article)
        {
            unitOfWork.OpenSession();

            articleRepository.Create(article);
            unitOfWork.Commit();
        }

        public void DeleteArticle(int id)
        {
            unitOfWork.OpenSession();

            articleRepository.Delete(id);
            unitOfWork.Commit();
        }

        public void UpdateArticle(Article article)
        {
            unitOfWork.OpenSession();

            articleRepository.Update(article);
            unitOfWork.Commit();
        }
    }
}