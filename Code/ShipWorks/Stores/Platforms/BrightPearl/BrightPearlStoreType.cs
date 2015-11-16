using System.Collections.Generic;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Brightpearl
{
    /// <summary>
    /// BrightPearlStoreType
    /// </summary>
    public class BrightpearlStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrightpearlStoreType"/> class.
        /// </summary>
        /// <param name="store"></param>
        public BrightpearlStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Gets the type code for this store
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Brightpearl;

        /// <summary>
        /// Get the log source
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Brightpearl;

        /// <summary>
        /// Create the user control used in the Store Manager window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new BrightpearlStoreAccountSettingsControl();
        }

        /// <summary>
        /// Create the Wizard pages used in the setup wizard to configure the store.
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            List<WizardPage> pages = new List<WizardPage>();

            pages.Add(new BrightpearlAddStoreWizardPage());

            return pages;
        }

        /// <summary>
        /// Gets the help URL to use in the setup wizard.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000022247-adding-a-brightpearl-store-in-shipworks";
    }
}
