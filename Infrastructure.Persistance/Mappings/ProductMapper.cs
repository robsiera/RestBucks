using Domain;
using FluentNHibernate.Mapping;

namespace Infrastructure.Persistance.Mappings
{
   public class ProductMapper : ClassMap<Product>
   {
      public ProductMapper()
      {
         Id(x => x.Id);
         Version(x => x.Version);
         Map(x => x.Name);
         Map(x => x.Price);
         HasManyToMany(x => x.Customizations).AsSet().Access.CamelCaseField(Prefix.Underscore);
      }
   }
}