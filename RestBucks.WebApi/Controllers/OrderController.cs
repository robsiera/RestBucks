

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Application.Dto;
using Domain;
using Infrastructure;
using Infrastructure.HyperMedia.Linker.Interfaces;
using RestBucks.WebApi.Models;

namespace RestBucks.WebApi.Controllers
{
   public class OrderController : ApiController
   {
      #region Fields

      private readonly IRepository<Order> _orderRepository;
      private readonly IDtoMapper _dtoMapper;
      private readonly IResourceLinker _resourceLinker;

      #endregion

      /// <summary>
      /// Initializes a new instance of the <see cref="T:System.Web.Mvc.Controller"/> class.
      /// </summary>
      public OrderController(IRepository<Order> orderRepository, IDtoMapper dtoMapper, IResourceLinker resourceLinker)
      {
         if (orderRepository == null)
         {
            throw new ArgumentNullException("orderRepository");
         }

         if (dtoMapper == null)
         {
            throw new ArgumentNullException("dtoMapper");
         }

         _orderRepository = orderRepository;
         _dtoMapper = dtoMapper;
         _resourceLinker = resourceLinker;
      }

      public HttpResponseMessage Get(int orderId)
      {
         Order order = _orderRepository.GetById(orderId);
         if (order == null)
         {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
         }

         return Request.CreateResponse(HttpStatusCode.Accepted, _dtoMapper.Map<Order, OrderDto>(order));
      }
       
      public HttpResponseMessage Post(OrderLocationDto orderDtoModel)
      {
         Order order = _orderRepository.GetById(orderDtoModel.OrderId);
         if (order == null)
         {
            return Request.CreateResponse(HttpStatusCode.NotFound);
         }

         order.Location = orderDtoModel.Location;
         return Request.CreateResponse(HttpStatusCode.OK);
      }

      public HttpResponseMessage PostPay(int orderId, PaymentDto paymentDto)
      {
         return Request.CreateResponse(HttpStatusCode.NotFound);
      }

      public HttpResponseMessage Delete(int orderId)
      {
         var order = _orderRepository.GetById(orderId);
         if (order == null)
         {
            return Request.CreateResponse(HttpStatusCode.NotFound);
         }

         order.Cancel("canceled from the rest interface");
         return Request.CreateResponse(HttpStatusCode.OK);
      }

       //public HttpResponseMessage GetReceipt(int orderId)
       //{
       //   Order order = _orderRepository.GetById(orderId);
       //   if (order == null)
       //   {
       //      return new HttpResponseMessage(HttpStatusCode.NotFound);
       //   }

       //   return Request.CreateResponse(HttpStatusCode.Accepted, _dtoMapper.Map<Order, OrderDto>(order));
       //}
   }
}
