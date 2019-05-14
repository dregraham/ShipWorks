using System.Collections.Generic;
using System.Linq;
using ShipWorks.Stores;

namespace ShipWorks.Warehouse
{
    /// <summary>
    /// StoreTypecodes that are supported by warehouse mode
    /// </summary>
    public static class WarehouseStoreTypes
    {
        /// <summary>
        /// The store types that are supported by Warehouse
        /// </summary>
        private static IEnumerable<StoreTypeCode> supportedStoreTypes =>
            new[] { StoreTypeCode.Amazon, StoreTypeCode.ChannelAdvisor };

        /// <summary>
        /// Is the given StoreTypeCode supported
        /// </summary>
        public static bool IsSupported(StoreTypeCode storeTypeCode) =>
            supportedStoreTypes.Contains(storeTypeCode);
    }
}
