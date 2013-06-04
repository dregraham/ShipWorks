using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsDeliveryConfirmationEntityLevel
    {
        [Description("Shipment")]
        Shipment = 0,

        [Description("Package")]
        Package = 1
    }
}
