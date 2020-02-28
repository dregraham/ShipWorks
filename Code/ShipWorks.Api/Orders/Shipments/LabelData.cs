using Newtonsoft.Json;

namespace ShipWorks.Api.Orders.Shipments
{
    /// <summary>
    /// An object that hold a Shipment's label image and name
    /// </summary>
    public class LabelData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LabelData(string name, string image)
        {
            Name = name;
            Image = image;
        }

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
