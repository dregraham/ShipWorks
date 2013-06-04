using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// Signature confirmation type
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExSignatureType
    {
        [Description("Service Default")]
        ServiceDefault = 0,

        [Description("No Signature Required")]
        NoSignature = 1,

        [Description("Indirect Signature Required")]
        Indirect = 2,

        [Description("Direct Signature Required")]
        Direct = 3,

        [Description("Adult Signature Required")]
        Adult = 4
    }
}
