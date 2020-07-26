using Business.Models;

namespace Business.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Article> Articles { get; }
        IRepository<Comment> Comments { get; }
    }
}
