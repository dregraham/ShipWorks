using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    public class ThreeDCartSkuInfo
    {
        [JsonProperty("SKU")]
        public string Sku { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}