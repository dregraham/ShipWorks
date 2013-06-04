using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Possible values for UPS Export Reason field
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsExportReason
    {
        [Description("Sale")]
        Sale = 0,

        [Description("Return")]
        Return = 1,

        [Description("Gift")]
        Gift = 2,

        [Description("Sample")]
        Sample = 3,

        [Description("Repair")]
        Repair = 4,

        [Description("Inter-company Data")]
        InterCompanyData = 5

    }
}
