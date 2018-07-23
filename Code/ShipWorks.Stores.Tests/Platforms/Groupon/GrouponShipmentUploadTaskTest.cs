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
using ShipWorks.Stores.Platforms.Groupon;
using ShipWorks.Stores.Platforms.Groupon.CoreExtensions.Actions;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Groupon
{
    public class GrouponShipmentUploadTaskTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly GrouponShipmentUploadTask testObject;
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

            GrouponStoreEntity store = new GrouponStoreEntity
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

            testObject = mock.Create<GrouponShipmentUploadTask>();
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
            mock.Mock<IGrouponOnlineUpdater>().Verify(s => s.UpdateShipmentDetails(It.IsAny<IGrouponStoreEntity>(), It.IsAny<IOrderEntity>(), It.IsAny<ShipmentEntity>()), Times.Once);
        }

        [Fact]
        public void CreateEditor_CreatesBasicShipmentUploadTaskEditor()
        {
            Assert.IsAssignableFrom<BasicShipmentUploadTaskEditor>(testObject.CreateEditor());
        }

        [Fact]
        public void SupportsStore_ReturnsTrueWhenGivenGrouponStore()
        {
            Assert.True(testObject.SupportsStore(new GrouponStoreEntity()));
        }

        [Fact]
        public void SupportStore_ReturnsFalseWhenGivenNonGrouponStore()
        {
            Assert.False(testObject.SupportsStore(new StoreEntity()));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}