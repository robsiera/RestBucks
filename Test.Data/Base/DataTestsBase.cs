using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infrastructure.Persistance.Modules;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Test.Data.Base
{
   public class DataTestsBase
   {
      protected ISessionFactory _sessionFactory;
      private Configuration _configuration;

      [TestFixtureSetUp]
      public void FixtureSetUp()
      {
         _sessionFactory = Fluently.Configure()
            //.Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("Restbucks_Test"))
            .Database(SQLiteConfiguration.Standard.UsingFile("restbucks_test.db"))
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateModule>())
            .ExposeConfiguration(c =>
                                    {
                                       _configuration = c;
                                       new SchemaExport(c).Execute(true, true, false);
                                    })
            .BuildSessionFactory();

         _sessionFactory = _configuration.BuildSessionFactory();
      }

      [TestFixtureTearDown]
      public void TearDown()
      {
         new SchemaExport(_configuration).Execute(true, true, true);
      }
   }
}