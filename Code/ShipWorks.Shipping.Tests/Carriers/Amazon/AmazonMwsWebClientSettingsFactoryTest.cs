using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Shipping.Carriers.Amazon;
using Autofac.Extras.Moq;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon.Api
{
    [Trait("Carrier", "Amazon")]
    public class AmazonMwsWebClientSettingsFactoryTest
    {
        [Fact]
        public void Create_ReturnsAmazonMwsWebClientSettings_FromStore()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonStoreEntity amazonStore = new AmazonStoreEntity()
                {   
                    AmazonApiRegion = "US",
                    MerchantID = "testMerchantID",
                    AuthToken = "abc123"
                };

                AmazonMwsWebClientSettingsFactory settingsFactory = mock.Create<AmazonMwsWebClientSettingsFactory>();

                AmazonMwsWebClientSettings testObject = settingsFactory.Create(amazonStore);

                Assert.Equal("testMerchantID", testObject.Connection.MerchantId);
            }
        }

        [Fact]
        public void Create_ThrowsException_WhenOrderIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettingsFactory testObject = mock.Create<AmazonMwsWebClientSettingsFactory>();
                AmazonStoreEntity amazonStore = null;

                Assert.Throws<ArgumentNullException>(() => testObject.Create(amazonStore));
            }
        }

        [Fact]
        public void Create_ThrowsException_WhenShipmentIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettingsFactory testObject = mock.Create<AmazonMwsWebClientSettingsFactory>();
                AmazonShipmentEntity amazonShipment = null;

                Assert.Throws<ArgumentNullException>(() => testObject.Create(amazonShipment));
            }
        }
    }
}
