using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class ExtensionAttributes : IExtensionAttributes
    {
        [JsonProperty("shipping_assignments")]
        public IList<IShippingAssignment> ShippingAssignments { get; set; }
    }
}