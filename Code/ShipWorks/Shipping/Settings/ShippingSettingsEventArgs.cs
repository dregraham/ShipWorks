using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Settings
{
    public class ShippingSettingsEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingSettingsEventArgs"/> class.
        /// </summary>
        /// <param name="shipmentTypeCode">The shipment type code.</param>
        public ShippingSettingsEventArgs(ShipmentTypeCode shipmentTypeCode)
        {
            ShipmentTypeCode = shipmentTypeCode;
        }

        /// <summary>
        /// Gets the shipment type code that triggered the event.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }
    }
}
