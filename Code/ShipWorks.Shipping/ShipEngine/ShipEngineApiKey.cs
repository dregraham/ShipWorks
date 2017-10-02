using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Api key for communicating with ShipEngine
    /// </summary>
    [Component]
    public class ShipEngineApiKey : IShipEngineApiKey
    {
        private readonly IShippingSettings shippingSettings;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IShipEnginePartnerClient partnerClient;
        private const string EncryptedPartnerApiKey = "Auapk4J9PBSgT+Luq91kHHGNhTddMY2y0Ih7x0/7V5bjZ1FQE2yF7WyR7oR0e0DA";

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineApiKey(IShippingSettings shippingSettings,
            IEncryptionProviderFactory encryptionProviderFactory,
            IShipEnginePartnerClient partnerClient)
        {
            this.shippingSettings = shippingSettings;
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.partnerClient = partnerClient;
        }

        /// <summary>
        /// the ApiKey
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Ensures the ApiKey contains a value
        /// </summary>
        public void Configure()
        {
            ShippingSettingsEntity settings = shippingSettings.Fetch();
            Value = settings.ShipEngineApiKey;
            if (settings.ShipEngineApiKey == string.Empty)
            {
                Value = GetNewApiKey();
                settings.ShipEngineApiKey = Value;
                shippingSettings.Save(settings);
            }
        }

        /// <summary>
        /// Creates a new account and gets the API Key from ShipEngine
        /// </summary>
        private string GetNewApiKey()
        {
            string partnerApiKey = GetPartnerApiKey();
            string shipEngineAccountId = partnerClient.CreateNewAccount(partnerApiKey);
            return partnerClient.GetApiKey(partnerApiKey, shipEngineAccountId);            
        }

        /// <summary>
        /// Gets the ShipWorks PartnerID
        /// </summary>
        private string GetPartnerApiKey()
        {
            IEncryptionProvider decrypter = encryptionProviderFactory.CreateSecureTextEncryptionProvider("ShipEngine");
            return decrypter.Decrypt(EncryptedPartnerApiKey);
        }
    }
}
