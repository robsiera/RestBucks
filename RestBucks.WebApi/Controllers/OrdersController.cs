using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Application.Dto;
using Domain;
using Domain.BaseClass;
using Infrastructure;
using Infrastructure.Persistance.Extensions;
using RestBucks.WebApi.Models;

namespace RestBucks.WebApi.Controllers
{
   public class OrdersController : ApiController
   {
      #region Fields

      private readonly IDtoMapper _dtoMapper;
      private readonly IRepository<Order> _orderRepository;
      private readonly IRepository<Product> _productRepository;

      #endregion

      #region Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="T:System.Web.Mvc.Controller"/> class.
      /// </summary>
      public OrdersController(IRepository<Order> orderRepository,
         IRepository<Product> productRepository, IDtoMapper dtoMapper)
      {
         if (orderRepository == null)
         {
            throw new ArgumentNullException("orderRepository");
         }

         if (productRepository == null)
         {
            throw new ArgumentNullException("productRepository");
         }

         if (dtoMapper == null)
         {
            throw new ArgumentNullException("dtoMapper");
         }

         _orderRepository = orderRepository;
         _productRepository = productRepository;
         _dtoMapper = dtoMapper;
      }

      #endregion

      /// <summary>
      /// Creates an order
      /// </summary>
      /// <param name="orderModel">Order dto model</param>
      /// <remarks>
      /// Json Invoke: {Location: "inShop", Items: {Name: "latte", Quantity: 5}}
      /// Xml invoke: 
      /// </remarks>
      /// <returns>Response</returns>
      public HttpResponseMessage Post(OrderDto orderModel)
      {
         var order = new Order
         {
            Date = DateTime.Today,
            Location = orderModel.Location
         };

         foreach (var requestedItem in orderModel.Items)
         {
            var product = _productRepository.GetByName(requestedItem.Name);
            if (product == null)
            {
               return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("We don't offer {0}", requestedItem.Name));
            }

            var orderItem = new OrderItem(product,
                                        requestedItem.Quantity,
                                        product.Price,
                                        requestedItem.Preferences.ToDictionary(x => x.Key, y => y.Value));
            order.AddItem(orderItem);
         }

         if (!order.IsValid())
         {
            var content = string.Join("\n", order.GetErrorMessages());
            return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Invalid entities values {0}", content));
         }

         _orderRepository.MakePersistent(order);
         //var uri = resourceLinker.GetUri<OrderResourceHandler>(orderResource => orderResource.Get(0, null), new { orderId = order.Id });
         return Request.CreateResponse(HttpStatusCode.OK);
      }
   }
}