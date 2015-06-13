using System;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure
{
   /// <summary>
   /// Base generic repository
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public interface IRepository<T>
   {
      void MakePersistent(params T[] entities);
      T GetById(long id);
      IQueryable<T> Retrieve(Expression<Func<T, bool>> criteria);
      IQueryable<T> RetrieveAll();
   }
}