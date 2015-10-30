using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Manages all ShipmentTypes available in ShipWorks
    /// </summary>
    public interface IShipmentTypeManager
    {
        /// <summary>
        /// Returns all shipment types in ShipWorks
        /// </summary>
        List<ShipmentType> ShipmentTypes { get; }
    }
}
