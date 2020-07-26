using System;
using System.Collections.Generic;

namespace Business.Models
{
    public class Article
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual bool Visibility { get; set; }
        public virtual DateTime? PubDate { get; set; }
        public virtual List<Comment> Comments { get; set; }

        public Article()
        {
            Comments = new List<Comment>();
        }
    }
}
