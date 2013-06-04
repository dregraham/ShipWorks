using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// Various states a WorldShip shipment can be in
    /// </summary>
    public enum WorldShipStatusType
    {
        /// <summary>
        /// Either the shipment is not a worldShip shipment (UPSOLT), or it has not yet been exported to worldship.
        /// </summary>
        None = 0,

        /// <summary>
        /// The shipment has been exported to worldship but not yet processed in WorldShip
        /// </summary>
        Exported = 1,

        /// <summary>
        /// The shipment has been exported to worldship, and the results of being processed in worldship have bee imported.
        /// </summary>
        Completed = 2,

        /// <summary>
        /// The shipment has been voided in worldship.
        /// </summary>
        Voided = 3
    }
}
