using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Enums
{
    /// <summary>
    /// ACH type
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum Express1AchType
    {
        [Description("Checking")]
        Checking = 0,

        [Description("Savings")]
        Savings = 1
    }
}
