﻿using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.WPeCommerce
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.WPeCommerce)]
    [Component(RegistrationType.Self)]
    public class WPeCommerceStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WPeCommerceStoreType(StoreEntity store) :
            base(store)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.WPeCommerce;

        /// <summary>
        /// Log request/responses as WP eCommerce
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.WPeCommerce;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000022267";
    }
}
