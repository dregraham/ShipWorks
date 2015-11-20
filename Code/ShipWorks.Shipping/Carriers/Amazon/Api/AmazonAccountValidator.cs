using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Validate Amazon accounts
    /// </summary>
    public class AmazonAccountValidator : IAmazonAccountValidator
    {
        private readonly IAmazonShippingWebClient client;
        private readonly IAmazonMwsWebClientSettingsFactory settingsFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonAccountValidator(IAmazonShippingWebClient client, IAmazonMwsWebClientSettingsFactory settingsFactory)
        {
            this.client = client;
            this.settingsFactory = settingsFactory;
        }

        /// <summary>
        /// Validate the given account
        /// </summary>
        public bool ValidateAccount(IAmazonCredentials credentials)
        {
            AmazonValidateCredentialsResponse response = client.ValidateCredentials(settingsFactory.Create(credentials));
            return response.Success;
        }
    }
}
