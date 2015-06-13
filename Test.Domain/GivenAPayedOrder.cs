using System.Linq;
using Application;
using Application.Dto;
using Domain;
using Infrastructure.Exceptions;
using NUnit.Framework;
using SharpTestsEx;
using Test.Domain.Mocks;

namespace Test.Domain
{
   [TestFixture]
   public class GivenAPayedOrder
   {
      #region Setup/Teardown

      [SetUp]
      public void SetUp()
      {
         var mapper = new DtoMapper(new LinkProvider());
         _order = new Order();
         _order.Pay("123", "jose");
         _dto = mapper.Map<Order, OrderDto>(_order);
      }

      #endregion

      private Order _order;
      private OrderDto _dto;

      [Test]
      public void CancelShouldThrow()
      {
         _order.Executing(o => o.Cancel("error"))
            .Throws<InvalidOrderOperationException>()
            .And
            .Exception.Message.Should().Be.EqualTo("The order can not be canceled because it is paid.");
      }

      [Test]
      public void PayShouldThrow()
      {
         _order.Executing(o => o.Pay("123", "jes"))
            .Throws<InvalidOrderOperationException>()
            .And
            .Exception.Message.Should().Be.EqualTo("The order can not be paid because it is paid.");
      }

      [Test]
      public void ThenNextStepsIncludeGet()
      {
         _dto.Links.Satisfy(links => links.Any(l => l.Uri == "http://restbuckson.net/order/0" && l.Relation.EndsWith("docs/order-get.htm")));
      }
   }
}