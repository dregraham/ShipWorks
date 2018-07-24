using System.Linq;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Shopify.DTOs;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.Shopify.DTOs
{
    public class ShopifyOrderTest
    {
        [Fact]
        public void Deserialize_PopulatesAllProperties()
        {
            var jsonText = GetType().Assembly.GetEmbeddedResourceString("Stores.Platforms.Shopify.DTOs.ShopifyOrderTest.FullObject.json");
            var testObject = JsonConvert.DeserializeObject<ShopifyOrder>(jsonText);

            var item = testObject.LineItems.SingleOrDefault();
            Assert.NotNull(item);
            Assert.Equal(1335787389033, item.ID);
            Assert.Equal(3111188545, item.VariantID);
        }

        [Fact]
        public void Deserialize_SetsDefaultValues_WhenObjectIsEmpty()
        {
            var testObject = JsonConvert.DeserializeObject<ShopifyOrder>("{}");

            Assert.Empty(testObject.LineItems);
        }
    }
}
