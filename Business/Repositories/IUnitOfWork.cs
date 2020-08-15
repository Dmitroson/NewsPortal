using System;

namespace Business.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void OpenSession();
        void Commit();
        void Rollback();
    }
}