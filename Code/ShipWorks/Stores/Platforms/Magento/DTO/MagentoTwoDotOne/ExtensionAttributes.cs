using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class ExtensionAttributes : IExtensionAttributes
    {
        public ExtensionAttributes(IEnumerable<ShippingAssignment> shippingAssignments)
        {
            ShippingAssignments = shippingAssignments;
        }

        [JsonProperty("shipping_assignments")]
        public IEnumerable<IShippingAssignment> ShippingAssignments { get; set; }
    }
}