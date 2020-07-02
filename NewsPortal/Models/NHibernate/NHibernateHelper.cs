using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NewsPortal.Models;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

public class NHibernateHelper
{
    public static ISession OpenSession()
    {
        ISessionFactory sessionFactory = Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2012
                .ConnectionString(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NewsPortalDb;Integrated Security=True")
                .ShowSql()
            )
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Article>())
            .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
            .BuildSessionFactory();

        return sessionFactory.OpenSession();
    }
}
