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
    /// Predefined Customer Classification Code
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsCustomerClassificationCode
    {
        [ApiValue("01")]
        [Description("Business")]
        Business = 0,

        [ApiValue("02")]
        [Description("Personal")]
        Personal = 1
    }
}
