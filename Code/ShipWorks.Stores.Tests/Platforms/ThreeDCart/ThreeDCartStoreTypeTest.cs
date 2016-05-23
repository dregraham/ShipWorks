using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ThreeDCart;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;
using ShipWorks.Stores.Platforms.ThreeDCart.WizardPages;
using Xunit;
using ThreeDCartOrderStatus = ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus;

namespace ShipWorks.Stores.Tests.Platforms.ThreeDCart
{
    public class ThreeDCartStoreTypeTest
    {
        private readonly ThreeDCartStoreType testObject;
        private readonly ThreeDCartStoreType soapTestObject;
        readonly Mock<StoreEntity> otherStore = new Mock<StoreEntity>();
        readonly Mock<ThreeDCartStoreEntity> threeDCartStore = new Mock<ThreeDCartStoreEntity>();
        readonly Mock<ThreeDCartStoreEntity> soapThreeDCartStore = new Mock<ThreeDCartStoreEntity>();
        private readonly List<EnumEntry<ThreeDCartOrderStatus>> orderStatuses = EnumHelper.GetEnumList<ThreeDCartOrderStatus>();

        public ThreeDCartStoreTypeTest()
        {
            threeDCartStore.Setup(s => s.TypeCode).Returns((int) StoreTypeCode.ThreeDCart);
            threeDCartStore.Setup(s => s.RestUser).Returns(true);
            testObject = new ThreeDCartStoreType(threeDCartStore.Object);

            soapThreeDCartStore.Setup(s => s.TypeCode).Returns((int)StoreTypeCode.ThreeDCart);
            soapThreeDCartStore.Setup(s => s.RestUser).Returns(false);
            soapTestObject = new ThreeDCartStoreType(soapThreeDCartStore.Object);
        }

        [Fact]
        public void TypeCode_ReturnsThreeDCart()
        {
            Assert.Equal(StoreTypeCode.ThreeDCart, testObject.TypeCode);
        }

        [Fact]
        public void Constructor_ThrowsInvalidOperationException_WhenStoreIsNotThreeDCart()
        {
            Assert.Throws<InvalidOperationException>(() => new ThreeDCartStoreType(otherStore.Object));
        }

        [Fact]
        public void AccountSettingsHelpUrl_ReturnsCorrectUrlString_WhenRestUser()
        {
            Assert.Equal("http://support.shipworks.com/solution/articles/4000076906-adding-3dcart-using-rest-api", testObject.AccountSettingsHelpUrl);
        }

        [Fact]
        public void AccountSettingsHelpUrl_ReturnsCorrectUrlString_WhenSoapUser()
        {
            Assert.Equal("http://support.shipworks.com/support/solutions/articles/167787-adding-a-3dcart-store-", soapTestObject.AccountSettingsHelpUrl);
        }

        [Fact]
        public void CreateStoreInstance_ReturnsInitializedThreeDCartStoreEntity()
        {
            ThreeDCartStoreEntity storeEntity = testObject.CreateStoreInstance() as ThreeDCartStoreEntity;

            Assert.NotNull(storeEntity);
            Assert.Equal("3dcart Store", storeEntity.StoreName);
        }

        [Fact]
        public void CreateOrderIdentifier_ReturnsThreeDCartOrderIdentifier_FromThreeDCartOrder()
        {
            ThreeDCartOrderEntity order = new ThreeDCartOrderEntity() {OrderNumber = 100};

            Assert.IsAssignableFrom<ThreeDCartOrderIdentifier>(testObject.CreateOrderIdentifier(order));
        }

        [Fact]
        public void CreateOrderItemInstance_ReturnsThreeDCartOrderItemEntity()
        {
            Assert.IsAssignableFrom<ThreeDCartOrderItemEntity>(testObject.CreateOrderItemInstance());
        }

        [Fact]
        public void CreateDownloader_ReturnsThreeDCartRestDownloader_WhenRestUser()
        {
            Assert.IsAssignableFrom<ThreeDCartRestDownloader>(testObject.CreateDownloader());
        }

        [Fact]
        public void CreateDownloader_ReturnsThreeDCartSoapDownloader_WhenSoapUser()
        {
            Assert.IsAssignableFrom<ThreeDCartSoapDownloader>(soapTestObject.CreateDownloader());
        }

        [Fact]
        public void CreateStoreSettingsControl_ReturnsThreeDCartSettingsControl()
        {
            Assert.IsAssignableFrom<ThreeDCartStoreSettingsControl>(testObject.CreateStoreSettingsControl());
        }

        [Fact]
        public void CreateInitialFilters_ReturnsListThreeDCartOrderStatusFilters()
        {
            List<string> filterNames = testObject.CreateInitialFilters().Select(filter => filter.Name).ToList();

            foreach (EnumEntry<ThreeDCartOrderStatus> status in orderStatuses)
            {
                Assert.Contains(status.Description, filterNames);
            }
        }

        [Fact]
        public void CreateOnlineUpdateInstanceCommands_ReturnsUploadShipmentDetailsAndUpdateStatusCommands()
        {
            List<string> commandNames = testObject.CreateOnlineUpdateInstanceCommands()
                .Select(command => command.Text).ToList();

            foreach (EnumEntry<ThreeDCartOrderStatus> orderStatus in orderStatuses)
            {
                Assert.Contains(orderStatus.Description, commandNames);
            }

            Assert.Contains("Upload Shipment Details", commandNames);
        }

        [Fact]
        public void GridOnlineColumnSupported_ReturnsTrue_WhenOnlineGridColumnIsOnlineStatus()
        {
            Assert.True(testObject.GridOnlineColumnSupported(OnlineGridColumnSupport.OnlineStatus));
        }

        [Fact]
        public void GridOnlineColumnSupported_ReturnsTrue_WhenOnlineGridColumnIsLastModified()
        {
            Assert.True(testObject.GridOnlineColumnSupported(OnlineGridColumnSupport.LastModified));
        }

        [Fact]
        public void CreateAddStoreWizardOnlineUpdateActionControl_ReturnsThreeDCartOnlineUpdateShipmentUpdateActionControl()
        {
            Assert.IsAssignableFrom<ThreeDCartOnlineUpdateActionControl>(
                testObject.CreateAddStoreWizardOnlineUpdateActionControl());
        }
    }
}