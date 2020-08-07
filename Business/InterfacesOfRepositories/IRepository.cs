using System.Collections.Generic;

namespace Business.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }

    public interface IEntity
    {
        int Id { get; }
    }
}
