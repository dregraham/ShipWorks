using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Stores.Platforms.VolusionHub
{
    /// <summary>
    /// VolusionHub Store type
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.VolusionHub)]
    [Component(RegistrationType.Self)]
    public class VolusionHubStoreType : PlatformStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionHubStoreType() : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionHubStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// VolusionHub StoreTypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.VolusionHub;
    }
}