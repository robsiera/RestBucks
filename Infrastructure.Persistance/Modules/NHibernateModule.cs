using System.IO;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Infrastructure.Persistance.Modules
{
   public class NHibernateModule : Module
   {
      #region Private methods

      /// <summary>
      /// Configure session Factory
      /// </summary>
      /// <returns>Session</returns>
      private ISessionFactory CreateSessionFactory(IComponentContext componentContext)
      {
         return Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("Restbucks")))
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateModule>())
            .BuildSessionFactory();
      }

      #endregion

      #region Overrides of Module

      /// <summary>
      /// Override to add registrations to the container.
      /// </summary>
      /// <remarks>
      /// Note that the ContainerBuilder parameter is unique to this module.
      /// </remarks>
      /// <param name="builder">The builder through which components can be
      ///             registered.</param>
      protected override void Load(ContainerBuilder builder)
      {
         builder.RegisterGeneric(typeof(Repository<>))
               .As(typeof(IRepository<>))
               .InstancePerApiRequest();

         builder.Register(c => new TransactionTracker())
             .InstancePerApiRequest();

         builder.Register(c => c.Resolve<ISessionFactory>().OpenSession())
             .InstancePerApiRequest()
             .OnActivated(e => e.Context.Resolve<TransactionTracker>().CurrentTransaction = e.Instance.BeginTransaction());

         builder.Register(CreateSessionFactory).SingleInstance();

         base.Load(builder);
      }

      #endregion
   }
}