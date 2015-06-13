using Domain;
using FluentNHibernate.Mapping;

namespace Infrastructure.Persistance.Mappings
{
   public class OrderItemMapper : ClassMap<OrderItem>
   {
      public OrderItemMapper()
      {
         Id(x => x.Id);
         Version(x => x.Version);
         Map(x => x.UnitPrice);
         Map(x => x.Quantity);
         HasMany(x => x.Preferences).AsMap<string>("id").Element("idx", x => x.Type<string>())
            .KeyColumn("orderite_key").Cascade.All();
         References(x => x.Order);
         References(x => x.Product);
      }
   }
}