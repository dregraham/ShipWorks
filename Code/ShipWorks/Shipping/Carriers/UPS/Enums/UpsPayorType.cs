using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Valid payors for a ups shipment
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsPayorType
    {
        [Description("Sender")]
        Sender = 0,

        [Description("Third Party")]
        ThirdParty = 1,

        [Description("Receiver")]
        Receiver = 2
    }
}
