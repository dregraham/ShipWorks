using Newtonsoft.Json;

namespace ShipWorks.Api.Orders.Shipments
{
    /// <summary>
    /// An object that hold a Shipments label image and name
    /// </summary>
    public class LabelData
    {
        /// <summary>
        /// The Labels name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The Labels Base64 encoded image
        /// </summary>
        [JsonProperty("image")]
        public string Image { get; set; }
    }
}
