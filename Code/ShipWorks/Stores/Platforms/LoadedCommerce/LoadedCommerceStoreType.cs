﻿using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.LoadedCommerce
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class LoadedCommerceStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LoadedCommerceStoreType(StoreEntity store) :
            base(store)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.LoadedCommerce;

        /// <summary>
        /// Log request/responses as LoadedCommerce
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.LoadedCommerce;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000044620";
    }
}
