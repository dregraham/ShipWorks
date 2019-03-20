﻿using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Amosoft
{
    /// <summary>
    /// Amosoft generic module store implementation
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Amosoft, ExternallyOwned = true)]
    public class AmosoftStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmosoftStoreType"/> class.
        /// </summary>
        public AmosoftStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {
        }

        /// <summary>
        /// Get the unique, store identifier
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                GenericModuleStoreEntity store = Store as GenericModuleStoreEntity;
                return store.ModuleUsername;
            }
        }

        /// <summary>
        /// Gets the type code for this store
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Amosoft;

        /// <summary>
        /// Gets the log source
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Amosoft;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl =>
            "https://shipworks.zendesk.com/hc/en-us/articles/360022464892";
    }
}