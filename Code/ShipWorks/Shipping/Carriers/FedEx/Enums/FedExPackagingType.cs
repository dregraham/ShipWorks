using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExPackagingType
    {
        [Description("FedEx® Envelope")]
        [ApiValue("fedex_envelope")]
        Envelope = 0,

        [Description("FedEx® Pak")]
        [ApiValue("fedex_pak")]
        Pak = 1,

        [Description("FedEx® Box")]
        [ApiValue("fedex_box")]
        Box = 2,

        [Description("FedEx® Tube")]
        [ApiValue("fedex_tube")]
        Tube = 3,

        [Description("FedEx® 10kg Box")]
        [ApiValue("fedex_10kg_box")]
        Box10Kg = 4,

        [Description("FedEx® 25kg Box")]
        [ApiValue("fedex_25kg_box")]
        Box25Kg = 5,

        [Description("Your Packaging")]
        [ApiValue("package")]
        Custom = 6,

        [Description("FedEx Ground® Economy Mail")]
        SPMediaMail = 7,

        [Description("FedEx Ground® Economy Select")]
        SPParcelSelect = 8,

        [Description("FedEx Ground® Economy Presorted  BPM")]
        SPPresortedBPM = 9,

        [Description("FedEx Ground® Economy Presorted Standard")]
        SPPresortedStandard = 10,

        [Description("FedEx® Small Box")]
        [ApiValue("fedex_small_box")]
        SmallBox = 11,

        [Description("FedEx® Medium Box")]
        [ApiValue("fedex_medium_box")]
        MediumBox = 12,

        [Description("FedEx® Large Box")]
        [ApiValue("fedex_large_box")]
        LargeBox = 13,

        [Description("FedEx® Extra Large Box")]
        [ApiValue("fedex_extra_large_box")]
        ExtraLargeBox = 14
    }
}
