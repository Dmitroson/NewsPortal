using System;

namespace NewsPortal.Models
{
    public class Comment
    {
        public virtual int Id { get; set; }
        public virtual string Text { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime? PubDate { get; set; }
        public virtual Article Article { get; set; }
    }
}