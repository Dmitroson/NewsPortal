using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.DAL.ClassMap;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Util;
using System.Configuration;
using System.Linq;
using System.Xml;

public class NHibernateHelper
{
    private static ISessionFactory sessionFactory;
    private static object sessionFactoryLock = new object();
    public static string ConnectionString { get; set; }

    private static ISessionFactory CreateSessionFactory()
    {
        var configuration = new NHibernate.Cfg.Configuration().SetProperty(Environment.UseProxyValidator, bool.FalseString);

        sessionFactory = Fluently.Configure(configuration)
            .Database(MsSqlConfiguration.MsSql2012
            .ConnectionString(ConnectionString)
            .ShowSql()
            )
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ArticleMap>())
            .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
            .BuildSessionFactory();

        return sessionFactory;
    }

    public static ISession OpenSession()
    {
        lock (sessionFactoryLock)
        {
            if(sessionFactory == null)
            {
                sessionFactory = CreateSessionFactory();
            }
        }
        return sessionFactory.OpenSession();
    }
}