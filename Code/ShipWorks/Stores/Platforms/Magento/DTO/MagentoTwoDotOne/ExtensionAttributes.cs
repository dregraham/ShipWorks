using System.Collections.Generic;
using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class ExtensionAttributes : IExtensionAttributes
    {
        [JsonProperty("shipping_assignments")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<IShippingAssignment>, List<ShippingAssignment>>))]
        public IEnumerable<IShippingAssignment> ShippingAssignments { get; set; }
    }
}