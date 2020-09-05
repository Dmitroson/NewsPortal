using Business.Models;
using System.Collections.Generic;

namespace Business.CacheRepositories
{
    public interface ICacheRepository<T> where T : CacheModel
    {
        T Get(string key);
        void Update(T item, string key);
        void Add(T item, string key);
        void Add(List<T> items, string key);
        void Delete(string key);
        IEnumerable<T> GetItems(string key);
    }
}
