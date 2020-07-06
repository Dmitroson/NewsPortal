using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Models
{
    public class ArticleIndexViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public PageInfo PageInfo { get; set; }
    }
} 