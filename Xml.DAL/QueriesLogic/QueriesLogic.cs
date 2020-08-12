using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Xml.DAL
{
    public class QueriesLogic
    {
        public static IEnumerable<Article> Filter(IEnumerable<Article> articles, string filterString, bool onlyVisible)
        {
            DateFilter filter = new DateFilter(filterString);
            articles = filter.FilterByDate(articles);
            if (onlyVisible)
            {
                articles = articles.Where(a => a.PubDate <= DateTime.Now && a.Visibility == true);
            }
            return articles;
        }

        public static IEnumerable<Article> Search(IEnumerable<Article> articles, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(a => a.Title.Contains(searchString)
                                            || a.Description.Contains(searchString));
            }
            return articles;
        }

        public static IEnumerable<Article> Sort(IEnumerable<Article> articles, int order)
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
    }
}
