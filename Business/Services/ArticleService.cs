using Business.Interfaces;
using Business.Lucene;
using Business.Models;
using System.Collections.Generic;

namespace Business.Services
{
    public class ArticleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Article> articleRepository;
        private readonly ILuceneSearcher<Article> luceneSearcher;

        public ArticleService()
        {
            unitOfWork = ServiceManager.GetUnitOfWork();
            articleRepository = ServiceManager.GetArticleRepository();
            luceneSearcher = ServiceManager.GetLuceneSearcher();
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
            var articles = luceneSearcher.GetArticlesBy(criteria, onlyVisible);
            return articles;
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
            luceneSearcher.Save(article);
            unitOfWork.Commit();
        }

        public void DeleteArticle(int id)
        {
            unitOfWork.OpenSession();

            articleRepository.Delete(id);
            luceneSearcher.Delete(id);
            unitOfWork.Commit();
        }

        public void UpdateArticle(Article article)
        {
            unitOfWork.OpenSession();

            articleRepository.Update(article);
            luceneSearcher.Save(article);
            unitOfWork.Commit();
        }

        public void UpdateLuceneIndex()
        {
            luceneSearcher.UpdateLuceneIndex(Articles);
        }
    }
}