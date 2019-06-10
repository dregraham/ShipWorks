using System;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon.Api
{
    [Trait("Carrier", "Amazon")]
    public class AmazonMwsWebClientSettingsFactoryTest
    {
        [Fact]
        public void Create_ReturnsAmazonMwsWebClientSettings_FromShipment()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                AmazonStoreEntity amazonStore = new AmazonStoreEntity()
                {
                    AmazonApiRegion = "US",
                    MerchantID = "testMerchantID",
                    AuthToken = "abc123"
                };

                mock.Mock<IStoreManager>()
                    .Setup(m => m.GetRelatedStore(It.IsAny<ShipmentEntity>()))
                    .Returns(amazonStore);


                var foo = mock.MockFunc<IAmazonCredentials, IAmazonMwsWebClientSettings>();
                AmazonMwsWebClientSettingsFactory settingsFactory = mock.Create<AmazonMwsWebClientSettingsFactory>(TypedParameter.From(foo));

                IAmazonMwsWebClientSettings testObject = settingsFactory.Create(new AmazonSFPShipmentEntity());
                foo.Verify(x => x(amazonStore));
            }
        }

        [Fact]
        public void Create_ThrowsException_WhenOrderIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettingsFactory testObject = mock.Create<AmazonMwsWebClientSettingsFactory>();
                AmazonSFPShipmentEntity amazonShipment = null;

                Assert.Throws<ArgumentNullException>(() => testObject.Create(amazonShipment));
            }
        }

        [Fact]
        public void Create_ThrowsException_WhenShipmentIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettingsFactory testObject = mock.Create<AmazonMwsWebClientSettingsFactory>();
                AmazonSFPShipmentEntity amazonShipment = null;

                Assert.Throws<ArgumentNullException>(() => testObject.Create(amazonShipment));
            }
        }
    }
}
