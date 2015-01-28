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
    /// Yes and No as defined in the UpsOpenAccount API
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsYesNoType
    {
        [ApiValue("0")] 
        [Description("No")]
        No = 0,

        [ApiValue("1")]
        [Description("Yes")]
        Yes = 1
    }
}
