using Business.Interfaces;
using System;
using System.Collections.Generic;

namespace NHibernate.DAL.Repositories
{
    public class NHBaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected ISession Session
        {
            get
            {
                return NHibernateHelper.GetSession();
            }
        }

        public void Create(T item)
        {
            Session.Save(item);
        }

        public virtual void Delete(int id)
        {
            var item = Session.Get<T>(id);
            Session.Delete(item);
        }

        public T Get(int id)
        {
            var item = Session.Get<T>(id);
            return item;
        }

        public IEnumerable<T> GetAll()
        {
            var items = Session.Query<T>();
            return items;
        }

        public virtual void Update(T item)
        {
            Session.Update(item);
        }
    }
}
