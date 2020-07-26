using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.DAL.ClassMap;
using NHibernate.Tool.hbm2ddl;

public class NHibernateHelper
{
    public static ISession OpenSession()
    {
        ISessionFactory sessionFactory = Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2012
            .ConnectionString(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NewsPortalDb;Integrated Security=True")
            //.ConnectionString(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=NewsPortalDb;Integrated Security=True")
            //.ConnectionString(@"Data Source=tcp:newsportal20200705224151dbserver.database.windows.net,1433;Initial Catalog=NewsPortal_db;User Id=Dmitroson@newsportal20200705224151dbserver;Password=#include_Root")
            .ShowSql()
            )
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ArticleMap>())
            .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
            .BuildSessionFactory();

        return sessionFactory.OpenSession();
    }
}