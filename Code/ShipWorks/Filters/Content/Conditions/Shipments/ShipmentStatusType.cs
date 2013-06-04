using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipmentStatusType
    {
        [Description("Not Processed")]
        None = 0,

        [Description("Processed")]
        Processed = 1,

        [Description("Voided")]
        Voided = 2
    }
}
