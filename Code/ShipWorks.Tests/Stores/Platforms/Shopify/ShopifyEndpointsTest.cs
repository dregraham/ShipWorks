using System.Collections.Generic;
using ShipWorks.Stores.Platforms.Shopify;
using Xunit;

namespace ShipWorks.Tests.Stores.Shopify
{
    public class ShopifyEndpointsTest
    {
        [Fact]
        public void InventoryLevelForItemsUrl_ReturnsCorrect()
        {
            ShopifyEndpoints endpoints = new ShopifyEndpoints("test");
            List<long> list = new List<long> { 123, 456, 789 };
            string url = endpoints.InventoryLevelForItemsUrl(list);

            Assert.Equal("https://test.myshopify.com/admin/inventory_levels.json?inventory_item_ids=123,456,789", url);
        }

        [Fact]
        public void InventoryLevelForLocationsUrl_ReturnsCorrect()
        {
            ShopifyEndpoints endpoints = new ShopifyEndpoints("test");

            string url = endpoints.InventoryLevelForLocationsUrl(new[] { 123L, 456L, 789L });

            Assert.Equal("https://test.myshopify.com/admin/inventory_levels.json?location_ids=123,456,789", url);
        }
    }
}