using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Enum for UPS Irrigular Indicator
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsIrregularIndicatorType
    {
        [Description("Not Applicable")]
        [ApiValue("3")]
        NotApplicable = 0,

        [Description("Balloon")]
        [ApiValue("1")]
        Balloon = 1,

        [Description("Oversize")]
        [ApiValue("2")]
        Oversize = 2,
    }
}
