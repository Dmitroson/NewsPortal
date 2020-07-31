using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class ArticleService
    {
        private IUnitOfWork unit;

        public ArticleService(IUnitOfWork unitOfWork)
        {
            unit = unitOfWork;
        }

        public IQueryable<Article> Articles
        {
            get
            {
                return unit.Articles.GetAll();
            }
        }

        public ArticlesIndex GetArticlesBy(string searchString, int sortOrder, string filterString, int page, int articlesPerPage, bool onlyVisible = false)
        {
            var articlesIndex = unit.Articles.GetArticlesBy(searchString, sortOrder, filterString, page, articlesPerPage, onlyVisible);
            return articlesIndex;
        }

        public Article GetArticle(int id)
        {
            return unit.Articles.Get(id);
        }

        public void CreateArticle(Article article)
        {            
            unit.Articles.Create(article);
        }

        public void DeleteArticle(int id)
        {
            unit.Articles.Delete(id);
        }

        public void UpdateArticle(Article article)
        {
            unit.Articles.Update(article);
        }

        public static IQueryable<Article> Filter(IQueryable<Article> articles, string filterString, bool onlyVisible)
        {
            DateFilter filter = new DateFilter(filterString);
            articles = filter.FilterByDate(articles);
            if (onlyVisible)
            {
                articles = articles.Where(a => a.PubDate <= DateTime.Now && a.Visibility == true);
            }
            return articles;
        }

        public static IQueryable<Article> Search(IQueryable<Article> articles, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
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
                    articles = articles.OrderBy(a => a.PubDate);
                    break;
                case 2:
                    articles = articles.OrderByDescending(a => a.PubDate);
                    break;
                case 3:
                    articles = articles.OrderBy(a => a.Title);
                    break;
                case 4:
                    articles = articles.OrderByDescending(a => a.Title);
                    break;
                case 5:
                    articles = articles.OrderBy(a => a.Description);
                    break;
                case 6:
                    articles = articles.OrderByDescending(a => a.Description);
                    break;
            }
            return articles;
        }

        public static ArticlesIndex MakePaging(IQueryable<Article> articles, int page, int pageSize)
        {
            var articlesList = articles.ToList();

            IEnumerable<Article> articlesPerPages = articlesList.Skip((page) * pageSize).Take(pageSize);
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
