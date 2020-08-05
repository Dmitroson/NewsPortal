using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class ArticleService
    {
        private IUnitOfWork unitOfWork;

        public ArticleService()
        {
            unitOfWork = UnitOfWorkManager.GetUnitOfWork();
        }

        public IEnumerable<Article> Articles
        {
            get
            {
                return unitOfWork.Articles.GetAll();
            }
        }

        public ArticleCollection GetArticlesBy(Criteria criteria, int articlesPerPage, bool onlyVisible = false)
        {
            var articlesIndex = unitOfWork.Articles.GetArticlesBy(criteria, articlesPerPage, onlyVisible);
            return articlesIndex;
        }

        public Article GetArticle(int id)
        {
            return unitOfWork.Articles.Get(id);
        }

        public void CreateArticle(Article article)
        {            
            unitOfWork.Articles.Create(article);
            unitOfWork.Save();
        }

        public void DeleteArticle(int id)
        {
            unitOfWork.Articles.Delete(id);
            unitOfWork.Save();
        }

        public void UpdateArticle(Article article)
        {
            unitOfWork.Articles.Update(article);
            unitOfWork.Save();
        }
    }
}