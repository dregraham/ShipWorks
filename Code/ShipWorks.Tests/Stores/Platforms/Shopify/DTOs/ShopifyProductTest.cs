using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Shopify.DTOs;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.Shopify.DTOs
{
    public class ShopifyProductTest
    {
        [Fact]
        public void Deserialize_PopulatesAllProperties()
        {
            var jsonText = GetType().Assembly.GetEmbeddedResourceString("Stores.Platforms.Shopify.DTOs.ShopifyProductTest.FullObject.json");
            var testObject = JsonConvert.DeserializeObject<ShopifyProduct>(jsonText);

            Assert.Equal(4, testObject.Images.Length);
            Assert.Equal(3, testObject.Variants.Length);
        }

        [Fact]
        public void Deserialize_SetsDefaultValues_WhenObjectIsEmpty()
        {
            var testObject = JsonConvert.DeserializeObject<ShopifyProduct>("{}");

            Assert.Empty(testObject.Variants);
            Assert.Empty(testObject.Images);
            Assert.Null(testObject.Image);
        }
    }
}
