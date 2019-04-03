﻿using System;
using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.SellerVantage
{
    /// <summary>
    /// SellerVantage Store implementation.
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.SellerVantage)]
    [Component(RegistrationType.Self)]
    public class SellerVantageStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SellerVantageStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {

        }

        /// <summary>
        /// Identifying typecode for SellerVantage
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.SellerVantage;

        /// <summary>
        /// Logging source
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.SellerVantage;


        /// <summary>
        /// Get the uniquely identifying string for this store instance
        /// </summary>
        protected override string InternalLicenseIdentifier => ((GenericModuleStoreEntity) Store).ModuleOnlineStoreCode;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => "https://shipworks.zendesk.com/hc/en-us/articles/360022654151";

        /// <summary>
        /// Create a new store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            GenericModuleStoreEntity genericStore = base.CreateStoreInstance() as GenericModuleStoreEntity;
            if (genericStore == null)
            {
                throw new InvalidOperationException("A GenericModuleStoreEntity instance was not returned to the SellerVantageStoreType.");
            }

            // SellerVantage has a single endpoint
            genericStore.ModuleUrl = "https://sifm.sellervantage.com/shipworksv3/";

            return genericStore;
        }

        /// <summary>
        /// Creates the SellerVantage setup wizard pages
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>
            {
                new WizardPages.SellerVantageAccountPage()
            };
        }

        /// <summary>
        /// Creates the SellerVantage account settings control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new SellerVantageAccountSettingsControl();
        }

        /// <summary>
        /// Create our SellerVantage specific web client
        /// </summary>
        public override IGenericStoreWebClient CreateWebClient() =>
            new SellerVantageWebClient((GenericModuleStoreEntity) Store);
    }
}
