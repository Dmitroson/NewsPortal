using Business.Interfaces;
using Business.Models;
using System.Collections.Generic;

namespace Business.Lucene
{
    public interface ILuceneSearcher<T> where T : class, IEntity
    {
        ArticleCollection GetArticlesBy(Criteria criteria, bool onlyVisible);
        ArticleCollection GetArticlesBy(IEnumerable<Article> articles, Criteria criteria, bool onlyVisible);
        void Save(T item);
        void Delete(int id);
        void UpdateLuceneIndex(IEnumerable<T> items);
    }
}
