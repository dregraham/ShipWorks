using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.PayPal
{
    /// <summary>
    /// Address Verification from PayPal
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum PayPalAddressStatus
    {
        [Description("")]
        [ImageResource("blank16")]
        None = 0,

        [Description("Confirmed")]
        [ImageResource("check16")]
        Confirmed = 1,

        [Description("Unconfirmed")]
        [ImageResource("delete16")]
        Unconfirmed = 2
    }
}
