using Business.Interfaces;
using System;
using System.Collections.Generic;

namespace Business.Models
{
    public class Article : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool Visibility { get; set; }
        public DateTime? PubDate { get; set; }
        public IList<Comment> Comments { get; set; }

        public Article()
        {
            Comments = new List<Comment>();
        }
    }
}
