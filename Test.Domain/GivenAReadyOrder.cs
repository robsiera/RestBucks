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
   public class GivenAReadyOrder
   {
      private OrderDto _dto;
      private Order _order;

      [SetUp]
      public void SetUp()
      {
         var mapper = new DtoMapper(new LinkProvider());
         _order = new Order();
         _order.Pay("123", "jose");
         _order.Finish();
         _dto = mapper.Map<Order, OrderDto>(_order);
      }

      [Test]
      public void ThenNextStepsIncludeGet()
      {
         _dto.Links
            .Satisfy(
               links =>
               links.Any(l => l.Uri == "http://restbuckson.net/order/0/receipt" && l.Relation.EndsWith("docs/receipt-coffee.htm")));
      }

      [Test]
      public void CancelShouldThrow()
      {
         _order.Executing(o => o.Cancel("error"))
            .Throws<InvalidOrderOperationException>()
            .And
            .Exception.Message.Should().Be.EqualTo("The order can not be canceled because it is ready.");
      }

      [Test]
      public void PayShouldThrow()
      {
         _order.Executing(o => o.Pay("a", "b"))
            .Throws<InvalidOrderOperationException>()
            .And
            .Exception.Message.Should().Be.EqualTo("The order can not be paid because it is ready.");
      }
   }
}