using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BigCommerce.DTO;

namespace ShipWorks.Tests.Stores.BigCommerce
{
    /// <summary>
    /// Summary description for BigCommerceProductImageCacheTest
    /// </summary>
    [TestClass]
    public class BigCommerceProductImageCacheTest
    {

        [TestMethod]
        public void EmptyListOfCaches_AddsNewCacheAndReturnsIt_Test()
        {
            LruCache<int, BigCommerceProductImage> newCache = BigCommerceProductImageCache.GetStoreProductImageCache("x", "x", "x");

            Assert.AreEqual(0, newCache.Keys.Count);

            newCache[1] = new BigCommerceProductImage() { Image = "1", ProductID = 1, Thumbnail = "1th" };

            LruCache<int, BigCommerceProductImage> secondCache = BigCommerceProductImageCache.GetStoreProductImageCache("x", "x", "x");

            Assert.AreEqual(1, secondCache.Keys.Count);
        }

        [TestMethod]
        public void TwoStores_GetTheirOwnCache_Test()
        {
            LruCache<int, BigCommerceProductImage> storeAcache = BigCommerceProductImageCache.GetStoreProductImageCache("x", "x", "x");
            LruCache<int, BigCommerceProductImage> storeBcache = BigCommerceProductImageCache.GetStoreProductImageCache("yyy", "yyy", "yyy");

            storeAcache[1] = new BigCommerceProductImage() { Image = "1", ProductID = 1, Thumbnail = "1th" };
            storeAcache[2] = new BigCommerceProductImage() { Image = "2", ProductID = 2, Thumbnail = "2th" };
            storeAcache[3] = new BigCommerceProductImage() { Image = "3", ProductID = 3, Thumbnail = "3th" };
            storeAcache[4] = new BigCommerceProductImage() { Image = "4", ProductID = 4, Thumbnail = "4th" };

            storeBcache[1] = new BigCommerceProductImage() { Image = "500", ProductID = 1, Thumbnail = "500th" };
            storeBcache[2] = new BigCommerceProductImage() { Image = "600", ProductID = 2, Thumbnail = "600th" };
            storeBcache[3] = new BigCommerceProductImage() { Image = "700", ProductID = 3, Thumbnail = "700th" };
            storeBcache[4] = new BigCommerceProductImage() { Image = "800", ProductID = 4, Thumbnail = "800th" };
            storeBcache[5] = new BigCommerceProductImage() { Image = "900", ProductID = 5, Thumbnail = "900th" };
            storeBcache[6] = new BigCommerceProductImage() { Image = "9999", ProductID = 6, Thumbnail = "9999th" };

            storeAcache = BigCommerceProductImageCache.GetStoreProductImageCache("x", "x", "x");
            storeBcache = BigCommerceProductImageCache.GetStoreProductImageCache("yyy", "yyy", "yyy");

            Assert.AreEqual("1th", storeAcache[1].Thumbnail);
            Assert.AreEqual("500th", storeBcache[1].Thumbnail);

            Assert.AreEqual("4th", storeAcache[4].Thumbnail);
            Assert.AreEqual("9999th", storeBcache[6].Thumbnail);

            Assert.AreEqual(null, storeAcache[6]);
        }
    }
}
