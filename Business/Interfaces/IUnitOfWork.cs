using Business.Models;
using System;

namespace Business.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IArticleRepository Articles { get; }
        ICommentRepository Comments { get; }
        void Save();
    }
}
