using Business.CacheRepositories;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Caching;

namespace Cache.Repositories
{
    public class CacheRepository<T> : ICacheRepository<T> where T : CacheModel
    {
        private MemoryCache memoryCache;
        private int cachingTime = Convert.ToInt32(ConfigurationManager.AppSettings.Get("cachingTimeInHours"));

        public CacheRepository(){
             memoryCache = MemoryCache.Default;
        }

        public T Get(string key)
        {
            T item = memoryCache.Get(key) as T;
            return item;
        }        

        public void Update(T item, string key)
        {
            memoryCache.Set(key, item, DateTime.Now.AddHours(cachingTime));
        }

        public void Add(T item, string key)
        {
            memoryCache.Add(key, item, DateTime.Now.AddHours(cachingTime));
        }

        public void Add(List<T> items, string key)
        {
            memoryCache.Add(key, items, DateTime.Now.AddHours(cachingTime));
        }

        public void Delete(string key)
        {
            memoryCache.Remove(key);
        }

        public IEnumerable<T> GetItems(string key)
        {
            return memoryCache.Get(key) as List<T>;
        }
    }
}