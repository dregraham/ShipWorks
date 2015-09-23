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
        [Description("FedEx SmartPost Parcel Select")]
        ParcelSelect = 0,

        [Description("FedEx SmartPost® Media")]
        MediaMail = 1,

        [Description("FedEx SmartPost® Bound Printed Matter")]
        BoundPrintedMatter = 2,

        [Description("FedEx SmartPost Parcel Select Lightweight")]
        PresortedStandard = 3,

        [Description("FedEx SmartPost® Returns")]
        ParcelReturn = 4,
    }
}
