using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class JetProduct
    {
        // Get from product repo
        // Description
        // ISBN or UPC

        [JsonProperty("mail_image_url")]
        public string MainImageUrl { get; set; }

        [JsonProperty("swatch_image_url")]
        public string SwatchImageUrl { get; set; }

        [JsonProperty("shipping_weight_pounds")]
        public double ShippingWeightPounds { get; set; }

        [JsonProperty("standard_product_codes")]
        public IEnumerable<JetProductCode> StandardProductCodes { get; set; }

        [JsonProperty("product_description")]
        public string ProductDescription { get; set; }
    }
}