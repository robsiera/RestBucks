using System;
using Application.Dto;
using AutoMapper;
using Domain;
using Infrastructure;

namespace Application
{
   public class DtoMapper : IDtoMapper
   {
      private readonly IResourceLinkProvider _linkProvider;

      #region Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="T:System.Object"/> class.
      /// </summary>
      public DtoMapper(IResourceLinkProvider linkProvider)
      {
         if (linkProvider == null)
         {
            throw new ArgumentNullException("linkProvider");
         }

         _linkProvider = linkProvider;
         ConfigureMapper();
      }

      #endregion

      #region Protected methods

      protected virtual void ConfigureMapper()
      {
         Mapper.Initialize(cfg =>
                              {
                                 cfg.CreateMap<Order, OrderDto>(MemberList.Source)
                                    .AfterMap((src, dest) =>
                                                 {
                                                    foreach (ILink link in _linkProvider.GetLinks(src))
                                                    {
                                                       dest.Links.Add(link);
                                                    }
                                                 });
                                 cfg.CreateMap<OrderItem, OrderItemDto>(MemberList.Source);
                                 cfg.CreateMap<Payment, PaymentDto>(MemberList.Source);
                              });
      }

      #endregion

      #region IDtoMapper Members

      /// <summary>
      /// Maps from Domain type to Dto representation
      /// </summary>
      /// <typeparam name="TSource">Domain type</typeparam>
      /// <typeparam name="TSourceDto">DTO representation</typeparam>
      /// <param name="source">Domain instance</param>
      /// <returns>Dto representation</returns>
      public TSourceDto Map<TSource, TSourceDto>(TSource source)
      {
         return Mapper.Map<TSource, TSourceDto>(source);
      }

      #endregion
   }
}