using FluentNHibernate.Mapping;

namespace NewsPortal.Models.ClassMap
{
    public class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            Table("Articles");
            Id(a => a.Id);
            Map(a => a.Title);
            Map(a => a.Description);
            Map(a => a.Visibility);
            Map(a => a.PubDate);
            HasOne(a => a.Image).Cascade.All().Constrained();
            HasMany(a => a.Comments).Inverse();
        }
    }
}