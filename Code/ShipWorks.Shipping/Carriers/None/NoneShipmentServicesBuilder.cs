using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.None
{
    public class NoneShipmentServicesBuilder : IShipmentServicesBuilder
    {
        /// <summary>
        /// Gets the AvailableServiceTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            return new Dictionary<int, string>();
        }
    }
}