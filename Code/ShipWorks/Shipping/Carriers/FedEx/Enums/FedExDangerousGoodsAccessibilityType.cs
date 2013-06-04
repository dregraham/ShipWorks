using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExDangerousGoodsAccessibilityType
    {
        [Description("Accessible")]
        Accessible = 0,

        [Description("Inaccessible")]
        Inaccessible = 1,

        [Description("N/A")]
        NotApplicable = 99
    }
}
