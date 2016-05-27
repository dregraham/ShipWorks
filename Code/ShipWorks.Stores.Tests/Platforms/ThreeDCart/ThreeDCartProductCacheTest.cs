using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ThreeDCart
{
    public class ThreeDCartProductCacheTest
    {
        readonly ThreeDCartProductCache cache = new ThreeDCartProductCache();
        private readonly string url = "url";
        private readonly string token = "token";

        [Fact]
        public void GetStoreProductWeightCache_ReturnsNewLruCache_WhenGivenNewStoreID()
        {
            Assert.IsAssignableFrom<LruCache<int, ThreeDCartProduct>>(cache.GetStoreProductCache(url, token));
            Assert.Equal(0, cache.GetStoreProductCache(url, token).Keys.Count);
        }

        [Fact]
        public void GetStoreProductWeightCache_ReturnsExistingLruCache_WhenGivenExistingStoreID()
        {
            LruCache<int, ThreeDCartProduct> lruCache = cache.GetStoreProductCache(url, token);

            lruCache[1] = new ThreeDCartProduct() { MainImageFile = "url/to/file.png"};

            Assert.Equal(1, cache.GetStoreProductCache(url, token).Keys.Count);
            Assert.Equal("url/to/file.png", cache.GetStoreProductCache(url, token)[1].MainImageFile);
        }
    }
}