using FluentNHibernate.Mapping;
using Business.Models;

namespace NHibernate.DAL.ClassMap
{
    public class CommentMap : ClassMap<Comment>
    {
        public CommentMap()
        {
            Table("Comments");
            Id(c => c.Id);
            Map(c => c.Text);
            Map(c => c.UserName);
            Map(c => c.PubDate);
            References(c => c.Article).Cascade.SaveUpdate();
        }
    }
}
