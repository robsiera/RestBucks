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
   public class GivenACanceledOrder
   {
      private Order _order;
      private OrderDto _dto;
      [SetUp]
      public void SetUp()
      {
         DtoMapper mapper = new DtoMapper(new LinkProvider());
         _order = new Order();
         _order.Cancel("You are too slow.");
         _dto = mapper.Map<Order, OrderDto>(_order);
      }

      [Test]
      public void NextStepsShouldBeEmpty()
      {
         _dto.Links.Should().Be.Empty();
      }

      [Test]
      public void CancelShouldThrow()
      {
         _order.Executing(o => o.Cancel("error"))
             .Throws<InvalidOrderOperationException>()
             .And
             .Exception.Message.Should().Be.EqualTo("The order can not be canceled because it is canceled.");
      }
      [Test]
      public void AddItemShouldThrow()
      {
         _order.Executing(o => o.AddItem(new OrderItem()))
             .Throws<InvalidOrderOperationException>()
             .And
             .Exception.Message.Should().Be.EqualTo("Can't add another item to the order because it is canceled.");
      }
      [Test]
      public void PayShouldThrow()
      {
         _order.Executing(o => o.Pay("a", "b"))
             .Throws<InvalidOrderOperationException>()
             .And
             .Exception.Message.Should().Be.EqualTo("The order can not be paid because it is canceled.");
      }
   }
}