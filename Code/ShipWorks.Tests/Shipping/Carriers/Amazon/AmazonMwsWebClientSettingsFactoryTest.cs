using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Features.ResolveAnything;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Shipping.Carriers.Amazon;
using Autofac.Extras.Moq;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon.Api
{
    [TestClass]
    public class AmazonMwsWebClientSettingsFactoryTest
    {
        [TestMethod]
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

                Assert.AreEqual("testMerchantID", testObject.Connection.MerchantId);
            }
        }

        [TestMethod]
        public void Create_ReturnsAmazonMwsWebClientSettings_FromShipment()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEntity amazonAccount = new AmazonAccountEntity()
                {
                    AmazonAccountID = 123,
                    MerchantID = "testMerchantID",
                    AuthToken = "abc123"
                };

                AmazonShipmentEntity shipment = new AmazonShipmentEntity()
                {
                    AmazonAccountID = 123
                };

                mock.Mock<IAmazonAccountManager>()
                    .Setup(a => a.GetAccount(123))
                    .Returns(amazonAccount);
                
                AmazonMwsWebClientSettingsFactory settingsFactory = mock.Create<AmazonMwsWebClientSettingsFactory>();

                AmazonMwsWebClientSettings testObject = settingsFactory.Create(shipment);

                Assert.AreEqual("testMerchantID", testObject.Connection.MerchantId);
            }
        }

        [TestMethod]
        public void Create_ThrowsException_WhenOrderIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettingsFactory testObject = mock.Create<AmazonMwsWebClientSettingsFactory>();
                AmazonStoreEntity amazonStore = null;

                try
                {
                    testObject.Create(amazonStore);
                    Assert.Fail();
                }
                catch (ArgumentNullException)
                {
                    // Pass
                }
            }
        }

        [TestMethod]
        public void Create_ThrowsException_WhenShipmentIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettingsFactory testObject = mock.Create<AmazonMwsWebClientSettingsFactory>();
                AmazonShipmentEntity amazonShipment = null;
                
                try
                {
                    testObject.Create(amazonShipment);
                    Assert.Fail();
                }
                catch (ArgumentNullException)
                {
                    // Pass
                }
            }
        }
    }
}
