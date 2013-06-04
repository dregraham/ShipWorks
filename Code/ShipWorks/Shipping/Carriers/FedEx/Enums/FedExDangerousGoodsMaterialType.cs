using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExDangerousGoodsMaterialType
    {
        [Description("Lithium Batteries")]
        LithiumBatteries = 0,

        [Description("Hazardous Materials")]
        HazardousMaterials = 1, 

        [Description("ORM-D")]
        OrmD = 2,

        [Description("N/A")]
        NotApplicable = 99
    }
}
