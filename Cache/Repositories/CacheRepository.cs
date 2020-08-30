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
        private MemoryCache memoryCache;

        public CacheRepository(){
             memoryCache = MemoryCache.Default;
        }

        public T Get(string key)
        {
            T item = memoryCache.Get(key) as T;
            return item;
        }

        public void Update(T item)
        {
            memoryCache.Set(item.Id.ToString(), item, DateTime.Now.AddMinutes(1));
        }

        public void Add(T item)
        {
            memoryCache.Add(item.Id.ToString(), item, DateTime.Now.AddMinutes(1));
        }

        public void Delete(string key)
        {
            memoryCache.Remove(key);
        }

        public IEnumerable<T> GetItems()
        {
            List<T> items = new List<T>();
            foreach(var item in memoryCache)
            {
                if(item.Value.GetType() == typeof(T))
                    items.Add(memoryCache.Get(item.Key) as T);
            }
            return items;
        }
    }
}