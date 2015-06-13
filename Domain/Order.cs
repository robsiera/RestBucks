using System;
using System.Collections.Generic;
using System.Linq;
using Domain.BaseClass;
using Infrastructure.Exceptions;

namespace Domain
{
   public class Order : EntityBase, IValidable
   {
      private readonly ICollection<OrderItem> _items;

      public Order()
      {
         _items = new HashSet<OrderItem>();
         Status = OrderStatus.Unpaid;
      }

      public virtual DateTime Date { get; set; }
      public virtual OrderStatus Status { get; set; }
      public virtual Location Location { get; set; }
      public virtual string CancelReason { get; protected set; }

      public virtual ICollection<OrderItem> Items
      {
         get { return _items; }
      }

      public virtual decimal Total
      {
         get { return Items.Sum(i => i.Quantity*i.UnitPrice); }
      }

      public virtual Payment Payment { get; protected set; }

      #region IValidable Members

      public virtual IEnumerable<string> GetErrorMessages()
      {
         if (!Items.Any()) yield return "The order must include at least one item.";

         IEnumerable<string> itemsErrors = Items.SelectMany((i, index) => i.GetErrorMessages()
                                                                                 .Select(
                                                                                    m =>
                                                                                    string.Format("Item {0}: {1}",
                                                                                                  index, m)));

         foreach (string itemsError in itemsErrors)
         {
            yield return itemsError;
         }
      }

      #endregion

      public virtual void AddItem(OrderItem orderItem)
      {
         if (Status != OrderStatus.Unpaid)
         {
            throw new InvalidOrderOperationException(
               string.Format("Can't add another item to the order because it is {0}.",
                             Status.ToString().ToLower()));
         }
         orderItem.Order = this;
         _items.Add(orderItem);
      }

      public virtual void Cancel(string cancelReason)
      {
         if (Status != OrderStatus.Unpaid)
         {
            throw new InvalidOrderOperationException(string.Format("The order can not be canceled because it is {0}.",
                                                                   Status.ToString().ToLower()));
         }
         CancelReason = cancelReason;
         Status = OrderStatus.Canceled;
      }

      public virtual void Pay(string cardNumber, string cardOwner)
      {
         if (Status != OrderStatus.Unpaid)
         {
            throw new InvalidOrderOperationException(string.Format("The order can not be paid because it is {0}.",
                                                                   Status.ToString().ToLower()));
         }
         Status = OrderStatus.Paid;
         Payment = new Payment {CardOwner = cardOwner, CreditCardNumber = cardNumber};
      }

      public virtual void Finish()
      {
         if (Status != OrderStatus.Paid)
         {
            throw new InvalidOrderOperationException(string.Format(
               "The order should not be finished because it is {0}.",
               Status.ToString().ToLower()));
         }
         Status = OrderStatus.Ready;
      }
   }
}