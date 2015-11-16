using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    public class AmazonAccountValidator : IAmazonAccountValidator
    {
        private IAmazonShippingWebClient client;
        private IAmazonMwsWebClientSettingsFactory settingsFactory;

        public AmazonAccountValidator(IAmazonShippingWebClient client, IAmazonMwsWebClientSettingsFactory settingsFactory)
        {
            this.client = client;
            this.settingsFactory = settingsFactory;
        }
        public bool ValidateAccount(IAmazonCredentials credentials)
        {
            AmazonValidateCredentialsResponse response = client.ValidateCredentials(settingsFactory.Create(credentials));
            return response.Success;
        }
    }
}
