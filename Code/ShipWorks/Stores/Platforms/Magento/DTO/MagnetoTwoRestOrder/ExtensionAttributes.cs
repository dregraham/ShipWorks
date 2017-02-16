using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class ExtensionAttributes
    {
        [JsonProperty("shipping_assignments")]
        public IEnumerable<ShippingAssignment> ShippingAssignments { get; set; }

        [JsonProperty("custom_options")]
        public IEnumerable<CustomOption> CustomOptions { get; set; }
    }
}