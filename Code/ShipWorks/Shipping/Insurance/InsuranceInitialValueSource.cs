using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Source for where to initially populate an insurance (or declared) value
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum InsuranceInitialValueSource
    {
        [Description("Item Subtotal")]
        ItemSubtotal = 0,

        [Description("Order Total")]
        OrderTotal = 1,

        [Description("Other Amount")]
        OtherAmount = 2
    }
}
