using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Api.Orders;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Api.Tests.Orders
{
    public class OrderResponseFactoryTest
    {
        [Fact]
        public void Create_SetsOrderInformation()
        {
            OrderEntity order = new OrderEntity()
            {
                OrderID = 5,
                OrderNumber = 123,
                OrderDate = new DateTime(2020, 2, 13, 16, 20, 45),
                OnlineLastModified = new DateTime(2020,2,14,16,20,0),
                OrderTotal = (decimal) 3.50,
                OnlineStatus = "blip"
            };
            order.ApplyOrderNumberPostfix("abc");
            order.ApplyOrderNumberPostfix("efg");

            var testObject = new OrderResponseFactory();
            var result = testObject.Create(order);
            
            Assert.Equal(order.OrderID, result.OrderId);
            Assert.Equal(order.OrderNumberComplete, result.OrderNumber);
            Assert.Equal(order.OrderDate, result.OrderDate);
            Assert.Equal(order.OnlineLastModified, result.LastModifiedDate);
            Assert.Equal(order.OrderTotal, result.OrderTotal);
            Assert.Equal(order.OnlineStatus, result.StoreStatus);
        }

        [Fact]
        public void Create_SetsShipAddressInformation()
        {
            OrderEntity order = new OrderEntity()
            {
                ShipUnparsedName = "First Last"
            };

            var testObject = new OrderResponseFactory();
            var result = testObject.Create(order);

            Assert.Equal(order.ShipUnparsedName, result.ShipAddress.RecipientName);
        }

        [Fact]
        public void Create_SetsBillAddressInformation()
        {
            OrderEntity order = new OrderEntity()
            {
                BillUnparsedName = "First Last"
            };

            var testObject = new OrderResponseFactory();
            var result = testObject.Create(order);

            Assert.Equal(order.BillUnparsedName, result.BillAddress.RecipientName);
        }
    }
}
