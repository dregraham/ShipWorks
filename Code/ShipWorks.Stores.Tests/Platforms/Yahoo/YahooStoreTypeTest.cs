using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Yahoo
{
    public class YahooStoreTypeTest
    {
        private readonly YahooStoreType apiTestObject;
        private readonly YahooStoreType emailTestObject;
        readonly Mock<StoreEntity> otherStore = new Mock<StoreEntity>();
        readonly Mock<YahooStoreEntity> yahooApiStore = new Mock<YahooStoreEntity>();
        private readonly Mock<YahooStoreEntity> yahooEmailStore = new Mock<YahooStoreEntity>();
        private readonly List<EnumEntry<YahooApiOrderStatus>> orderStatuses = EnumHelper.GetEnumList<YahooApiOrderStatus>();
        private YahooOrderItemEntity item;


        public YahooStoreTypeTest()
        {
            yahooApiStore.Setup(x => x.TypeCode).Returns(2);
            yahooApiStore.Setup(x => x.YahooStoreID).Returns("ysht-123456789");

            yahooEmailStore.Setup(x => x.TypeCode).Returns(2);
            yahooEmailStore.Setup(x => x.YahooStoreID).Returns("");

            apiTestObject = new YahooStoreType(yahooApiStore.Object);
            emailTestObject = new YahooStoreType(yahooEmailStore.Object);

            item = new YahooOrderItemEntity()
            {
                Url = "a url"
            };
        }

        [Fact]
        public void TypeCode_ReturnsYahooStoreTypeCode_Test()
        {
            Assert.Equal(StoreTypeCode.Yahoo, apiTestObject.TypeCode);
        }

        [Fact]
        public void Constructor_ThrowsInvalidOperationException_WhenStoreIsNotYahoo_Test()
        {
            Assert.Throws<InvalidOperationException>(() => new YahooStoreType(otherStore.Object));
        }

        [Fact]
        public void AccountSettingsHelpUrl_ReturnsCorrectUrlString_Test()
        {
            Assert.Equal("http://support.shipworks.com/solution/articles/4000068682-adding-a-yahoo-store-using-api", apiTestObject.AccountSettingsHelpUrl);
        }

        [Fact]
        public void CreateStoreInstance_ReturnsInitializedYahooStoreEntity_Test()
        {
            YahooStoreEntity storeEntity = apiTestObject.CreateStoreInstance() as YahooStoreEntity;

            Assert.NotNull(storeEntity);

            Assert.Equal("My Yahoo Store", storeEntity.StoreName);
            Assert.Equal("", storeEntity.YahooStoreID);
        }

        [Fact]
        public void CreateOrderIdentifier_ReturnsYahooOrderIdentifier_FromYahooOrder_Test()
        {
            YahooOrderEntity order = new YahooOrderEntity() {YahooOrderID = "1"};

            Assert.IsAssignableFrom<YahooOrderIdentifier>(apiTestObject.CreateOrderIdentifier(order));
        }

        [Fact]
        public void CreateOrderIdentifier_ThrowsYahooException_WhenOrderIsNotYahooOrNull_Test()
        {
            Assert.Throws<YahooException>(() => apiTestObject.CreateOrderIdentifier(new OrderEntity()));
            Assert.Throws<YahooException>(() => apiTestObject.CreateOrderIdentifier(null));
        }

        [Fact]
        public void CreateOrderItemInstance_ReturnsYahooOrderItemEntity_Test()
        {
            Assert.IsAssignableFrom<YahooOrderItemEntity>(apiTestObject.CreateOrderItemInstance());
        }

        [Fact]
        public void CreateDownloader_ReturnsYahooEmailDownloader_WhenEmailUser_Test()
        {
            Assert.IsAssignableFrom<YahooEmailDownloader>(emailTestObject.CreateDownloader());
        }

        [Fact]
        public void CreateDownloader_ReturnsYahooApiDownloader_WhenApiUser_Test()
        {
            Assert.IsAssignableFrom<YahooApiDownloader>(apiTestObject.CreateDownloader());
        }

        [Fact]
        public void CreateStoreSettingsControl_ReturnsEmailStoreSettingsControl_WhenEmailUser_Test()
        {
            Assert.IsAssignableFrom<YahooEmailStoreSettingsControl>(emailTestObject.CreateStoreSettingsControl());
        }

        [Fact]
        public void CreateStoreSettingsControl_ReturnsNull_WhenApiUser_Test()
        {
            Assert.Null(apiTestObject.CreateStoreSettingsControl());
        }

        [Fact]
        public void CreateInitialFilters_ReturnsEmptyList_WhenEmailUser_Test()
        {
            Assert.Equal(0, emailTestObject.CreateInitialFilters().Count);
        }

        [Fact]
        public void CreateInitialFilters_ReturnsListOfAllYahooOrderStatusFilters_WhenApiUser_Test()
        {
            List<string> filterNames = apiTestObject.CreateInitialFilters().Select(filter => filter.Name).ToList();

            foreach (EnumEntry<YahooApiOrderStatus> status in orderStatuses)
            {
                Assert.Contains(status.Description, filterNames);
            }
        }

        [Fact]
        public void CreateOnlineUpdateInstanceCommands_OnlyReturnsUploadShipmentDetailsCommand_WhenEmailUser_Test()
        {
            List<MenuCommand> commands = emailTestObject.CreateOnlineUpdateInstanceCommands();

            Assert.Equal(1, commands.Count);

            Assert.Equal("Upload Shipment Details", commands.FirstOrDefault()?.Text);
        }

        [Fact]
        public void CreateOnlineUpdateInstanceCommands_ReturnsUploadShipmentDetailsAndUpdateStatusCommands_WhenApiUser()
        {
            List<string> commandNames = apiTestObject.CreateOnlineUpdateInstanceCommands()
                .Select(command => command.Text).ToList();

            Assert.Equal(7, commandNames.Count);

            foreach (EnumEntry<YahooApiOrderStatus> orderStatus in orderStatuses.
                Where(orderStatus => orderStatus.Value != YahooApiOrderStatus.PartiallyShipped &&
                orderStatus.Value != YahooApiOrderStatus.FullyShipped
                && orderStatus.Value != YahooApiOrderStatus.Tracked))
            {
                Assert.Contains(orderStatus.Description, commandNames);
            }

            Assert.Contains("Upload Shipment Details", commandNames);
        }

        [Fact]
        public void GridOnlineColumnSupported_ReturnsTrue_WhenApiUserAndColumnIsOnlineStatus_Test()
        {
            Assert.True(apiTestObject.GridOnlineColumnSupported(OnlineGridColumnSupport.OnlineStatus));
        }

        [Fact]
        public void GridOnlineColumnSupported_ReturnsFalse_WhenEmailUser_Test()
        {
            Assert.False(emailTestObject.GridOnlineColumnSupported(OnlineGridColumnSupport.OnlineStatus));
        }

        [Fact]
        public void GridHyperlinkSupported_ReturnsTrue_WhenApiUserAndFieldIsOrderItemName_Test()
        {


            Assert.True(apiTestObject.GridHyperlinkSupported(item, OrderItemFields.Name));
        }

        [Fact]
        public void GridHyperlinkSupported_ReturnsFalse_WhenApiUserAndFieldIsNotOrderItemName_Test()
        {
            Assert.False(apiTestObject.GridHyperlinkSupported(item, OrderFields.BillFirstName));
        }

        [Fact]
        public void GridHyperlinkSupported_ReturnsFalse_WhenEmailUser_Test()
        {
            Assert.False(emailTestObject.GridHyperlinkSupported(item, OrderItemFields.Name));
        }

        [Fact]
        public void CreateAddStoreWizardOnlineUpdateActionControl_ReturnsNull_WhenEmailUser_Test()
        {
            Assert.Null(emailTestObject.CreateAddStoreWizardOnlineUpdateActionControl());
        }

        [Fact]
        public void CreateAddStoreWizardOnlineUpdateActionControl_ReturnsOnlineUpdateShipmentUpdateActionControl_WhenApiUser_Test()
        {
            Assert.IsAssignableFrom<OnlineUpdateShipmentUpdateActionControl>(
                apiTestObject.CreateAddStoreWizardOnlineUpdateActionControl());
        }
    }
}
