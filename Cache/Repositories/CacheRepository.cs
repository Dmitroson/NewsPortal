using Business.CacheRepositories;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Cache.Repositories
{
    public class CacheRepository<T> : ICacheRepository<T> where T : CacheModel
    {
        public T Get(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            T item = memoryCache.Get(key) as T;
            return item;
        }

        public void Update(T item)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set(item.Id.ToString(), item, DateTime.Now.AddMinutes(1));
        }

        public void Create(T item)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Add(item.Id.ToString(), item, DateTime.Now.AddMinutes(1));
        }

        public void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Remove(key);
        }

        public IEnumerable<T> GetItems()
        {
            List<T> items = new List<T>();
            var memoryCache = MemoryCache.Default;
            foreach(var item in memoryCache)
            {
                if(item.Value.GetType() == typeof(T))
                    items.Add(memoryCache.Get(item.Key) as T);
            }
            return items;
        }

    }
}