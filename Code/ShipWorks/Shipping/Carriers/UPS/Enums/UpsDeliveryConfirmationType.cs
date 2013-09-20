using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Possible UPS delivery confirmations
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsDeliveryConfirmationType
    {
        [Description("None")]
        None = 0,

        [Description("No Signature")]
        NoSignature = 1,

        [Description("Signature Required")]
        Signature = 2,

        [Description("Adult Signature Required")]
        AdultSignature = 3,

        [Description("USPS Delivery Confirmation")]
        UspsDeliveryConfirmation = 4
    }
}
