using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class JetProductCodes
    {
        [JsonProperty("standard_product_code_type")]
        public string StandardProductCodeType { get; set; }
        
        [JsonProperty("standard_product_code")]
        public string StandardProductCode { get; set; }
    }
}