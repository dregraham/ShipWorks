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
                ShipUnparsedName = "First Last",
                ShipStreet1 = "street1",
                ShipStreet2 = "street2",
                ShipStreet3 = "street3",
                ShipCity = "city",
                ShipStateProvCode = "ST",
                ShipCountryCode = "CC",
                ShipPostalCode = "12345"
            };

            var testObject = new OrderResponseFactory();
            var result = testObject.Create(order);

            Assert.Equal(order.ShipUnparsedName, result.ShipAddress.RecipientName);
            Assert.Equal(order.ShipStreet1, result.ShipAddress.Street1);
            Assert.Equal(order.ShipStreet2, result.ShipAddress.Street2);
            Assert.Equal(order.ShipStreet3, result.ShipAddress.Street3);
            Assert.Equal(order.ShipCity, result.ShipAddress.City);
            Assert.Equal(order.ShipStateProvCode, result.ShipAddress.StateProvince);
            Assert.Equal(order.ShipCountryCode, result.ShipAddress.CountryCode);
            Assert.Equal(order.ShipPostalCode, result.ShipAddress.PostalCode);
        }

        [Fact]
        public void Create_SetsBillAddressInformation()
        {
            OrderEntity order = new OrderEntity()
            {
                BillUnparsedName = "First Last",
                BillStreet1 = "street1",
                BillStreet2 = "street2",
                BillStreet3 = "street3",
                BillCity = "city",
                BillStateProvCode = "ST",
                BillCountryCode = "CC",
                BillPostalCode = "12345"
            };

            var testObject = new OrderResponseFactory();
            var result = testObject.Create(order);

            Assert.Equal(order.BillUnparsedName, result.BillAddress.RecipientName);
            Assert.Equal(order.BillStreet1, result.BillAddress.Street1);
            Assert.Equal(order.BillStreet2, result.BillAddress.Street2);
            Assert.Equal(order.BillStreet3, result.BillAddress.Street3);
            Assert.Equal(order.BillCity, result.BillAddress.City);
            Assert.Equal(order.BillStateProvCode, result.BillAddress.StateProvince);
            Assert.Equal(order.BillCountryCode, result.BillAddress.CountryCode);
            Assert.Equal(order.BillPostalCode, result.BillAddress.PostalCode);
        }
    }
}
