using System;
using System.Collections;
using System.Collections.Generic;

namespace NewsPortal.Models
{
    public class Comment: IEnumerable<Comment>
    {
        public virtual int Id { get; set; }
        public virtual string Text { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime? PubDate { get; set; }
        public virtual Article Article { get; set; }

        public virtual IEnumerator<Comment> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}