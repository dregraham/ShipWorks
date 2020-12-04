using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Warehouse.Configuration.DTO;

namespace ShipWorks.Warehouse.Configuration.Customer
{
    /// <summary>
    /// Configures customer information downloaded from the Hub
    /// </summary>
    [Component]
    public class HubCustomerConfigurator : IHubCustomerConfigurator
    {
        private readonly IShippingSettings shippingSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubCustomerConfigurator(IShippingSettings shippingSettings)
        {
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Configure customer
        /// </summary>
        public void Configure(HubConfiguration hubConfiguration)
        {
            ShippingSettingsEntity shippingSettingsEntity = shippingSettings.Fetch();
            bool hasShipEngineApiKey = !string.IsNullOrWhiteSpace(shippingSettingsEntity.ShipEngineApiKey);
            bool hasShipEngineAccountID = !string.IsNullOrWhiteSpace(shippingSettingsEntity.ShipEngineAccountID);

            if (!hasShipEngineAccountID)
            {
                if (hasShipEngineApiKey)
                {
                    // If the user has a ShipEngine API key, but not an account ID, that means they have a ShipEngine account that
                    // was created for this specific database. We don't want to overwrite this with the account from the hub,
                    // so just return.
                    return;
                }

                // If the user doesn't have either ShipEngine field, that means a SE account has not been created for this
                // database, so import the account from the hub.
                shippingSettingsEntity.ShipEngineAccountID = hubConfiguration.ShipEngineAccountID;
                shippingSettingsEntity.ShipEngineApiKey = hubConfiguration.ShipEngineApiKey;

                shippingSettings.Save(shippingSettingsEntity);
            }
            else
            {
                // If the ShipEngine Account ID from the hub matches the one in the database, but the keys do not match
                // import the new key from the hub. This allows us to change a customers SE API key on the hub
                // and have it propagate down to each database
                if (string.Equals(shippingSettingsEntity.ShipEngineAccountID,
                                  hubConfiguration.ShipEngineAccountID,
                                  StringComparison.InvariantCultureIgnoreCase) &&
                    !string.Equals(shippingSettingsEntity.ShipEngineApiKey,
                                  hubConfiguration.ShipEngineApiKey,
                                  StringComparison.InvariantCultureIgnoreCase))
                {
                    shippingSettingsEntity.ShipEngineApiKey = hubConfiguration.ShipEngineApiKey;

                    shippingSettings.Save(shippingSettingsEntity);
                }
            }
        }
    }
}