﻿using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.SellerActive
{
    /// <summary>
    /// SellerActive generic module store implementation
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.SellerActive, ExternallyOwned = true)]
    public class SellerActiveStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SellerActiveStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {
        }

        /// <summary>
        /// StoreTypeCode enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.SellerActive;

        /// <summary>
        /// Log request/responses as SellerActive
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.SellerActive;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl =>
            "https://shipworks.zendesk.com/hc/en-us/articles/360022654071";
    }
}
