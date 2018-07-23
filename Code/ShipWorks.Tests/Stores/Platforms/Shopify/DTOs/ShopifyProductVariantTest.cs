using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Shopify.DTOs;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.Shopify.DTOs
{
    public class ShopifyProductVariantTest
    {
        [Fact]
        public void Deserialize_PopulatesAllProperties()
        {
            var jsonText = GetType().Assembly.GetEmbeddedResourceString("Stores.Platforms.Shopify.DTOs.ShopifyProductVariantTest.FullObject.json");
            var testObject = JsonConvert.DeserializeObject<ShopifyProductVariant>(jsonText);

            Assert.Equal(1380328669194, testObject.ID);
            Assert.Equal(1411223617546, testObject.InventoryItemID);
            Assert.Equal("1", testObject.Barcode);
        }

        [Fact]
        public void Deserialize_SetsDefaultValues_WhenObjectIsEmpty()
        {
            var testObject = JsonConvert.DeserializeObject<ShopifyProductVariant>("{}");

            Assert.Equal(0, testObject.ID);
            Assert.Equal(0, testObject.InventoryItemID);
            Assert.Empty(testObject.Barcode);
        }
    }
}
