using Domain;
using FluentNHibernate.Mapping;

namespace Infrastructure.Persistance.Mappings
{
   public class OrderMapper : ClassMap<Order>
   {
      public OrderMapper()
      {
         Id(x => x.Id);
         Version(x => x.Version);
         HasMany(x => x.Items).AsSet().Cascade.All().Access.CamelCaseField(Prefix.Underscore);
         References(x => x.Payment).Cascade.All().Access.BackingField();
         Map(x => x.Total).Access.ReadOnly();
         Map(x => x.Status).CustomType<int>();
         Map(x => x.Location).CustomType<int>();
         Map(x => x.CancelReason).Access.ReadOnly();
         Map(x => x.Date);
      }
   }
}