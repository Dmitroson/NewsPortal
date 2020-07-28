using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class ArticleService
    {
        private IUnitOfWork unity;

        public ArticleService(IUnitOfWork unitOfWork)
        {
            unity = unitOfWork;
        }

        public IQueryable<Article> Articles
        {
            get
            {
                return unity.Articles.GetAll();
            }
        }

        public ArticlesIndex GetArticlesBy(string searchString, int sortOrder, string filterString, int page, int articlesPerPage, bool onlyVisible = false)
        {
            var articlesIndex = unity.Articles.GetArticlesBy(searchString, sortOrder, filterString, page, articlesPerPage, onlyVisible);
            return articlesIndex;
        }

        public Article GetArticle(int id)
        {
            return unity.Articles.Get(id);
        }

        public void CreateArticle(Article article)
        {
            if (article.PubDate == null)
            {
                article.PubDate = DateTime.Now;
            }
            else
            {
                article.PubDate = article.PubDate.Value.AddHours(-3);
            }

            unity.Articles.Create(article);
        }

        public void DeleteArticle(int id)
        {
            unity.Articles.Delete(id);
        }

        public void UpdateArticle(Article article)
        {
            article.PubDate = article.PubDate.Value.AddHours(-3);
            unity.Articles.Update(article);
        }

        public static IQueryable<Article> Filter(IQueryable<Article> articles, string filterString, bool onlyVisible)
        {
            DateFilter filter = new DateFilter(filterString);
            articles = filter.FilterByDate(articles);
            if (onlyVisible)
            {
                articles = articles.Where(a => a.PubDate <= DateTime.Now.AddHours(3) && a.Visibility == true);
            }
            return articles;
        }

        public static IQueryable<Article> Search(IQueryable<Article> articles, string searchString)
        {
            if (searchString != "")
            {
                articles = articles.Where(a => a.Title.Contains(searchString)
                                            || a.Description.Contains(searchString));
            }
            return articles;
        }

        public static IQueryable<Article> Sort(IQueryable<Article> articles, int order)
        {
            switch (order)
            {
                case 1:
                    articles = articles.OrderByDescending(a => a.PubDate);
                    break;
                case 2:
                    articles = articles.OrderBy(a => a.Title);
                    break;
                case 3:
                    articles = articles.OrderBy(a => a.Description);
                    break;
            }
            return articles;
        }

        public static ArticlesIndex MakePaging(IQueryable<Article> articles, int page, int pageSize)
        {
            var articlesList = articles.ToList();

            IEnumerable<Article> articlesPerPages = articlesList.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = articlesList.Count
            };
            ArticlesIndex articlesIndex = new ArticlesIndex
            {
                Articles = articlesPerPages,
                PageInfo = pageInfo
            };
            return articlesIndex;
        }
    }
}
