using FluentNHibernate.Mapping;

namespace NewsPortal.Models.ClassMap
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