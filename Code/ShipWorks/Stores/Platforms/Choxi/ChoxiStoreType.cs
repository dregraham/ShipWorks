using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Choxi 
{
    public class ChoxiStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChoxiStoreType"/> (formerly NoMoreRack) class.
        /// </summary>
        public ChoxiStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Gets the type code for this store
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Choxi;


        /// <summary>
        /// Get the log source
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Choxi;

        /// <summary>
        /// Create the user control used in the Store Manager window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new ChoxiStoreAccountSettingsControl();
        }

        /// <summary>
        /// Create the Wizard pages used in the setup wizard to configure the store.
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            List<WizardPage> pages = new List<WizardPage> { new ChoxiAddStoreWizardPage() };

            return pages;
        }

        /// <summary>
        /// Identifies this store type
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                //NMR rebranded as Choxi, have to send old url as the identifier 
                //to keep from having to reset all of the tango licenses

                return ((GenericModuleStoreEntity)Store).ModuleUrl.Replace( "choxi", "nomorerack");
            }
        }

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000044856";
    }
}
