﻿using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Fortune3
{
    /// <summary>
    /// Fortune3StoreType
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Fortune3)]
    [Component(RegistrationType.Self)]
    public class Fortune3StoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fortune3StoreType"/> class.
        /// </summary>
        public Fortune3StoreType(StoreEntity store)
            : base(store)
        { }

        /// <summary>
        /// Gets the type code for this store
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Fortune3;

        /// <summary>
        /// Get the log source
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Fortune3;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000022391";
    }
}
