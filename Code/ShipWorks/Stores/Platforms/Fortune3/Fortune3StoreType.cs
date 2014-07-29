using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Fortune3
{
    /// <summary>
    /// Fortune3StoreType
    /// </summary>
    public class Fortune3StoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fortune3StoreType"/> class.
        /// </summary>
        public Fortune3StoreType(StoreEntity store)
            : base(store)
        {}


        /// <summary>
        /// Gets the type code for this store
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.Fortune3; 
            }
        }


        /// <summary>
        /// Get the log source
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.Fortune3;
            }
        }

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl
        {
            get { return "http://support.shipworks.com/solution/categories/105240/folders/4000004783/articles/4000022391-connecting-fortune3-with"; }
        }
    }
}
