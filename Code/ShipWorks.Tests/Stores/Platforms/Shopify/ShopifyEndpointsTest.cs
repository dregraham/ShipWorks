using System.Collections.Generic;
using ShipWorks.Stores.Platforms.Shopify;
using Xunit;

namespace ShipWorks.Tests.Stores.Shopify
{
    public class ShopifyEndpointsTest
    {
        [Fact]
        public void Inventoryurl_ReturnsCorrect()
        {
            ShopifyEndpoints endpoints = new ShopifyEndpoints("test");
            List<long> list = new List<long> {123, 456, 789};
            string url = endpoints.InventoryLevelUrl(list);

            Assert.Equal("https://test.myshopify.com/admin/inventory_levels.json?inventory_item_ids=123,456,789", url);
        }
    }
}