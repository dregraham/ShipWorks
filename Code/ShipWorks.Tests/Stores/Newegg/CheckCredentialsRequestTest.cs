using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Newegg;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation;

namespace ShipWorks.Tests.Stores.Newegg
{
    [TestClass]
    public class CheckCredentialsRequestTest
    {
        private CheckCredentialsRequest testObject;

        private Credentials validCredentials;
        private Credentials invalidSellerCredentials;
        private Credentials invalidSecretKeyCredentials;

        [TestInitialize]
        public void Initialize()
        {
            // Use the non-logging newegg request so we don't have to have a ShipWorks session
            testObject = new CheckCredentialsRequest(new Mocked.NonLoggingNeweggRequest());

            validCredentials = new Credentials("A09V", "E09799F3-A8FD-46E0-989F-B8587A1817E0", NeweggChannelType.US);
            invalidSellerCredentials = new Credentials("123", "E09799F3-A8FD-46E0-989F-B8587A1817E0", NeweggChannelType.US);
            invalidSecretKeyCredentials = new Credentials("A09V", "ABCD", NeweggChannelType.US);
        }

        [TestMethod]
        [Ignore]
        public void AreCredentialsValid_ReturnsFalse_WithInvalidSellerId_IntegrationTest()
        {            
            // This is an integration test; Marked with Ignore attribute, so this is not run in the automated build
            Assert.IsFalse(testObject.AreCredentialsValid(invalidSellerCredentials));
        }

        [TestMethod]
        [Ignore]
        public void AreCredentialsValid_ReturnsFalse_WithInvalidSecretKey_IntegrationTest()
        {
            // This is an integration test; Marked with Ignore attribute, so this is not run in the automated build
            Assert.IsFalse(testObject.AreCredentialsValid(invalidSecretKeyCredentials));
        }

        [TestMethod]
        [Ignore]
        public void AreCredentialsValid_ReturnsTrue_WithValidCredentials_IntegrationTest()
        {
            // This is an integration test; Marked with Ignore attribute, so this is not run in the automated build
            Assert.IsTrue(testObject.AreCredentialsValid(validCredentials));
        }
    }
}
