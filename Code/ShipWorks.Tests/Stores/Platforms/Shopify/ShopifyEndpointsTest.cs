using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Shopify;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Stores.Shopify
{
    public class ShopifyEndpointsTest
    {
        private ShopifyEndpoints endpoints = new ShopifyEndpoints("test");
        private IEnumerable<long> InventoryItemIDs;

        [Fact]
        public void Inventoryurl_ReturnsCorrect()
        {
            List<long> list = new List<long>();
            list.Add(123);
            list.Add(456);
            list.Add(789);

            InventoryItemIDs = list;

            string url = endpoints.InventoryLevelUrl(InventoryItemIDs);

            Assert.Equal("https://test.myshopify.com/admin/inventory_levels.json?inventory_item_ids=123,456,789", url);
        }
    }
}