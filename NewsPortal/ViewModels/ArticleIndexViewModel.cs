﻿using Business.Models;
using NewsPortal.Helpers;
using System.Collections.Generic;

namespace NewsPortal.ViewModels
{
    public class ArticleIndexViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public PageInfo PageInfo { get; set; }
    }
} 