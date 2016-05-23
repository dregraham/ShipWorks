using System;
using System.Collections.Generic;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.SellerVantage
{
    /// <summary>
    /// SellerVantage Store implementation.
    /// </summary>
    public class SellerVantageStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SellerVantageStoreType(StoreEntity store)
            : base(store)
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
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000065051";

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
            genericStore.ModuleUrl = "http://app.sellervantage.com/shipworksv3/";

            return genericStore;
        }

        /// <summary>
        /// Creates the SellerVantage setup wizard pages
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
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
        public override GenericStoreWebClient CreateWebClient()
        {
            return new SellerVantageWebClient((GenericModuleStoreEntity) Store);
        }
    }
}
