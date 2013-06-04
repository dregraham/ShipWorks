using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExPayorType
    {
        [Description("Sender")]
        Sender = 0,

        [Description("Recipient")]
        Recipient = 1,

        [Description("Third Party")]
        ThirdParty = 2,

        [Description("Collect")]
        Collect = 3
    }
}
