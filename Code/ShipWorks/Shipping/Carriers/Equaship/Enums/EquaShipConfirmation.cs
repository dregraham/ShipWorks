using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.EquaShip.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    enum EquaShipConfirmationType
    {
        [Description("None")]
        None = 0,

        [Description("Delivery Confirmation")]
        Delivery = 1,

        [Description("Signature Confirmation")]
        Signature = 2
    }
}
