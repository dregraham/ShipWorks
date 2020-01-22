using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.ShipEngine
{
    /// <summary>
    /// Api key for communicating with ShipEngine
    /// </summary>
    [Component(SingleInstance = true)]
    public class ShipEngineApiKey : IShipEngineApiKey
    {
        private const string EncryptedPartnerApiKey = "Auapk4J9PBSgT+Luq91kHHGNhTddMY2y0Ih7x0/7V5bjZ1FQE2yF7WyR7oR0e0DA";

        private readonly IShippingSettings shippingSettings;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IShipEnginePartnerWebClient partnerWebClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineApiKey(IShippingSettings shippingSettings,
            IEncryptionProviderFactory encryptionProviderFactory,
            IShipEnginePartnerWebClient partnerWebClient)
        {
            this.shippingSettings = shippingSettings;
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.partnerWebClient = partnerWebClient;
        }

        /// <summary>
        /// Actual API Key value
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Ensures the ApiKey contains a value
        /// </summary>
        public async Task ConfigureAsync()
        {
            ShippingSettingsEntity settings = shippingSettings.Fetch();
            string apiKey = settings.ShipEngineApiKey;
            try
            {
                if (string.IsNullOrEmpty(apiKey))
                {
                    apiKey = await GetNewApiKeyAsync().ConfigureAwait(false);
                    settings.ShipEngineApiKey = apiKey;

                    shippingSettings.Save(settings);
                }
            }
            catch (ShipEngineException)
            {
                // do nothing. if this exception was thrown, apiKey will be blank and that
                // is what value will be set as...
            }

            Value = apiKey;
        }


        /// <summary>
        /// Ensures the ApiKey contains a value
        /// </summary>
        public void Configure()
        {
            ShippingSettingsEntity settings = shippingSettings.Fetch();

            try
            {
                if (string.IsNullOrEmpty(settings.ShipEngineApiKey))
                {
                    settings.ShipEngineApiKey = GetNewApiKey();

                    shippingSettings.Save(settings);
                }
            }
            catch (ShipEngineException)
            {
                // do nothing. if this exception was thrown, apiKey will be blank and that
                // is what value will be set as...
            }

            Value = settings.ShipEngineApiKey;
        }

        /// <summary>
        /// Creates a new account and gets the API Key from ShipEngine
        /// </summary>
        private async Task<string> GetNewApiKeyAsync()
        {
            string partnerApiKey = GetPartnerApiKey();
            string shipEngineAccountId = await partnerWebClient.CreateNewAccountAsync(partnerApiKey).ConfigureAwait(false);

            return await partnerWebClient.GetApiKeyAsync(partnerApiKey, shipEngineAccountId);
        }

        /// <summary>
        /// Creates a new account and gets the API Key from ShipEngine
        /// </summary>
        private string GetNewApiKey()
        {
            string partnerApiKey = GetPartnerApiKey();
            string shipEngineAccountId = partnerWebClient.CreateNewAccount(partnerApiKey);

            return partnerWebClient.GetApiKey(partnerApiKey, shipEngineAccountId);
        }

        /// <summary>
        /// Gets the ShipWorks PartnerID
        /// </summary>
        public string GetPartnerApiKey()
        {
            IEncryptionProvider decrypter = encryptionProviderFactory.CreateSecureTextEncryptionProvider("ShipEngine");
            return decrypter.Decrypt(EncryptedPartnerApiKey);
        }
    }
}
