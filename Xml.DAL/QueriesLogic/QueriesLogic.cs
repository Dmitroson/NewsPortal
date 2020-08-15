using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Xml.DAL
{
    public class QueriesLogic
    {
        public static IEnumerable<Article> Filter(IEnumerable<Article> articles, DateRange range, bool onlyVisible)
        {
            articles = articles.Where(a => range.Includes(a.PubDate.Value));
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

        public static IEnumerable<Article> Sort(IEnumerable<Article> articles, SortOrder order)
        {
            switch (order)
            {
                case SortOrder.DateAscending:
                    articles = articles.OrderBy(a => a.PubDate);
                    break;
                case SortOrder.DateDescending:
                    articles = articles.OrderByDescending(a => a.PubDate);
                    break;
                case SortOrder.TitleAscending:
                    articles = articles.OrderBy(a => a.Title);
                    break;
                case SortOrder.TitleDescending:
                    articles = articles.OrderByDescending(a => a.Title);
                    break;
                case SortOrder.DescriptionAscending:
                    articles = articles.OrderBy(a => a.Description);
                    break;
                case SortOrder.DescriptionDescending:
                    articles = articles.OrderByDescending(a => a.Description);
                    break;
            }
            return articles;
        }
    }
}
