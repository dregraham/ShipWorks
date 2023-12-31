﻿using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.SellerExpress.WizardPages;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.SellerExpress
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.SellerExpress)]
    [Component(RegistrationType.Self)]
    public class SellerExpressStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SellerExpressStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.SellerExpress;

        /// <summary>
        /// Log request/responses as SellerExpress
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.SellerExpress;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => "https://shipworks.zendesk.com/hc/en-us/articles/360022654131";

        /// <summary>
        /// Create the Wizard pages used in the setup wizard to configure the store.
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>
            {
                new SellerExpressWizardPage()
            };
        }

        /// <summary>
        /// Create the user control used in the Store Manager window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new SellerExpressAccountSettingsControl();
        }

        /// <summary>
        /// Use the username, since the integration url will be the same for all customers.
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                GenericModuleStoreEntity genericStore = (GenericModuleStoreEntity) Store;
                return genericStore.ModuleUsername;
            }
        }
    }
}
