﻿using System;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Stores.Orders;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Stores.Platforms.Walmart.DTO;
using ShipWorks.Stores.Platforms.Walmart.OnlineUpdating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class WalmartShipmentDetailsUpdaterTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly WalmartStoreEntity store;

        public WalmartShipmentDetailsUpdaterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            store = new WalmartStoreEntity { StoreTypeCode = StoreTypeCode.Walmart };

            mock.Mock<IWalmartCombineOrderSearchProvider>()
                .Setup(x => x.GetOrderIdentifiers(It.IsAny<IOrderEntity>()))
                .ReturnsAsync(new[] { new WalmartCombinedIdentifier(1006, "1000") });
        }

        [Fact]
        public async Task UpdateShipmentDetails_DoesNotUploadShipmentDetails_WhenOrderIsManual()
        {
            ShipmentEntity shipment = new ShipmentEntity { Order = new WalmartOrderEntity() { IsManual = true } };

            var testObject = mock.Create<ShipmentDetailsUpdater>();
            await testObject.UpdateShipmentDetails(store, shipment);

            mock.Mock<IWalmartWebClient>().Verify(w => w.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UpdateShipmentDetails_DelegatesToWalmartWebClient()
        {
            ShipmentEntity shipment = CreateShipment();

            var testObject = mock.Create<ShipmentDetailsUpdater>();
            await testObject.UpdateShipmentDetails(store, shipment);

            mock.Mock<IWalmartWebClient>()
                .Verify(w => w.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateShipmentDetails_SendsUsps_WhenOrderIsOther_AndCarrierIsUsps()
        {
            ShipmentEntity shipment = CreateShipment();
            shipment.Other = new OtherShipmentEntity()
            {
                Carrier = "usps"
            };
            shipment.ShipmentTypeCode = ShipmentTypeCode.Other;
            
            var testObject = mock.Create<ShipmentDetailsUpdater>();
            await testObject.UpdateShipmentDetails(store, shipment);

            mock.Mock<IWalmartWebClient>()
                .Verify(
                    w => w.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(),
                        It.Is<orderShipment>(s =>
                            s.orderLines[0].orderLineStatuses[0].trackingInfo.carrierName.Item == "USPS"),
                        It.IsAny<string>()), Times.Once);

        }

        [Fact]
        public async Task UpdateShipmentDetails_SendsOther_WhenOrderIsOther_AndCarrierIsUnknown()
        {
            ShipmentEntity shipment = CreateShipment();
            shipment.Other = new OtherShipmentEntity()
            {
                Carrier = "blah"
            };
            shipment.ShipmentTypeCode = ShipmentTypeCode.Other;

            var testObject = mock.Create<ShipmentDetailsUpdater>();
            await testObject.UpdateShipmentDetails(store, shipment);

            mock.Mock<IWalmartWebClient>()
                .Verify(
                    w => w.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(),
                        It.Is<orderShipment>(s =>
                            (string) s.orderLines[0].orderLineStatuses[0].trackingInfo.carrierName.Item == "blah"),
                        It.IsAny<string>()), Times.Once);

        }

        [Fact]
        public async Task UpdateShipmentDetails_SavesShipmentReturnedFromWebClient()
        {
            ShipmentEntity shipment = CreateShipment();
            Order order = GetOrderDto();

            mock.Mock<IWalmartWebClient>()
                .Setup(o => o.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()))
                .Returns(order);

            var testObject = mock.Create<ShipmentDetailsUpdater>();
            await testObject.UpdateShipmentDetails(store, shipment);

            mock.Mock<IOrderRepository>().Verify(r => r.Save(shipment.Order), Times.Once);
            mock.Mock<IWalmartWebClient>().Verify(c => c.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()), Times.Once);

        }

        [Fact]
        public async Task UpdateShipmentDetails_RedownloadsOrderAndSubmitsAgain_IfFirstUpdateFailed()
        {
            ShipmentEntity shipment = CreateShipment();
            Order order = GetOrderDto();

            var badRequest = mock.CreateMock<HttpWebResponse>();
            badRequest.Setup(r => r.StatusCode)
                .Returns(HttpStatusCode.BadRequest);

            mock.Mock<IWalmartWebClient>()
                .SetupSequence(o => o.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()))
                .Throws(new WalmartException(string.Empty, new WebException(string.Empty, null, WebExceptionStatus.CacheEntryNotFound, badRequest.Object)))
                .Returns(order);

            var testObject = mock.Create<ShipmentDetailsUpdater>();
            await testObject.UpdateShipmentDetails(store, shipment);

            mock.Mock<IOrderRepository>().Verify(r => r.Save(shipment.Order), Times.Exactly(2));
            mock.Mock<IWalmartWebClient>().Verify(c => c.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()), Times.Exactly(2));
        }

        private static Order GetOrderDto()
        {
            return new Order()
            {
                orderLines = new[]
                            {
                    new orderLineType()
                    {
                        lineNumber = "42",
                        orderLineStatuses = new[]
                        {
                            new orderLineStatusType()
                            {
                                status = orderLineStatusValueType.Cancelled
                            }
                        }
                    }
                }
            };
        }

        private static ShipmentEntity CreateShipment()
        {
            return new ShipmentEntity
            {
                Order =
                    new WalmartOrderEntity()
                    {
                        OrderID = 1006,
                        IsManual = false,
                        PurchaseOrderID = "123",
                        RequestedShippingMethodCode = "VALUE",
                        OrderItems =
                        {
                            new WalmartOrderItemEntity()
                            {
                                LineNumber = "42",
                                OnlineStatus = orderLineStatusValueType.Acknowledged.ToString(),
                                OriginalOrderID = 1006
                            }
                        }
                    }
            };
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}