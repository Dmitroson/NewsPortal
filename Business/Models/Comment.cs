﻿using System;

namespace Business.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public DateTime? PubDate { get; set; }
        public int ArticleId { get; set; }
    }
}