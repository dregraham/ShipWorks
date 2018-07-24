using System.Linq;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Shopify.DTOs;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.Shopify.DTOs
{
    public class ShopifyInventoryLevelsResponseTest
    {
        [Fact]
        public void Deserialize_PopulatesAllProperties()
        {
            var testObject = JsonConvert.DeserializeObject<ShopifyInventoryLevelsResponse>("{ inventory_levels: [{}] }");

            Assert.Equal(1, testObject.InventoryLevels.Count());
        }

        [Fact]
        public void Deserialize_SetsDefaultValues_WhenObjectIsEmpty()
        {
            var testObject = JsonConvert.DeserializeObject<ShopifyInventoryLevelsResponse>("{}");

            Assert.Empty(testObject.InventoryLevels);
        }
    }
}
