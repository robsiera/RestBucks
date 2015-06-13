using Domain;
using FluentNHibernate.Mapping;

namespace Infrastructure.Persistance.Mappings
{
   public class PaymentMapper : ClassMap<Payment>
   {
      public PaymentMapper()
      {
         Id(x => x.Id);
         Version(x => x.Version);
         Map(x => x.CardOwner);
         Map(x => x.CreditCardNumber);
      }
   }
}