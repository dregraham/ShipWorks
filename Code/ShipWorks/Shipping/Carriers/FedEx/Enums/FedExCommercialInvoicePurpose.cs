using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExCommercialInvoicePurpose
    {
        [Description("Sold")]
        Sold = 0,

        [Description("Not Sold")]
        NotSold = 1,

        [Description("Gift")]
        Gift = 2,

        [Description("Sample")]
        Sample = 3,

        [Description("Return and Repair")]
        Repair = 4,

        [Description("Personal")]
        Personal = 5
    }
}
