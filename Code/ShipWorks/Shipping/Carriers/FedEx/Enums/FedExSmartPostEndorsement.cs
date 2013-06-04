using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExSmartPostEndorsement
    {
        [Description("None")]
        None = 0,

        [Description("Address Correction")]
        AddressCorrection = 1,

        [Description("Leave if no response")]
        LeaveIfNoResponse = 2,

        [Description("Change Service")]
        ChangeService = 3,

        [Description("Forwarding Service")]
        ForwardingService = 4,

        [Description("Return Service")]
        ReturnService = 5
    }
}
