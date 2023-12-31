﻿using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.ZenCart
{
    /// <summary>
    /// Implementation of the Zen Cart integration
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.ZenCart)]
    [Component(RegistrationType.Self)]
    public class ZenCartStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ZenCartStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {

        }

        /// <summary>
        /// Gets the store typecode for this store
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.ZenCart;

        /// <summary>
        /// Log request/responses as ZenCart
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.ZenCart;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => "https://shipworks.zendesk.com/hc/en-us/articles/360022654751";
    }
}
