using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Stores.Platforms.BrightpearlHub
{
    /// <summary>
    /// BrightpearlHub Store type
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.BrightpearlHub)]
    [Component(RegistrationType.Self)]
    public class BrightpearlHubStoreType : PlatformStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BrightpearlHubStoreType() : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BrightpearlHubStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// BrightpearlHub StoreTypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.BrightpearlHub;
    }
}