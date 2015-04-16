using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.Postal
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum PostalConfirmationType
    {
        [Description("None")]
        None = 0,

        [Description("Delivery Confirmation")]
        Delivery = 1,

        [Description("Signature Confirmation")]
        Signature = 2,

        [Description("Adult Signature Required")]
        AdultSignatureRequired = 3,

        [Description("Adult Signature Restricted Delivery")]
        AdultSignatureRestricted = 4
    }
}
