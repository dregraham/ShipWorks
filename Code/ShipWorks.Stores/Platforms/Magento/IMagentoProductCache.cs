using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Represents a Magento product cache
    /// </summary>
    public interface IMagentoProductCache
    {
        /// <summary>
        /// Retrieve a cache for the given store
        /// </summary>
        LruCache<string, Product> GetStoreProductCache(long storeID);
    }
}