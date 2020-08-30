using Business.Models;
using System.Collections.Generic;

namespace Business.CacheRepositories
{
    public interface ICacheRepository<T> where T : CacheModel
    {
        T Get(string key);
        void Update(T item);
        void Add(T item);
        void Delete(string key);
        IEnumerable<T> GetItems();
    }
}
