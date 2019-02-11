﻿using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Zenventory
{
    /// <summary>
    /// Zenventory store type
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Zenventory)]
    [Component(RegistrationType.Self)]
    public class ZenventoryStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZenventoryStoreType"/> class.
        /// </summary>
        public ZenventoryStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {

        }

        /// <summary>
        /// Gets the type code for this store
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Zenventory;

        /// <summary>
        /// Get the log source
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Zenventory;

        /// <summary>
        /// Create the user control used in the Store Manager window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new ZenventoryStoreAccountSettingsControl();
        }

        /// <summary>
        /// Create the Wizard pages used in the setup wizard to configure the store.
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            List<WizardPage> pages = new List<WizardPage> { new ZenventoryAddStoreWizardPage() };

            return pages;
        }

        /// <summary>
        /// Identifies this store type
        /// </summary>
        protected override string InternalLicenseIdentifier => ((GenericModuleStoreEntity) Store).ModuleUsername;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "https://shipworks.zendesk.com/hc/en-us/articles/360022466772";
    }
}
