using Business.Models;

namespace Business.Interfaces
{
    public interface IUnitOfWork
    {
        IArticleRepository Articles { get; }
        IRepository<Comment> Comments { get; }
    }
}
