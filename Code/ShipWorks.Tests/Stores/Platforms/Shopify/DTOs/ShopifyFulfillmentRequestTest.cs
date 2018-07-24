using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Shopify.DTOs;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.Shopify.DTOs
{
    public class ShopifyFulfillmentRequestTest
    {
        [Fact]
        public void Serialize_FulfillmentIsSet_WhenObjectExists()
        {
            var fulfillment = new ShopifyFulfillment("T-Test", "C-Test", "CTU-Test", 987, new ShopifyStoreEntity { ShopifyNotifyCustomer = true });
            var testObject = new ShopifyFulfillmentRequest(fulfillment);

            var json = JsonConvert.SerializeObject(testObject);

            var jobject = JObject.Parse(json);

            Assert.NotNull(jobject["fulfillment"]);
        }

        [Fact]
        public void Serialize_FulfillmentIsNotSet_WhenObjectDoesNotExist()
        {
            var testObject = new ShopifyFulfillmentRequest(null);

            var json = JsonConvert.SerializeObject(testObject);

            var jobject = JObject.Parse(json);

            Assert.Null(jobject["fulfillment"]);
        }
    }
}
