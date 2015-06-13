﻿namespace Domain.BaseClass
{
   public class EntityBase : IVersionable
   {
      public virtual long Id { get; set; }

      #region IVersionable Members

      public virtual int Version { get; set; }

      #endregion

      public override bool Equals(object obj)
      {
         var other = obj as EntityBase;
         if (other == null) return false;
         if (default(long) == other.Id) return ReferenceEquals(this, obj);
         return other.Id == Id;
      }

      public override int GetHashCode()
      {
         return Id.GetHashCode();
      }
   }
}