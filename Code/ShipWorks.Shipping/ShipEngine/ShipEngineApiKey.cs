﻿using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Api key for communicating with ShipEngine
    /// </summary>
    [Component(SingleInstance = true)]
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    public class ShipEngineApiKey : IShipEngineApiKey, IInitializeForCurrentSession
    {
        private readonly IShippingSettings shippingSettings;
        private readonly IShipEnginePartnerWebClient partnerWebClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineApiKey(IShippingSettings shippingSettings,
            IEncryptionProviderFactory encryptionProviderFactory,
            IShipEnginePartnerWebClient partnerWebClient)
        {
            this.shippingSettings = shippingSettings;
            this.partnerWebClient = partnerWebClient;
        }

        /// <summary>
        /// Actual API Key value
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Ensures the ApiKey contains a value
        /// </summary>
        public async Task Configure()
        {
            ShippingSettingsEntity settings = shippingSettings.Fetch();
            string apiKey = settings.ShipEngineApiKey;
            try
            {
                if (string.IsNullOrEmpty(apiKey))
                {
                    apiKey = await partnerWebClient.CreateNewAccount().ConfigureAwait(false);
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
        /// Reset the api key value when logging in
        /// this ensures that we get a fresh value when connecting to a new database
        /// </summary>
        public void InitializeForCurrentSession() =>
            Value = null;
    }
}
