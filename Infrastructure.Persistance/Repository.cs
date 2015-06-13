using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;

namespace Infrastructure.Persistance
{
   /// <summary>
   /// Implementation of repository using NHibernate
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public class Repository<T> : IRepository<T>
   {
      private readonly ISession _session;

      public Repository(ISession session)
      {
         _session = session;
      }

      private ISession CurrentSession
      {
         get { return _session; }
      }

      #region IRepository<T> Members

      public void MakePersistent(params T[] entities)
      {
         foreach (T entity in entities)
         {
            CurrentSession.Save(entity);  //todo RS: te bespreken met Sacha (seesion ipv context)
         }
      }

      public T GetById(long id)
      {
         return CurrentSession.Get<T>(id);
      }

      public IQueryable<T> Retrieve(Expression<Func<T, bool>> criteria)
      {
         return CurrentSession
            .Query<T>().Where(criteria);
      }

      public IQueryable<T> RetrieveAll()
      {
         return CurrentSession.Query<T>();
      }

      #endregion
   }
}