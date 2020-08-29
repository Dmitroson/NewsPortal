using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.CacheRepositories
{
    public interface ICacheRepository<T> where T : CacheModel
    {
        T Get(string key);
        void Update(T item);
        void Create(T item);
        void Delete(string key);
        IEnumerable<T> GetItems();
    }
}
