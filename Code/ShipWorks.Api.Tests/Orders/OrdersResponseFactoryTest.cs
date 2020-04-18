using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Api.Orders;
using ShipWorks.Api.Orders.Shipments;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Api.Tests.Orders
{
    public class OrdersResponseFactoryTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private readonly OrdersResponseFactory testObject;

        public OrdersResponseFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>(); 
            shipmentAdapter.SetupGet(x => x.ServiceTypeName).Returns("Service");

            mock.Mock<ICarrierShipmentAdapterFactory>().Setup(f => f.Get(AnyShipment)).Returns(shipmentAdapter);

            testObject = mock.Create<OrdersResponseFactory>();
        }

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

            var result = testObject.CreateOrdersResponse(order);
            
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

            var result = testObject.CreateOrdersResponse(order);

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

            var result = testObject.CreateOrdersResponse(order);

            Assert.Equal(order.BillUnparsedName, result.BillAddress.RecipientName);
            Assert.Equal(order.BillStreet1, result.BillAddress.Street1);
            Assert.Equal(order.BillStreet2, result.BillAddress.Street2);
            Assert.Equal(order.BillStreet3, result.BillAddress.Street3);
            Assert.Equal(order.BillCity, result.BillAddress.City);
            Assert.Equal(order.BillStateProvCode, result.BillAddress.StateProvince);
            Assert.Equal(order.BillCountryCode, result.BillAddress.CountryCode);
            Assert.Equal(order.BillPostalCode, result.BillAddress.PostalCode);
        }

        [Fact]
        public void CreateProcessShipmentResponse_SetsCarrier()
        {
            ProcessShipmentResult processShipmentResult = CreateProcessShipmentResult();

            var response = testObject.CreateProcessShipmentResponse(processShipmentResult);

            Assert.Equal("FedEx", response.Carrier);
        }

        [Fact]
        public void CreateProcessShipmentResponse_SetsService()
        {
            ProcessShipmentResult processShipmentResult = CreateProcessShipmentResult();

            var response = testObject.CreateProcessShipmentResponse(processShipmentResult);

            Assert.Equal("Service", response.Service);
        }

        [Fact]
        public void CreateProcessShipmentResponse_SetsCost()
        {
            ProcessShipmentResult processShipmentResult = CreateProcessShipmentResult();


            var response = testObject.CreateProcessShipmentResponse(processShipmentResult);

            Assert.Equal(1.23m, response.Cost);
        }

        [Fact]
        public void CreateProcessShipmentResponse_SetsTracking()
        {
            ProcessShipmentResult processShipmentResult = CreateProcessShipmentResult();

            var response = testObject.CreateProcessShipmentResponse(processShipmentResult);

            Assert.Equal("foo", response.Tracking);
        }

        [Fact]
        public void CreateProcessShipmentResponse_DelegatesToApiLabelFactoryForLabels()
        {
            ProcessShipmentResult processShipmentResult = CreateProcessShipmentResult();

            testObject.CreateProcessShipmentResponse(processShipmentResult);

            mock.Mock<IApiLabelFactory>().Verify(f => f.GetLabels(shipmentAdapter.Object));
        }

        private static ProcessShipmentResult CreateProcessShipmentResult()
        {
            return new ProcessShipmentResult
            (
                new ShipmentEntity
                {
                    ShipmentTypeCode = ShipmentTypeCode.FedEx,
                    ShipmentCost = 1.23m,
                    TrackingNumber = "foo"
                }
            );
        }
    }
}
