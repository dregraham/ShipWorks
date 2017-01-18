using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Magento
{
    public class MagentoProductCacheTest
    {
        [Fact]
        public void GetStoreProductCache_WhenNoCacheExists_ReturnsNewEmptyCache()
        {
            MagentoProductCache testObject = new MagentoProductCache();
            LruCache<string, Product> cache = testObject.GetStoreProductCache(1234);

            Assert.Empty(cache.Keys);
        }

        [Fact]
        public void GetStoreProductCache_IsUniquePerStoreID()
        {
            MagentoProductCache testObject = new MagentoProductCache();

            LruCache<string, Product> testObject1 = testObject.GetStoreProductCache(1234);
            LruCache<string, Product> testObject2 = testObject.GetStoreProductCache(5678);
            Assert.NotEqual(testObject2, testObject1);
        }

        [Fact]
        public void InitializeForCurrentDatabase_ClearsCache()
        {
            MagentoProductCache testObject = new MagentoProductCache();
            long storeid = 12345678;
            Product product = new Product() { ID = 123123 };

            LruCache<string, Product> cache = testObject.GetStoreProductCache(storeid);
            cache["sku123"] = product;

            testObject.InitializeForCurrentDatabase(new TestExecutionMode());

            Assert.False(testObject.GetStoreProductCache(storeid).Contains("sku123"));
        }
    }
}