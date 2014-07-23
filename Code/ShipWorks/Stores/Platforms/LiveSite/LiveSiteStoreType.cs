using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.LiveSite
{
    /// <summary>
    /// LiveSite StoreType
    /// </summary>
    internal class LiveSiteStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveSiteStoreType"/> class.
        /// </summary>
        public LiveSiteStoreType(StoreEntity store)
            : base(store)
        {}

        /// <summary>
        /// Gets the type code for this store
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.LiveSite;
            }
        }

        /// <summary>
        /// Get the log source
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.LiveSite;
            }
        }
    }
}
