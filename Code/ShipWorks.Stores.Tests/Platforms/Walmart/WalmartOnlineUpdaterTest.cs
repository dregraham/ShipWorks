using System;
using System.Collections.Generic;
using System.Net;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using Moq.Language.Flow;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Stores.Platforms.Walmart.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class WalmartOnlineUpdaterTest : IDisposable
    {
        private readonly AutoMock mock;

        public WalmartOnlineUpdaterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void UpdateShipmentDetails_DoesNotUploadShipmentDetails_WhenOrderHasNoShipments()
        {
            mock.Mock<IOrderManager>().Setup(o => o.GetLatestActiveShipment(It.IsAny<long>())).Returns(() => null);
            
            var testObject = mock.Create<WalmartOnlineUpdater>(new TypedParameter(typeof(WalmartStoreEntity), new WalmartStoreEntity()));
            testObject.UpdateShipmentDetails(1);

            mock.Mock<IWalmartWebClient>().Verify(w => w.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void UpdateShipmentDetails_DoesNotUploadShipmentDetails_WhenOrderIsManual()
        {
            ShipmentEntity shipment = new ShipmentEntity {Order = new WalmartOrderEntity() {IsManual = true}};
            mock.Mock<IOrderManager>().Setup(o => o.GetLatestActiveShipment(It.IsAny<long>())).Returns(shipment);
            
            var testObject = mock.Create<WalmartOnlineUpdater>(new TypedParameter(typeof(WalmartStoreEntity), new WalmartStoreEntity()));
            testObject.UpdateShipmentDetails(1);

            mock.Mock<IWalmartWebClient>().Verify(w => w.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void UpdateShipmentDetails_DelegatesToWalmartWebClient()
        {
            ShipmentEntity shipment = CreateShipment();
            mock.Mock<IOrderManager>().Setup(o => o.GetLatestActiveShipment(It.IsAny<long>())).Returns(shipment);

            var testObject = mock.Create<WalmartOnlineUpdater>(new TypedParameter(typeof(WalmartStoreEntity), new WalmartStoreEntity()));
            testObject.UpdateShipmentDetails(1);
            
            mock.Mock<IWalmartWebClient>().Verify(w => w.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void UpdateShipmentDetails_SavesShipmentReturnedFromWebClient()
        {
            ShipmentEntity shipment = CreateShipment();
            mock.Mock<IOrderManager>().Setup(o => o.GetLatestActiveShipment(It.IsAny<long>())).Returns(shipment);
            Order order = GetOrderDto();

            mock.Mock<IWalmartWebClient>()
                .Setup(o =>o.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()))
                .Returns(order);

            var testObject = mock.Create<WalmartOnlineUpdater>(new TypedParameter(typeof(WalmartStoreEntity), new WalmartStoreEntity()));
            testObject.UpdateShipmentDetails(1);

            mock.Mock<IOrderRepository>().Verify(r => r.Save(shipment.Order), Times.Once);
            mock.Mock<IWalmartWebClient>().Verify(c => c.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()), Times.Once);

        }

        [Fact]
        public void UpdateShipmentDetails_RedownloadsOrderAndSubmitsAgain_IfFirstUpdateFailed()
        {
            ShipmentEntity shipment = CreateShipment();
            mock.Mock<IOrderManager>().Setup(o => o.GetLatestActiveShipment(It.IsAny<long>())).Returns(shipment);
            Order order = GetOrderDto();

            var badRequest = mock.CreateMock<HttpWebResponse>();
            badRequest.Setup(r => r.StatusCode)
                .Returns(HttpStatusCode.BadRequest);

            mock.Mock<IWalmartWebClient>()
                .SetupSequence(o => o.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()))
                .Throws(new WalmartException(string.Empty, new WebException(string.Empty, null, WebExceptionStatus.CacheEntryNotFound, badRequest.Object)))
                .Returns(order);

            var testObject = mock.Create<WalmartOnlineUpdater>(new TypedParameter(typeof(WalmartStoreEntity), new WalmartStoreEntity()));
            testObject.UpdateShipmentDetails(1);

            mock.Mock<IOrderRepository>().Verify(r => r.Save(shipment.Order), Times.Exactly(2));
            mock.Mock<IWalmartWebClient>().Verify(c=>c.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()), Times.Exactly(2));
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
                        IsManual = false,
                        PurchaseOrderID = "123",
                        RequestedShippingMethodCode = "VALUE",
                        OrderItems =
                        {
                            new WalmartOrderItemEntity()
                            {
                                LineNumber = "42",
                                OnlineStatus = orderLineStatusValueType.Acknowledged.ToString()
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