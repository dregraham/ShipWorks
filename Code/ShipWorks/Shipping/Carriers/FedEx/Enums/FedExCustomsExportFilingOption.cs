using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// An enumeration to coincide with the FedEx B13A filing option for exports.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExCustomsExportFilingOption
    {
        [Description("Not Required")]
        NotRequired = 0,

        [Description("Manually Attached")]
        ManuallyAttached = 1,

        [Description("Filed Electronically")]
        FiledElectonically = 2,

        [Description("Summary Reporting")]
        SummaryReporting = 3
    }
}
