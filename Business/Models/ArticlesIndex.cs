using System.Collections.Generic;

namespace Business.Models
{
    public class ArticlesIndex
    {
        public IEnumerable<Article> Articles { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}