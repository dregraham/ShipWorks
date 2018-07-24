using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Shopify.DTOs;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.Shopify.DTOs
{
    public class ShopifyProductImageTest
    {
        [Fact]
        public void Deserialize_PopulatesAllProperties()
        {
            var jsonText = GetType().Assembly.GetEmbeddedResourceString("Stores.Platforms.Shopify.DTOs.ShopifyProductImageTest.FullObject.json");
            var testObject = JsonConvert.DeserializeObject<ShopifyProductImage>(jsonText);

            Assert.Equal("https://cdn.shopify.com/s/files/1/0195/8594/products/green.png?v=1516640762", testObject.Src);
            Assert.Contains(3, testObject.VariantIDs);
            Assert.Contains(4, testObject.VariantIDs);
            Assert.Contains(5, testObject.VariantIDs);
        }

        [Fact]
        public void Deserialize_SetsDefaultValues_WhenObjectIsEmpty()
        {
            var testObject = JsonConvert.DeserializeObject<ShopifyProductImage>("{}");

            Assert.Empty(testObject.Src);
            Assert.Empty(testObject.VariantIDs);
        }
    }
}
