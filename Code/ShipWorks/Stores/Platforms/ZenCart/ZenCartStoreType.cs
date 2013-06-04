using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.ZenCart
{
    /// <summary>
    /// Implementation of the Zen Cart integration
    /// </summary>
    public class ZenCartStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ZenCartStoreType(StoreEntity store)
            : base(store)
        {
          
        }

        /// <summary>
        /// Gets the store typecode for this store
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.ZenCart;
            }
        }

        /// <summary>
        /// Log request/responses as ZenCart
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.ZenCart;
            }
        }
    }
}
