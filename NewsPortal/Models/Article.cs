﻿using System;
using System.Collections.Generic;

namespace NewsPortal.Models
{
    public class Article
    {
        public Article()
        {
            Comments = new HashSet<Comment>();
        }

        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual bool Visibility { get; set; }
        public virtual DateTime? PubDate { get; set; }
        public virtual ISet<Comment> Comments { get; set; }
    }
}