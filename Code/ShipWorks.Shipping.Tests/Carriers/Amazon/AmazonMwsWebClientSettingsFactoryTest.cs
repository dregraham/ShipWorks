﻿using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using Autofac.Extras.Moq;
using Xunit;
using Moq;
using ShipWorks.Stores;
using ShipWorks.Shipping.Carriers.Amazon.SFP;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon.Api
{
    [Trait("Carrier", "Amazon")]
    public class AmazonMwsWebClientSettingsFactoryTest
    {
        [Fact]
        public void Create_ReturnsAmazonMwsWebClientSettings_FromShipment()
        {
            using (var mock = AutoMock.GetLoose())
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


                AmazonMwsWebClientSettingsFactory settingsFactory = mock.Create<AmazonMwsWebClientSettingsFactory>();

                IAmazonMwsWebClientSettings testObject = settingsFactory.Create(new AmazonShipmentEntity());

                Assert.Equal("testMerchantID", testObject.Credentials.MerchantID);
            }
        }

        [Fact]
        public void Create_ThrowsException_WhenOrderIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettingsFactory testObject = mock.Create<AmazonMwsWebClientSettingsFactory>();
                AmazonShipmentEntity amazonShipment = null;

                Assert.Throws<ArgumentNullException>(() => testObject.Create(amazonShipment));
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
