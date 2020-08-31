using Business.Models;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lucene
{
    static class LuceneExtentions
    {
        internal static void Fill(this Document document, Article article)
        {
            document.Add(new Field("Id", article.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            document.Add(new Field("Title", article.Title, Field.Store.YES, Field.Index.ANALYZED));

            var clearDescription = Regex.Replace(article.Description, @"<[^>]*>", string.Empty);
            document.Add(new Field("Description", clearDescription, Field.Store.YES, Field.Index.ANALYZED));

            document.Add(new Field("ImageUrl", article.ImageUrl, Field.Store.YES, Field.Index.NO));

            document.Add(new Field("Visibility", article.Visibility.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            var pubDate = DateTools.DateToString(article.PubDate ?? DateTime.MinValue, DateTools.Resolution.SECOND);
            document.Add(new Field("PubDate", pubDate, Field.Store.YES, Field.Index.NOT_ANALYZED));
        }

        internal static IEnumerable<Article> ToArticlesList(this IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            var articlesList = new List<Article>();
            foreach (var hit in hits)
            {
                var articleDocument = searcher.Doc(hit.Doc);
                articlesList.Add(articleDocument.ToArticle());
            }

            return articlesList;
        }

        internal static ArticleCollection ToArticleCollection(this IEnumerable<ScoreDoc> hits, IndexSearcher searcher, Criteria criteria, bool onlyVisible)
        {
            var articles = hits.ToArticlesList(searcher);

            if (onlyVisible)
                articles = articles.GetVisibleArticles();

            var articleCollection = new ArticleCollection
            {
                TotalItems = articles.Count()
            };

            articles = articles.Skip((criteria.Page) * criteria.ArticlesPerPage).Take(criteria.ArticlesPerPage);

            articleCollection.AddItems(articles);
            return articleCollection;
        }

        internal static ArticleCollection ToArticleCollection(this IEnumerable<Article> articles, Criteria criteria)
        {
            var articleCollection = new ArticleCollection
            {
                TotalItems = articles.Count()
            };

            articles = articles.Skip((criteria.Page) * criteria.ArticlesPerPage).Take(criteria.ArticlesPerPage);

            articleCollection.AddItems(articles);
            return articleCollection;
        }

        internal static Article ToArticle(this Document document)
        {
            var article = new Article
            {
                Id = int.Parse(document.Get("Id")),
                Title = document.Get("Title"),
                Description = document.Get("Description"),
                ImageUrl = document.Get("ImageUrl"),
                Visibility = bool.Parse(document.Get("Visibility")),
                PubDate = DateTools.StringToDate(document.Get("PubDate"))
            };

            return article;
        }

        internal static List<Article> GetVisibleArticles(this IEnumerable<Article> articles)
        {
            articles = articles.Where(a => a.PubDate <= DateTime.Now && a.Visibility == true);
            return articles.ToList();
        }
    }
}
