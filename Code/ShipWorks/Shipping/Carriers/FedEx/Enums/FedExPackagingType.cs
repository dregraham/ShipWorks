using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExPackagingType
    {
        [Description("FedEx Envelope")]
        Envelope = 0,

        [Description("FedEx Pak")]
        Pak = 1,

        [Description("FedEx Box")]
        Box = 2,

        [Description("FedEx Tube")]
        Tube = 3,

        [Description("FedEx 10kg Box")]
        Box10Kg = 4,

        [Description("FedEx 25kg Box")]
        Box25Kg = 5,

        [Description("Your Packaging")]
        Custom = 6,

        [Description("SmartPost Media Mail")]
        SPMediaMail = 7,

        [Description("SmartPost Parcel Select")]
        SPParcelSelect = 8,

        [Description("SmartPost Presorted  BPM")]
        SPPresortedBPM = 9,

        [Description("SmartPost Presorted Standard")]
        SPPresortedStandard = 10
    }
}
