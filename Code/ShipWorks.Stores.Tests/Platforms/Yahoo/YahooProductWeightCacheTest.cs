﻿using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Yahoo
{
    public class YahooProductWeightCacheTest
    {
        readonly YahooProductWeightCache cache = new YahooProductWeightCache();
        private readonly string storeID = "yhst-12345";

        [Fact]
        public void GetStoreProductWeightCache_ReturnsNewLruCache_WhenGivenNewStoreID()
        {
            Assert.IsAssignableFrom<LruCache<string, YahooCatalogItem>>(cache.GetStoreProductWeightCache(storeID));
            Assert.Equal(0, cache.GetStoreProductWeightCache(storeID).Keys.Count);
        }

        [Fact]
        public void GetStoreProductWeightCache_ReturnsExistingLruCache_WhenGivenExistingStoreID()
        {
            LruCache<string, YahooCatalogItem> lruCache = cache.GetStoreProductWeightCache(storeID);

            lruCache["1"] = new YahooCatalogItem() {ShipWeightTransport = "5"};

            Assert.Equal(1, cache.GetStoreProductWeightCache(storeID).Keys.Count);
            Assert.Equal(5, cache.GetStoreProductWeightCache(storeID)["1"].ShipWeight);
        }
    }
}
