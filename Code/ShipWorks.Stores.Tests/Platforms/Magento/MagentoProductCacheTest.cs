using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Magento
{
    public class MagentoProductCacheTest
    {
        [Fact]
        public void GetStoreProductCache_WhenNoCacheExists_ReturnsNewEmptyCache()
        {
            LruCache<string, Product> testObject = MagentoProductCache.Instance.GetStoreProductCache(1234);

            Assert.Empty(testObject.Keys);
        }

        [Fact]
        public void GetStoreProductCache_IsUniquePerStoreID()
        {
            LruCache<string, Product> testObject1 = MagentoProductCache.Instance.GetStoreProductCache(1234);
            LruCache<string, Product> testObject2 = MagentoProductCache.Instance.GetStoreProductCache(5678);
            Assert.NotEqual(testObject2, testObject1);
        }
    }
}