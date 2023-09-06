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
    public enum FedExSmartPostEndorsement
    {
        [Description("None")]
        [ApiValue("none")]
        None = 0,

        [Description("Address Correction")]
        [ApiValue("address_service_requested")]
        AddressCorrection = 1,

        [Description("Leave if no response")]
        [ApiValue("leave_if_no_response")]
        LeaveIfNoResponse = 2,

        [Description("Change Service")]
        [ApiValue("change_service_requested")]
        ChangeService = 3,

        [Description("Forwarding Service")]
        [ApiValue("forwarding_service_requested")]
        ForwardingService = 4,

        [Description("Return Service")]
        [ApiValue("return_service_requested")]
        ReturnService = 5
    }
}
