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
                //.ConnectionString(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=NewsPortalDb;Integrated Security=True")
                //.ConnectionString(@"Data Source=testnewsportal20200702113407dbserver.database.windows.net;Initial Catalog=NewsPortal20200702133310_db;User ID=Dmitroson;Password=#include_Root;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
                .ShowSql()
            )
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Article>())
            .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
            .BuildSessionFactory();

        return sessionFactory.OpenSession();
    }
}
