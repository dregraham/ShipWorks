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
        public void GetStoreProductCache_WhenNoSKUCacheExists_ReturnsNewEmptyCache()
        {
            MagentoProductCache testObject = new MagentoProductCache();
            LruCache<string, Product> cache = testObject.GetStoreProductBySkuCache(1234);

            Assert.Empty(cache.Keys);
        }

        [Fact]
        public void GetStoreProductCache_WhenNoIdCacheExists_ReturnsNewEmptyCache()
        {
            MagentoProductCache testObject = new MagentoProductCache();
            LruCache<int, Product> cache = testObject.GetStoreProductByIdCache(1234);

            Assert.Empty(cache.Keys);
        }

        [Fact]
        public void GetStoreProductCache_IsUniquePerStoreID()
        {
            MagentoProductCache testObject = new MagentoProductCache();

            LruCache<string, Product> testObject1 = testObject.GetStoreProductBySkuCache(1234);
            LruCache<string, Product> testObject2 = testObject.GetStoreProductBySkuCache(5678);
            Assert.NotEqual(testObject2, testObject1);

            LruCache<int, Product> testObject3 = testObject.GetStoreProductByIdCache(1234);
            LruCache<int, Product> testObject4 = testObject.GetStoreProductByIdCache(5678);
            Assert.NotEqual(testObject3, testObject4);
        }

        [Fact]
        public void InitializeForCurrentDatabase_ClearsCache()
        {
            MagentoProductCache testObject = new MagentoProductCache();
            long storeid = 12345678;
            Product product = new Product() { ID = 123123 };

            LruCache<string, Product> skuCache = testObject.GetStoreProductBySkuCache(storeid);
            skuCache["sku123"] = product;

            LruCache<int, Product> idCache = testObject.GetStoreProductByIdCache(storeid);
            idCache[123123] = product;

            testObject.InitializeForCurrentDatabase(new TestExecutionMode());

            Assert.False(testObject.GetStoreProductBySkuCache(storeid).Contains("sku123"));
            Assert.False(testObject.GetStoreProductByIdCache(storeid).Contains(123123));
        }
    }
}