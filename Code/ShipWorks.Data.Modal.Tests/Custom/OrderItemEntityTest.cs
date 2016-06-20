using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class OrderItemEntityTest
    {
        [Fact]
        public void Constructor_OrderIsSet()
        {
            OrderEntity order = new OrderEntity();
            OrderItemEntity testObject = new OrderItemEntity(order);

            Assert.Equal(order, testObject.Order);
        }

        [Fact]
        public void Constructor_StringFieldIsSetToBlank()
        {
            OrderEntity order = new OrderEntity();
            OrderItemEntity testObject = new OrderItemEntity(order);

            Assert.Equal(string.Empty, testObject.Code);
        }

        [Fact]
        public void Constructor_IsManualIsFalse()
        {
            OrderEntity order = new OrderEntity();
            OrderItemEntity testObject = new OrderItemEntity(order);

            Assert.False(testObject.IsManual);
        }

        [Fact]
        public void Constructor_OrderContainsOrderItem()
        {
            OrderEntity order = new OrderEntity();
            OrderItemEntity testObject = new OrderItemEntity(order);

            Assert.Contains(testObject, order.OrderItems);
        }
    }
}
