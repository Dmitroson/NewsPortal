using Business.Models;

namespace Business.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        ArticleCollection GetArticlesBy(Criteria criteria, bool onlyVisible);
    }
}
