using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipmentInsuredType
    {
        [Description("Not Insured")]
        None = 0,

        [Description("ShipWorks")]
        ShipWorks = 1,

        [Description("Carrier")]
        Carrier = 2
    }
}
