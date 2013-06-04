using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// What to do when a shipment has a blank phone number
    /// </summary>
    enum ShipmentBlankPhoneOption
    {
        /// <summary>
        /// Use the phone number of the shipper
        /// </summary>
        ShipperPhone = 0,

        /// <summary>
        /// Use a specified phone number
        /// </summary>
        SpecifiedPhone = 1
    }
}
