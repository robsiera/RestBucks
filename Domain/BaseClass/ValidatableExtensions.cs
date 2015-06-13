using System.Linq;

namespace Domain.BaseClass
{
   public static class ValidatableExtensions
   {
      public static bool IsValid(this IValidable validable)
      {
         return !validable.GetErrorMessages().Any();
      }
   }
}