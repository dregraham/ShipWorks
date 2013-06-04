using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExHazardousMaterialsPackingGroup
    {
        [Description("Default")]
        Default = 0,

        [Description("I")]
        I = 1,

        [Description("II")]
        II = 2,

        [Description("III")]
        III = 3
    }
}
