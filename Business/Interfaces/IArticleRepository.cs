using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        ArticlesIndex GetArticlesBy(string searchString, int sortOrder, string filterString, int page, int articlesPerPage, bool onlyVisible);
    }
}
