using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.NoMoreRack
{
    public class NoMoreRackStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoMoreRackStoreType"/> class.
        /// </summary>
        public NoMoreRackStoreType(StoreEntity store)
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
                return StoreTypeCode.NoMoreRack;
            }
        }


        /// <summary>
        /// Get the log source
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.NoMoreRack;
            }
        }

        /// <summary>
        /// Create the user control used in the Store Manager window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new NoMoreRackStoreAccountSettingsControl();
        }

        /// <summary>
        /// Create the Wizard pages used in the setup wizard to configure the store.
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            List<WizardPage> pages = new List<WizardPage> { new NoMoreRackAddStoreWizardPage() };

            return pages;
        }

        /// <summary>
        /// Identifies this store type
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                return ((GenericModuleStoreEntity)Store).ModuleUrl;
            }
        }

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        //public override string AccountSettingsHelpUrl
        //{
        //    get { return "http://support.shipworks.com/solution/articles/4000023323-connecting-NoMoreRack-with"; }
        //}
    }
}
