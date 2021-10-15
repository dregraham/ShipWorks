using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Stores.Platforms.GrouponHub
{

    /// <summary>
    /// GrouponHub Store type
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.GrouponHub)]
    public class GrouponHubStoreType : PlatformStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponHubStoreType() : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponHubStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// GrouponHub StoreTypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.GrouponHub;
    }
}
