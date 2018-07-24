using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Shopify.DTOs;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.Shopify.DTOs
{
    public class ShopifyFulfillmentTest
    {
        [Fact]
        public void Serialize_UsesAllProperties()
        {
            var testObject = new ShopifyFulfillment("T-Test", "C-Test", "CTU-Test", 987, new ShopifyStoreEntity { ShopifyNotifyCustomer = true });
            var json = JsonConvert.SerializeObject(testObject);

            var jobject = JObject.Parse(json);

            Assert.Equal("T-Test", jobject.Value<string>("tracking_number"));
            Assert.Equal("C-Test", jobject.Value<string>("tracking_company"));
            Assert.Equal("CTU-Test", jobject.Value<string>("custom_tracking_url"));
            Assert.Equal(true, jobject.Value<bool>("notify_customer"));
            Assert.Equal(987, jobject.Value<long>("location_id"));

            Assert.Null(jobject["line_items"]);
        }

        [Fact]
        public void Serialize_IncludesItemIDs_WhenItemsNotNull()
        {
            var testObject = new ShopifyFulfillment("T-Test", "C-Test", "CTU-Test", 987, new ShopifyStoreEntity { ShopifyNotifyCustomer = true });
            var itemTestObject = testObject.WithLocation(678, new[]
            {
                new ShopifyOrderItemEntity { ShopifyOrderItemID = 6 },
                new ShopifyOrderItemEntity { ShopifyOrderItemID = 7 }
            });
            var json = JsonConvert.SerializeObject(itemTestObject);

            var jobject = JObject.Parse(json);

            Assert.Equal("T-Test", jobject.Value<string>("tracking_number"));
            Assert.Equal("C-Test", jobject.Value<string>("tracking_company"));
            Assert.Equal("CTU-Test", jobject.Value<string>("custom_tracking_url"));
            Assert.Equal(true, jobject.Value<bool>("notify_customer"));
            Assert.Equal(678, jobject.Value<long>("location_id"));

            Assert.Equal(2, jobject["line_items"].Count());
            Assert.Contains(6, jobject["line_items"].Select(x => x["id"].Value<long>()));
            Assert.Contains(7, jobject["line_items"].Select(x => x["id"].Value<long>()));
        }
    }
}
