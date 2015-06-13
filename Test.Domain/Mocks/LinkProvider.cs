﻿using System.Collections.Generic;
using Application.Dto;
using Domain;
using Infrastructure;

namespace Test.Domain.Mocks
{
   public class LinkProvider : IResourceLinkProvider
   {
      #region Implementation of IResourceLinkProvider

      public IEnumerable<ILink> GetLinks<TEntity>(TEntity entity)
      {
         Order order = entity as Order;
         if (order == null)
         {
            yield break;
         }

         switch (order.Status)
         {
            default:
               yield break;
         }
      }

      #endregion
   }
}