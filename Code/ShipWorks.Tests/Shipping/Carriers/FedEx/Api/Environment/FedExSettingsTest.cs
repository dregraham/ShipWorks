using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Environment
{
    public class FedExSettingsTest
    {
        private FedExSettings testObject;

        Mock<ICarrierSettingsRepository> settingsRepository;
        private ShippingSettingsEntity shippingSettings;

        public FedExSettingsTest()
        {
            shippingSettings = new ShippingSettingsEntity { FedExUsername = "username", FedExPassword = "password" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.UseListRates).Returns(true);
            settingsRepository.Setup(r => r.UseTestServer).Returns(true);
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject = new FedExSettings(settingsRepository.Object);
        }

        [Fact]
        public void UserCredentialKey()
        {
            // Make sure the user credentials return those retrieved from the repository
            Assert.Equal("username", testObject.UserCredentialsKey);
        }

        [Fact]
        public void UserCredentialsPassword_IsEncrypted_WhenFedExPasswordIsNotNull()
        {
            // Make sure the user credentials return those retrieved from the repository
            Assert.Equal(SecureText.Decrypt("password", "FedEx"), testObject.UserCredentialsPassword);
        }

        [Fact]
        public void UserCredentialsPassword_IsNull_WhenFedExPasswordIsNull()
        {
            // setup the test by setting the password to null
            shippingSettings.FedExPassword = null;

            // Make sure the user credentials return those retrieved from the repository
            Assert.Null(testObject.UserCredentialsPassword);
        }

        [Fact]
        public void CspCredentialKey()
        {
            // Testing the property value to make sure inadvertent changes are not made
            Assert.Equal("55sFa2ocvAw0Baxl", testObject.CspCredentialKey);
        }

        [Fact]
        public void CspCredentialPassword()
        {
            // Testing the property value to make sure inadvertent changes are not made
            Assert.Equal("9kJDF0zRYZ9kyBiavLBPyGTSO", testObject.CspCredentialPassword);
        }

        [Fact]
        public void ClientProductionId()
        {
            // Testing the property value to make sure inadvertent changes are not made
            Assert.Equal("ITSW", testObject.ClientProductId);
        }

        [Fact]
        public void ClientProductionVersion()
        {
            // Testing the property value to make sure inadvertent changes are not made
            Assert.Equal("6828", testObject.ClientProductVersion);
        }

        [Fact]
        public void CspSolutionId()
        {
            // Testing the property value to make sure inadvertent changes are not made
            Assert.Equal("086", testObject.CspSolutionId);
        }

        [Fact]
        public void EndpointUrl_ReturnsTestingUrl_WhenUsingTestServer()
        {
            // We've setup the repository in the initialize method to indicate we should use the
            // test server, so there's no additional setup needed
            Assert.Equal("https://wsbeta.fedex.com:443/web-services/", testObject.EndpointUrl);
        }

        [Fact]
        public void EndpointUrl_ReturnsProductionUrl_WhenNotUsingTestServer()
        {
            // setup the repository to indicate we should be using the production server
            settingsRepository.Setup(r => r.UseTestServer).Returns(false);

            // This will need to be updated when we receive the production URL from FedEx
            Assert.Equal("https://ws.fedex.com:443/web-services/", testObject.EndpointUrl);
        }
    }
}
