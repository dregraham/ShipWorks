using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// Initializes a new instance of the <see cref="NoMoreRackStoreType"/> class.
        /// </summary>
        public ChoxiStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Gets the type code for this store
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.Choxi;
            }
        }


        /// <summary>
        /// Get the log source
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.Choxi;
            }
        }

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
        public override List<WizardPage> CreateAddStoreWizardPages()
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
        public override string AccountSettingsHelpUrl
        {
            get { return "http://support.shipworks.com/solution/articles/4000044856-connecting-nomorerack"; }
        }
    }
}
