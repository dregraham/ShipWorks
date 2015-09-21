using Xunit;
using ShipWorks.Stores.Platforms.Newegg;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation;

namespace ShipWorks.Tests.Stores.Newegg
{
    public class CheckCredentialsRequestTest
    {
        private CheckCredentialsRequest testObject;

        private Credentials validCredentials;
        private Credentials invalidSellerCredentials;
        private Credentials invalidSecretKeyCredentials;

        public CheckCredentialsRequestTest()
        {
            // Use the non-logging newegg request so we don't have to have a ShipWorks session
            testObject = new CheckCredentialsRequest(new Mocked.NonLoggingNeweggRequest());

            validCredentials = new Credentials("A09V", "E09799F3-A8FD-46E0-989F-B8587A1817E0", NeweggChannelType.US);
            invalidSellerCredentials = new Credentials("123", "E09799F3-A8FD-46E0-989F-B8587A1817E0", NeweggChannelType.US);
            invalidSecretKeyCredentials = new Credentials("A09V", "ABCD", NeweggChannelType.US);
        }

        
        public void AreCredentialsValid_ReturnsFalse_WithInvalidSellerId_IntegrationTest()
        {            
            // This is an integration test; Marked with Ignore attribute, so this is not run in the automated build
            Assert.False(testObject.AreCredentialsValid(invalidSellerCredentials));
        }

        
        public void AreCredentialsValid_ReturnsFalse_WithInvalidSecretKey_IntegrationTest()
        {
            // This is an integration test; Marked with Ignore attribute, so this is not run in the automated build
            Assert.False(testObject.AreCredentialsValid(invalidSecretKeyCredentials));
        }

        
        public void AreCredentialsValid_ReturnsTrue_WithValidCredentials_IntegrationTest()
        {
            // This is an integration test; Marked with Ignore attribute, so this is not run in the automated build
            Assert.True(testObject.AreCredentialsValid(validCredentials));
        }
    }
}
