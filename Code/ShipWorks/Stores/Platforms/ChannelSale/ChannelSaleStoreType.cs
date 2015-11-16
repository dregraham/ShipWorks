using System.Collections.Generic;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.ChannelSale.WizardPages;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.ChannelSale
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class ChannelSaleStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelSaleStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.ChannelSale;

        /// <summary>
        /// Log request/responses as CreLoaded
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.ChannelSale;

        /// <summary>
        /// Create the Wizard pages used in the setup wizard to configure the store.
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>
            {
                new ChannelSaleWizardPage()
            };
        }

        /// <summary>
        /// Create the user control used in the Store Manager window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new ChannelSaleAccountSettingsControl();
        }

        /// <summary>
        /// Get the account settings help url
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000023678";

        /// <summary>
        /// Use the username, since the integration url will be the same for all customers.
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {   
                GenericModuleStoreEntity genericStore = (GenericModuleStoreEntity)Store;
                return genericStore.ModuleUsername;
            }
        }
    }
}
