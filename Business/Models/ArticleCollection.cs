﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business.Models
{
    public class ArticleCollection : Collection<Article>
    {
        public int TotalItems { get; set; }

        public void AddItems(IEnumerable<Article> articles)
        {
            foreach (var article in articles)
            {
                Items.Add(article);
            }
        }
    }
}
