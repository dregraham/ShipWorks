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
    public enum FedExSmartPostIndicia
    {
        [Description("FedEx Ground® Economy")]
        [ApiValue("fedex_smartpost_parcel_select")]
        ParcelSelect = 0,

        [Description("FedEx Ground® Economy Media Mail")]
        [ApiValue("fedex_smartpost_media")]
        MediaMail = 1,

        [Description("FedEx Ground® Economy Bound Printed Matter")]
        [ApiValue("fedex_smartpost_bound_printed_matter")]
        BoundPrintedMatter = 2,

        [Description("FedEx Ground® Economy(Under 1lb)")]
        [ApiValue("fedex_smartpost_presorted_standard")]
        PresortedStandard = 3,

        [Description("FedEx Ground® Economy Returns")]
        [ApiValue("fedex_smartpost_returns")]
        ParcelReturn = 4,
    }
}
