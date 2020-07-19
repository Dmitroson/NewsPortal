using Business.Models;
using System.Collections.Generic;

namespace NewsPortal.Models
{
    public class ArticleIndexViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public PageInfo PageInfo { get; set; }
    }
} 