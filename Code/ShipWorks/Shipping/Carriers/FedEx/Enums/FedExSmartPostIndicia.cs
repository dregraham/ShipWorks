using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExSmartPostIndicia
    {
        [Description("Parcel Select")]
        ParcelSelect = 0,

        [Description("Media Mail")]
        MediaMail = 1,

        [Description("Bound Printed Matter")]
        BoundPrintedMatter = 2,

        [Description("Presorted Standard")]
        PresortedStandard = 3,

        [Description("Parcel Return")]
        ParcelReturn = 4,
    }
}
