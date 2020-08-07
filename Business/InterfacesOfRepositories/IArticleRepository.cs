using Business.Models;

namespace Business.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        ArticleCollection GetArticlesBy(Criteria criteria, int articlesPerPage, bool onlyVisible);
    }
}
