using Domain;
using FluentNHibernate.Mapping;

namespace Infrastructure.Persistance.Mappings
{
   public class CustomizationMapper : ClassMap<Customization>
   {
      public CustomizationMapper()
      {
         Id(x => x.Id).GeneratedBy.Native();
         Version(x => x.Version);
         Map(x => x.Name);
         HasManyToMany(x => x.PossibleValues).Cascade.All().AsSet().Element("id").Access.CamelCaseField(Prefix.Underscore);
      }
   }
}