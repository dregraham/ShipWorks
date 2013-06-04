using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Communication;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter;

namespace ShipWorks.Stores.Platforms.AuctionSound
{
    /// <summary>
    /// AuctionSound Store implementation.
    /// </summary>
    public class AuctionSoundStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AuctionSoundStoreType(StoreEntity store)
            : base(store)
        {

        }
       
        /// <summary>
        /// Identifying typecode for AuctionSound
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.AuctionSound;
            }
        }

        /// <summary>
        /// Logging source
        /// </summary>
        public override ApiLogSource LogSource
        {
            get { return ApiLogSource.AuctionSound; }
        }


        /// <summary>
        /// Get the uniquely identifying string for this store instance
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                return ((GenericModuleStoreEntity) Store).ModuleOnlineStoreCode;
            }
        }

        /// <summary>
        /// Gets the module version we're requiring
        /// </summary>
        public override Version GetRequiredModuleVersion()
        {
            return new Version("2.4.3");
        }
        
        /// <summary>
        /// Create a new store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            GenericModuleStoreEntity genericStore = base.CreateStoreInstance() as GenericModuleStoreEntity;
            if (genericStore == null)
            {
                throw new InvalidOperationException("A GenericModuleStoreEntity instance was not returned to the AuctionSoundStoreType.");
            }

            // AuctionSound has a single endpoint
            genericStore.ModuleUrl = "https://secure.auctionsound.net/shipworks/";

            return genericStore;
        }

        /// <summary>
        /// Creates the AuctionSound setup wizard pages
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>
            {
                new WizardPages.AuctionSoundAccountPage()
            };
        }

        /// <summary>
        /// Creates the AuctionSound account settings control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new AuctionSoundAccountSettingsControl();
        }

        /// <summary>
        /// Create the new web client
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
                
                // all calls use the storecode/client
                {"storecode", new VariableTransformer("client")}
            };

            // get custom store capabilities
            GenericModuleCapabilities capabilities = LegacyAdapterCapabilities.CreateOscDerivativeDefaults();
            capabilities.OnlineShipmentDetails = true;

            // create the legacy client instead of the regular genric one
            LegacyAdapterStoreWebClient client = new LegacyAdapterStoreWebClient((GenericModuleStoreEntity)Store, LegacyStyleSheets.OscStyleSheet, capabilities, transformers);

            return client;
        }
    }
}
