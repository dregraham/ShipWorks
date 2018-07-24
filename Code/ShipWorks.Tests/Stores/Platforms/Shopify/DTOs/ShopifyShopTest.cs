using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Shopify.DTOs;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.Shopify.DTOs
{
    public class ShopifyShopTest
    {
        [Fact]
        public void Deserialize_PopulatesAllProperties()
        {
            var jsonText = GetType().Assembly.GetEmbeddedResourceString("Stores.Platforms.Shopify.DTOs.ShopifyShopTest.FullObject.json");
            var testObject = JsonConvert.DeserializeObject<ShopifyShop>(jsonText);

            Assert.Equal("Joanie Loves Tchotchkes", testObject.StoreName);
            Assert.Equal("1 Memorial Drive", testObject.Street1);
            Assert.Equal("St. Louis", testObject.City);
            Assert.Equal("Missouri", testObject.StateProvince);
            Assert.Equal("63102", testObject.PostalCode);
            Assert.Equal("US", testObject.Country);
            Assert.Equal("support@ShipWorks.com", testObject.Email);
            Assert.Equal("800-952-7784", testObject.Phone);
            Assert.Equal(49582922, testObject.PrimaryLocationID);
        }

        [Fact]
        public void Deserialize_SetsDefaultValues_WhenObjectIsEmpty()
        {
            var testObject = JsonConvert.DeserializeObject<ShopifyShop>("{}");

            Assert.Equal("Shopify Store", testObject.StoreName);
            Assert.Equal("", testObject.Street1);
            Assert.Equal("", testObject.City);
            Assert.Equal("", testObject.StateProvince);
            Assert.Equal("", testObject.PostalCode);
            Assert.Equal("", testObject.Country);
            Assert.Equal("", testObject.Email);
            Assert.Equal("", testObject.Phone);
            Assert.Equal(0, testObject.PrimaryLocationID);
        }
    }
}
