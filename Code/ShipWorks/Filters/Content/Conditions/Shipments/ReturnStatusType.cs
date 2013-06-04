using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ReturnStatusType
    {
        [Description("No")]
        RegularShipment = 0,

        [Description("Yes")]
        ReturnShipment = 1
    }
}
