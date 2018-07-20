using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Shopify.DTOs;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.Shopify.DTOs
{
    public class ShopifyInventoryLevelTest
    {
        [Fact]
        public void Deserialize_PopulatesAllProperties()
        {
            var jsonText = GetType().Assembly.GetEmbeddedResourceString("Stores.Platforms.Shopify.DTOs.ShopifyInventoryLevelTest.FullObject.json");
            var testObject = JsonConvert.DeserializeObject<ShopifyInventoryLevel>(jsonText);

            Assert.Equal(2, testObject.Available);
            Assert.Equal(49582922, testObject.LocationID);
            Assert.Equal(1411223617546, testObject.InventoryItemID);
        }

        [Fact]
        public void Deserialize_SetsDefaultValues_WhenObjectIsEmpty()
        {
            var testObject = JsonConvert.DeserializeObject<ShopifyInventoryLevel>("{}");

            Assert.Null(testObject.Available);
            Assert.Equal(0, testObject.LocationID);
            Assert.Equal(0, testObject.InventoryItemID);
        }
    }
}
