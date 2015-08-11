using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using Moq;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.OpenAccount.Api.Environment
{
    public class UpsOpenAccountSettingsTest
    {
        private UpsSettings testObject;

        Mock<ICarrierSettingsRepository> settingsRepository;
        private ShippingSettingsEntity shippingSettings;

        [TestInitialize]
        public void Initialize()
        {
            shippingSettings = new ShippingSettingsEntity { FedExUsername = "username", FedExPassword = "password" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.UseTestServer).Returns(true);
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject = new UpsSettings(settingsRepository.Object);
        }

        [Fact]
        public void EndpointUrl_ReturnsTestingUrl_WhenUsingTestServer_Test()
        {
            // We've setup the repository in the initialize method to indicate we should use the 
            // test server, so there's no additional setup needed
            Assert.AreEqual("https://wwwcie.ups.com", testObject.EndpointUrl);
        }

        [Fact]
        public void EndpointUrl_ReturnsProductionUrl_WhenNotUsingTestServer_Test()
        {
            // setup the repository to indicate we should be using the production server
            settingsRepository.Setup(r => r.UseTestServer).Returns(false);

            // This will need to be updated when we receive the production URL from FedEx
            Assert.AreEqual("https://onlinetools.ups.com/", testObject.EndpointUrl);
        }
    }
}
