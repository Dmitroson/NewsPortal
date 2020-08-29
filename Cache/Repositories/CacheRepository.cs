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
            memoryCache.Set(item.Id.ToString(), item, DateTime.Now.AddMinutes(5));
        }

        public void Create(T item)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Add(item.Id.ToString(), item, DateTime.Now.AddMinutes(5));
        }

        public void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Remove(key);
        }

        public IEnumerable<T> GetItems()
        {
            IEnumerable<T> items = null;
            var memoryCache = MemoryCache.Default;
            foreach(var item in memoryCache.AsEnumerable())
            {
                try
                {
                    items.Append(memoryCache.Get(item.Key));
                }
                catch
                {
                    continue;
                }
            }
            return items;
        }

    }
}