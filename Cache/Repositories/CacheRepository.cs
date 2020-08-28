using Business.Models;
using System;
using System.Linq;
using System.Runtime.Caching;

namespace Cache.Repositories
{
    public class CacheRepository<T> where T : CacheModel
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

        public void Create(IQueryable<T> items)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            foreach(var item in items)
            {
                memoryCache.Add(item.Id.ToString(), item, DateTime.Now.AddMinutes(5));
            }
        }

        public void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Remove(key);
        }

        public IQueryable<T> GetItems()
        {
            IQueryable<T> items = null;
            MemoryCache memoryCache = MemoryCache.Default;
            foreach(var key in memoryCache.AsQueryable())
            {
                try
                {
                    items.Append(memoryCache.Get(key.Key));
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