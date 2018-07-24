using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Platforms.Walmart.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Walmart.OnlineUpdating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class GrouponShipmentUploadTaskTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly WalmartShipmentUploadTask testObject;
        private readonly Mock<IActionStepContext> actionStepContext;
        private readonly Mock<IStoreManager> storeManager;
        private readonly Mock<IShippingManager> shippingManager;
        private readonly Mock<ICarrierShipmentAdapter> carrierShipmentAdapter;

        public GrouponShipmentUploadTaskTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            actionStepContext = mock.Mock<IActionStepContext>();
            storeManager = mock.Mock<IStoreManager>();
            shippingManager = mock.Mock<IShippingManager>();
            carrierShipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();

            WalmartStoreEntity store = new WalmartStoreEntity
            {
                StoreID = 1005
            };
            storeManager.Setup(sm => sm.GetStore(It.IsAny<long>())).Returns(store);

            ShipmentEntity shipment = new ShipmentEntity()
            {
                ShipmentID = 1031
            };
            carrierShipmentAdapter.Setup(c => c.Shipment).Returns(shipment);
            shippingManager.Setup(sm => sm.GetShipment(It.IsAny<long>())).Returns(carrierShipmentAdapter.Object);

            testObject = mock.Create<WalmartShipmentUploadTask>();
        }

        [Fact]
        public void InputEntityType_IsShipmentEntity()
        {
            Assert.Equal(EntityType.ShipmentEntity, testObject.InputEntityType);
        }

        [Fact]
        public async Task ActionContextPostponement_IsNotUsed()
        {
            List<long> ids = new List<long> { 1 };
            testObject.StoreID = carrierShipmentAdapter.Object.Shipment.ShipmentID;
            await testObject.RunAsync(ids, actionStepContext.Object);

            actionStepContext.Verify(a => a.CanPostpone, Times.Never);
            mock.Mock<IShipmentDetailsUpdater>().Verify(s => s.UpdateShipmentDetails(It.IsAny<IWalmartStoreEntity>(), It.IsAny<ShipmentEntity>()), Times.Once);
        }

        [Fact]
        public void CreateEditor_CreatesBasicShipmentUploadTaskEditor()
        {
            Assert.IsAssignableFrom<BasicShipmentUploadTaskEditor>(testObject.CreateEditor());
        }

        [Fact]
        public void SupportsStore_ReturnsTrueWhenGivenWalmartStore()
        {
            Assert.True(testObject.SupportsStore(new WalmartStoreEntity()));
        }

        [Fact]
        public void SupportStore_ReturnsFalseWhenGivenNonWalmartStore()
        {
            Assert.False(testObject.SupportsStore(new StoreEntity()));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}