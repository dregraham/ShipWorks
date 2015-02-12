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
    /// Predefined carrier type being replaced by UPS
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsOtherCarrierType
    {
        [ApiValue("31")]
        [Description("DHL")]
        Dhl = 0,

        [ApiValue("32")]
        [Description("FedEx")]
        FedEx = 1,

        [ApiValue("33")]
        [Description("USPS")]
        Usps = 2,

        [ApiValue("34")]
        [Description("Other")]
        Other = 3
    }
}
