using System.Linq;
using Application;
using Application.Dto;
using Domain;
using NUnit.Framework;
using SharpTestsEx;
using Test.Domain.Mocks;

namespace Test.Domain
{
   [TestFixture]
   public class GivenAnUnpaidOrder
   {
      #region Setup/Teardown

      [SetUp]
      public void SetUp()
      {
         var mapper = new DtoMapper(new LinkProvider());
         _order = new Order();
         _dto = mapper.Map<Order, OrderDto>(_order);
      }

      #endregion

      private Order _order;
      private OrderDto _dto;

      [Test]
      public void CancelShouldWork()
      {
         _order.Cancel("error");
         _order.Status.Should().Be.EqualTo(OrderStatus.Canceled);
      }

      [Test]
      public void NextStepShouldNotIncludeReceipt()
      {
         _dto.Links
            .Satisfy(
               links =>
               !links.Any(
                  l => l.Uri == "http://restbuckson.net/order/ready/0" && l.Relation.EndsWith("docs/receipt-coffee.htm")));
      }

      [Test]
      public void PayShouldWork()
      {
         _order.Pay("Jose", "123123123");
         _order.Status.Should().Be.EqualTo(OrderStatus.Paid);
      }

      [Test]
      public void TheNextStepsIncludePay()
      {
         _dto.Links
            .Satisfy(
               links =>
               links.Any(
                  l =>
                  l.Uri == "http://restbuckson.net/order/0/payment" && l.Relation.EndsWith("docs/order-pay.htm")));
      }

      [Test]
      public void TheNextStepsIncludeUpdate()
      {
         _dto.Links
            .Satisfy(
               links =>
               links.Any(
                  l => l.Uri == "http://restbuckson.net/order/0" && l.Relation.EndsWith("docs/order-update.htm")));
      }

      [Test]
      public void ThenNextStepsIncludeCancel()
      {
         _dto.Links
            .Satisfy(
               links =>
               links.Any(
                  l => l.Uri == "http://restbuckson.net/order/0" && l.Relation.EndsWith("docs/order-cancel.htm")));
      }

      [Test]
      public void ThenNextStepsIncludeGet()
      {
         _dto.Links
            .Satisfy(
               links =>
               links.Any(l => l.Uri == "http://restbuckson.net/order/0" && l.Relation.EndsWith("docs/order-get.htm")));
      }
   }
}