using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Zenventory
{
    public class ZenventoryStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZenventoryStoreType"/> class.
        /// </summary>
        public ZenventoryStoreType(StoreEntity store) 
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
                return StoreTypeCode.Zenventory;
            }
        }


        /// <summary>
        /// Get the log source
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.Zenventory;
            }
        }
    }
}
