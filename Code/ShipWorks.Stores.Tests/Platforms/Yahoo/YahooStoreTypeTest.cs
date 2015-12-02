using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration.WizardPages;
using ShipWorks.UI.Wizard;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Yahoo
{
    public class YahooStoreTypeTest
    {
        private YahooStoreType testObject;
        readonly Mock<StoreEntity> store = new Mock<StoreEntity>();
        readonly Mock<YahooStoreEntity> yahooStore = new Mock<YahooStoreEntity>();

        public YahooStoreTypeTest()
        {
            yahooStore.Setup(s => s.TypeCode).Returns(2);
            testObject = new YahooStoreType(yahooStore.Object);
        }

        [Fact]
        public void TypeCode_ReturnsYahooStoreTypeCode_Test()
        {
            Assert.Equal(StoreTypeCode.Yahoo, testObject.TypeCode);
        }

        [Fact]
        public void Constructor_ThrowsArgumentException_IfStoreIsNotYahoo_Test()
        {
            Assert.Throws<ArgumentException>(() => new YahooStoreType(store.Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_IfStoreIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new YahooStoreType(null));
        }

        [Fact]
        public void AccountSettingsHelpUrl_ReturnsCorrectUrlString_Test()
        {
            Assert.Equal("http://www.shipworks.com/shipworks/help/Yahoo_Email_Account.html", testObject.AccountSettingsHelpUrl);
        }

        [Fact]
        public void CreateStoreInstance_ReturnsInitializedYahooStoreEntity_Test()
        {
            YahooStoreEntity storeEntity = testObject.CreateStoreInstance() as YahooStoreEntity;

            Assert.NotNull(storeEntity);

            Assert.Equal("My Yahoo Store", storeEntity.StoreName);
            Assert.Equal("", storeEntity.YahooStoreID);
        }

        [Fact]
        public void CreateAddStoreWizardPages_ReturnsOneYahooApiAccountPageHost_Test()
        {
            List<WizardPage> pages = testObject.CreateAddStoreWizardPages();

            Assert.Equal(1, pages.Count);
            Assert.IsAssignableFrom<YahooApiAccountPageHost>(pages.First());
        }

        [Fact]
        public void CreateOrderIdentifier_ReturnsYahooOrderIdentifier_FromYahooOrder_Test()
        {
            YahooOrderEntity order = new YahooOrderEntity() {YahooOrderID = "1"};

            Assert.IsAssignableFrom<YahooOrderIdentifier>(testObject.CreateOrderIdentifier(order));
        }

        [Fact]
        public void CreateOrderIdentifier_ThrowsYahooException_IfOrderIsNotYahooOrNull_Test()
        {
            Assert.Throws<YahooException>(() => testObject.CreateOrderIdentifier(new OrderEntity()));
            Assert.Throws<YahooException>(() => testObject.CreateOrderIdentifier(null));
        }

        [Fact]
        public void CreateOrderItemInstance_ReturnsYahooOrderItemEntity_Test()
        {
            Assert.IsAssignableFrom<YahooOrderItemEntity>(testObject.CreateOrderItemInstance());
        }

        [Fact]
        public void CreateDownloader_ReturnsYahooEmailDownloader_IfEmailUser_Test()
        {

        }

        [Fact]
        public void CreateDownloader_ReturnsYahooApiDownloader_IfApiUser_Test()
        {

        }

    }
}
