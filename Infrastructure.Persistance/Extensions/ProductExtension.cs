using System.Linq;
using Domain;

namespace Infrastructure.Persistance.Extensions
{
   public static class ProductExtension
   {
      public static Product GetByName(this IRepository<Product> products, string name)
      {
         return products.Retrieve(p => p.Name.ToLower() == name.ToLower())
                                            .FirstOrDefault();
      }
   }
}