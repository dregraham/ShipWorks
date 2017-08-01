using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class JetProduct
    {
        [JsonProperty("swatch_image_url")]
        public string ImageUrl { get; set; }
    }
}