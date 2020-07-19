using Business.Models;
using FluentNHibernate.Mapping;

namespace NHibernate.DAL.ClassMap
{
    public class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            Table("Articles");
            Id(a => a.Id);
            Map(a => a.Title);
            Map(a => a.Description);
            Map(a => a.ImageUrl);
            Map(a => a.Visibility);
            Map(a => a.PubDate);
            HasMany(a => a.Comments).Inverse().Cascade.All();
        }
    }
}
