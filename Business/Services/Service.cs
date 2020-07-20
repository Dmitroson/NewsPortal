using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class Service : IService
    {
        private IUnitOfWork unity;

        public Service(IUnitOfWork unitOfWork)
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

        public void CreateComment(Comment comment, int articleId)
        {
            var article = unity.Articles.Get(articleId);
            comment.PubDate = DateTime.Now;
            comment.Article = article;
            article.Comments.Add(comment);
            unity.Comments.Create(comment);
        }

        public void DeleteArticle(int id)
        {
            unity.Articles.Delete(id);
        }

        public void DeleteComment(int id)
        {
            unity.Comments.Delete(id);
        }

        public IEnumerable<Comment> GetComments(int articleId)
        {
            var article = unity.Articles.Get(articleId);
            var comments = article.Comments.ToList();
            return comments;
        }

        public void UpdateArticle(Article article)
        {
            article.PubDate = article.PubDate.Value.AddHours(-3);
            unity.Articles.Update(article); ;
        }

        public int GetArticleIdByCommentId(int id)
        {
            var comment = unity.Comments.Get(id);
            var articleId = comment.Article.Id;
            return articleId;
        }

        public IQueryable<Article> Filter(IQueryable<Article> articles, string filterString)
        {
            DateFilter filter = new DateFilter(filterString);
            articles = filter.FilterByDate(articles);
            return articles;
        }

        public IQueryable<Article> Search(IQueryable<Article> articles, string searchString)
        {
            if (searchString != "")
            {
                articles = articles.Where(a => a.Title.Contains(searchString)
                                            || a.Description.Contains(searchString));
            }
            return articles;
        }

        public IQueryable<Article> Sort(IQueryable<Article> articles, string order)
        {
            switch (order)
            {
                case "Title":
                    articles = articles.OrderBy(a => a.Title);
                    break;
                case "Description":
                    articles = articles.OrderBy(a => a.Description);
                    break;
                default:
                    articles = articles.OrderByDescending(a => a.PubDate);
                    break;
            }
            return articles;
        }

        public void ParseParams(string paramsString, out string searchString, out string filterString)
        {
            searchString = "";
            filterString = "";
            string[] paramsArray = paramsString.Split('&');

            switch (paramsArray.Length)
            {
                case 1:
                    int foundIndex = paramsArray[0].IndexOf("=");
                    if (paramsArray[0].Contains("searchString"))
                    {
                        searchString = paramsArray[0].Substring(foundIndex + 1);
                        filterString = "";
                    }
                    else
                    {
                        filterString = paramsArray[0].Substring(foundIndex + 1);
                        searchString = "";
                    }
                    break;
                case 2:
                    int foundIndex1 = paramsArray[0].IndexOf("=");
                    searchString = paramsArray[0].Substring(foundIndex1 + 1);

                    int foundIndex2 = paramsArray[1].IndexOf("=");
                    filterString = paramsArray[1].Substring(foundIndex2 + 1);
                    break;
            }
        }

        public ArticlesIndex MakePaging(IQueryable<Article> articles, int page)
        {
            var articlesList = articles.ToList();
            int pageSize = 10;

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
