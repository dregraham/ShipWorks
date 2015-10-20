using System;
using System.Collections.Generic;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.UI.Wizard;
using System.Net;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter;

namespace ShipWorks.Stores.Platforms.OrderDynamics
{
    /// <summary>
    /// OrderDynamics integration.  
    /// 
    /// OrderDynamics hits a local web server running on the user's desktop.  An
    /// OrderDynamics licence identifier is verified and used in our licensing scheme.
    /// </summary>
    public class OrderDynamicsStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDynamicsStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Identifying store typecode for OrderDynamics
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.OrderDynamics;

        /// <summary>
        /// Logging source
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.OrderDynamics;

        /// <summary>
        /// Get the license identifier for ShipWorks
        /// </summary>
        protected override string InternalLicenseIdentifier => ((GenericModuleStoreEntity) Store).ModuleOnlineStoreCode;

        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/solution/articles/4000065047";

        /// <summary>
        /// Required module version 
        /// </summary>
        public override Version GetRequiredModuleVersion()
        {
            return new Version("2.4.1");
        }

        /// <summary>
        /// Create a new store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            GenericModuleStoreEntity genericStore = base.CreateStoreInstance() as GenericModuleStoreEntity;
            if (genericStore == null)
            {
                throw new InvalidOperationException("A GenericModuleStoreEntity instance was not returned to the OrderDynamisStoreType.");
            }

            // OrderDyanmics hits a local web server by default
            genericStore.ModuleUrl = @"http://localhost/ShipWorksHandler.ashx";

            return genericStore;
        }

        /// <summary>
        /// Create the OrderDynamics wizard pages
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>
            {
                new WizardPages.OrderDynamicsAccountPage()
            };
        }

        /// <summary>
        /// Creates the OrderDynamics account settings usercontrol
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new OrderDynamicsAccountSettingsControl();
        }

        /// <summary>
        /// Create the new web client, based on V2 osc behavior
        /// </summary>
        public override GenericStoreWebClient CreateWebClient()
        {
            // configure transformation
            Dictionary<string, VariableTransformer> transformers = new Dictionary<string, VariableTransformer>
            {
                // OD doesn't use a username or password so remove them.  Null name indicates it will be removed
                {"username", new RemoveVariableTransformer()},
                {"password", new RemoveVariableTransformer()},
                {"storecode", new RemoveVariableTransformer()},

                // updatestatus call 
                {"status", new VariableTransformer("code")},

                // getorders, getcount
                {"start", new UnixTimeVariableTransformer()},
            };

            // behaves exactly like OSC, no mods needed
            GenericModuleCapabilities capabilities = LegacyAdapterCapabilities.CreateOscDerivativeDefaults();

            // Buld the legacy web client, based on Osc behavior
            LegacyAdapterStoreWebClient client = new LegacyAdapterStoreWebClient((GenericModuleStoreEntity)Store, LegacyStyleSheets.OscStyleSheet, capabilities, transformers);

            // some legacy stores blow up if they don't have querystring values when needed. Look for http 500.
            client.VersionProbeCompatibilityIndicator = HttpStatusCode.InternalServerError;

            return client;
        }
    }
}
