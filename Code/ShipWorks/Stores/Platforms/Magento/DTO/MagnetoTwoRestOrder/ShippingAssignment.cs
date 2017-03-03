using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class ShippingAssignment
    {
        [JsonProperty("shipping")]
        public Shipping Shipping { get; set; }
    }
}