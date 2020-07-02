using FluentNHibernate.Mapping;

namespace NewsPortal.Models.ClassMap
{
    public class ImageMap : ClassMap<Image>
    {
        public ImageMap()
        {
            Table("Images");
            Id(i => i.Id);
            Map(i => i.UrlMini);
            Map(i => i.UrlFullSize);
            HasOne(i => i.Article);
        }
    }
}