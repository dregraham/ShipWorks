using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.FoxyCart
{
    /// <summary>
    /// FoxyCart Store Type
    /// </summary>
    public class FoxyCartStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FoxyCartStoreType"/> class.
        /// </summary>
        /// <param name="store"></param>
        public FoxyCartStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Gets the type code for this store
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.FoxyCart;
            }
        }

        /// <summary>
        /// Get the log source
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.FoxyCart;
            }
        }
    }
}
