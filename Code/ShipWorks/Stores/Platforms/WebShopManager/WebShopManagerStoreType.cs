using System;
using System.Collections.Generic;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Logging;
using System.Net;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter;

namespace ShipWorks.Stores.Platforms.WebShopManager
{
    /// <summary>
    /// WebShopManager Store implementation.
    /// </summary>
    public class WebShopManagerStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WebShopManagerStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Identifying typecode for WebShopManager
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.WebShopManager;

        /// <summary>
        /// Logging source
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.WebShopManager;

        /// <summary>
        /// Get required module version
        /// </summary>
        public override Version GetRequiredModuleVersion()
        {
            return new Version("2.8.20");
        }

        /// <summary>
        /// Gets or sets the account settings help URL.
        /// </summary>
        /// <value>
        /// The account settings help URL.
        /// </value>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000065053";

        /// <summary>
        /// Create a legacy-compatible web client configured for WebShopManager
        /// </summary>
        public override GenericStoreWebClient CreateWebClient()
        {
            // register parameter renaming and value transforming
            Dictionary<string, VariableTransformer> transformers = new Dictionary<string, VariableTransformer>
            {
                // updatestatus call 
                {"status", new VariableTransformer("code")},

                // getorders, getcount
                {"start", new UnixTimeVariableTransformer()},
            };

            // get custom store capabilities
            GenericModuleCapabilities capabilities = LegacyAdapterCapabilities.CreateOscDerivativeDefaults();

            // WebShopManager accepts shipment updates
            capabilities.OnlineShipmentDetails = true;

            // create the legacy client instead of the regular genric one
            LegacyAdapterStoreWebClient client = new LegacyAdapterStoreWebClient((GenericModuleStoreEntity)Store, LegacyStyleSheets.OscStyleSheet, capabilities, transformers);

            // WebShopManager reports Not Found when parameters are missing and Post
            client.VersionProbeCompatibilityIndicator = HttpStatusCode.NotFound;

            return client;
        }
    }
}
