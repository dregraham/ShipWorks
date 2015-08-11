using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Environment
{
    public class FedExSettingsTest
    {
        private FedExSettings testObject;

        Mock<ICarrierSettingsRepository> settingsRepository;
        private ShippingSettingsEntity shippingSettings;

        [TestInitialize]
        public void Initialize()
        {
            shippingSettings = new ShippingSettingsEntity {FedExUsername = "username", FedExPassword = "password"};

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.UseListRates).Returns(true);
            settingsRepository.Setup(r => r.UseTestServer).Returns(true);
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject = new FedExSettings(settingsRepository.Object);
        }

        [Fact]
        public void UserCredentialKey_Test()
        {
            // Make sure the user credentials return those retrieved from the repository
            Assert.AreEqual("username", testObject.UserCredentialsKey);
        }

        [Fact]
        public void UserCredentialsPassword_IsEncrypted_WhenFedExPasswordIsNotNull_Test()
        {
            // Make sure the user credentials return those retrieved from the repository
            Assert.AreEqual(SecureText.Decrypt("password", "FedEx"), testObject.UserCredentialsPassword);
        }

        [Fact]
        public void UserCredentialsPassword_IsNull_WhenFedExPasswordIsNull_Test()
        {
            // setup the test by setting the password to ull
            shippingSettings.FedExPassword = null;

            // Make sure the user credentials return those retrieved from the repository
            Assert.IsNull(testObject.UserCredentialsPassword);
        }


        [Fact]
        public void CspCredentialKey_Test()
        {
            // Testing the property value to make sure inadvertent changes are not made
            Assert.AreEqual("fOilEsjTTKLzuwNi", testObject.CspCredentialKey);
        }

        [Fact]
        public void CspCredentialPassword_Test()
        {
            // Testing the property value to make sure inadvertent changes are not made
            Assert.AreEqual("vyyT54JnUrAVbq0NR8IIcq293", testObject.CspCredentialPassword);
        }

        [Fact]
        public void ClientProductionId_Test()
        {
            // Testing the property value to make sure inadvertent changes are not made
            Assert.AreEqual("ITSW", testObject.ClientProductId);
        }

        [Fact]
        public void ClientProductionVersion_Test()
        {
            // Testing the property value to make sure inadvertent changes are not made
            Assert.AreEqual("5236", testObject.ClientProductVersion);
        }

        [Fact]
        public void CspSolutionId_Test()
        {
            // Testing the property value to make sure inadvertent changes are not made
            Assert.AreEqual("086", testObject.CspSolutionId);
        }

        [Fact]
        public void EndpointUrl_ReturnsTestingUrl_WhenUsingTestServer_Test()
        {
            // We've setup the repository in the initialize method to indicate we should use the 
            // test server, so there's no additional setup needed
            Assert.AreEqual("https://wsbeta.fedex.com:443/web-services/", testObject.EndpointUrl);
        }

        [Fact]
        public void EndpointUrl_ReturnsProductionUrl_WhenNotUsingTestServer_Test()
        {
            // setup the repository to indicate we should be using the production server
            settingsRepository.Setup(r => r.UseTestServer).Returns(false);

            // This will need to be updated when we receive the production URL from FedEx
            Assert.AreEqual("https://ws.fedex.com:443/web-services/", testObject.EndpointUrl);
        }
    }
}
