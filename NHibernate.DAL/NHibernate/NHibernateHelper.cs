using Business.Services;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.DAL.ClassMap;
using NHibernate.DAL.Repositories;
using NHibernate.Tool.hbm2ddl;

public class NHibernateHelper
{
    private static ISessionFactory sessionFactory;
    private static object sessionFactoryLock = new object();
    public static NHUnitOfWork UnitOfWork { get; set; }
    public static string ConnectionString { get; set; }

    private static ISessionFactory CreateSessionFactory()
    {
        var configuration = new Configuration().SetProperty(Environment.UseProxyValidator, bool.FalseString);

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
        if (sessionFactory == null)
        {
            lock (sessionFactoryLock)
            {
                sessionFactory = CreateSessionFactory();
            }
        }
        return sessionFactory.OpenSession();
    }

    public static ISession GetSession()
    {
        UnitOfWork = ServiceManager.GetUnitOfWork() as NHUnitOfWork;
        if(UnitOfWork.Session == null)
        {
            return OpenSession();
        }
        return UnitOfWork.Session;
    }
}