﻿using System.Collections.Generic;
using Domain;
using Infrastructure;

namespace RestBucks.WebApi.ResourceProvider
{
   public class ResourceLinkProvider : IResourceLinkProvider
   {
      #region Implementation of IResourceLinkProvider

      public IEnumerable<ILink> GetLinks<TEntity>(TEntity entity)
      {
         if (entity is Order)
         {
            foreach (ILink internalLink in GetInternalLinks(entity as Order))
            {
               yield return internalLink;
            }
         }
      }

      #endregion

      #region Private methods

      private IEnumerable<ILink> GetInternalLinks(Order order)
      {


         switch (order.Status)
         {
            case OrderStatus.Unpaid:
               //yield return get;
               //yield return update;
               //yield return cancel;
               //yield return pay;
               break;
            case OrderStatus.Paid:
            case OrderStatus.Delivered:
               //yield return get;
               break;
            case OrderStatus.Ready:
               //yield return receipt;
               break;
            case OrderStatus.Canceled:
               yield break;
            default:
               yield break;
         }
      }

      #endregion

   }
}