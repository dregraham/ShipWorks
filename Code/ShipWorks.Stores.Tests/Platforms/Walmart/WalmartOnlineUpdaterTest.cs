using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
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
            var orderManager = mock.Mock<IOrderManager>();
            orderManager.Setup(o => o.GetLatestActiveShipment(It.IsAny<long>())).Returns(() => null);

            var webClient = mock.Mock<IWalmartWebClient>();

            var testObject = mock.Create<WalmartOnlineUpdater>(new TypedParameter(typeof(WalmartStoreEntity), new WalmartStoreEntity()));
            testObject.UpdateShipmentDetails(new List<long> {1});

            webClient.Verify(w => w.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void UpdateShipmentDetails_DoesNotUploadShipmentDetails_WhenOrderIsManual()
        {
            ShipmentEntity shipment = new ShipmentEntity {Order = new WalmartOrderEntity() {IsManual = true}};

            var orderManager = mock.Mock<IOrderManager>();
            orderManager.Setup(o => o.GetLatestActiveShipment(It.IsAny<long>())).Returns(() => shipment);

            var webClient = mock.Mock<IWalmartWebClient>();

            var testObject = mock.Create<WalmartOnlineUpdater>(new TypedParameter(typeof(WalmartStoreEntity), new WalmartStoreEntity()));
            testObject.UpdateShipmentDetails(new List<long> { 1 });

            webClient.Verify(w => w.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void UpdateShipmentDetails_DelegatesToWalmartWebClient()
        {
            ShipmentEntity shipment = new ShipmentEntity { Order = new WalmartOrderEntity() { IsManual = false, PurchaseOrderID = "123", RequestedShippingMethodCode = "VALUE"} };

            var orderManager = mock.Mock<IOrderManager>();
            orderManager.Setup(o => o.GetLatestActiveShipment(It.IsAny<long>())).Returns(() => shipment);

            var webClient = mock.Mock<IWalmartWebClient>();

            var testObject = mock.Create<WalmartOnlineUpdater>(new TypedParameter(typeof(WalmartStoreEntity), new WalmartStoreEntity()));
            testObject.UpdateShipmentDetails(new List<long> { 1 });

            webClient.Verify(w => w.UpdateShipmentDetails(It.IsAny<WalmartStoreEntity>(), It.IsAny<orderShipment>(), It.IsAny<string>()), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}