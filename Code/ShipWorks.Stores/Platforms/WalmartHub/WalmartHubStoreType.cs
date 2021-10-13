using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Stores.Platforms.WalmartHub
{
    /// <summary>
    /// WalmartHub Store type
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.WalmartHub)]
    [Component(RegistrationType.Self)]
    public class WalmartHubStoreType : PlatformStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WalmartHubStoreType() : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WalmartHubStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// WalmartHub StoreTypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.WalmartHub;
    }
}
