using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Possible values for USPS Endorsements, for SurePost and MI
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UspsEndorsementType
    {
        [Description("None")]
        [ApiValue("")]
        None = 0,

        [Description("Carrier - Leave If No Response")]
        [ApiValue("")]
        CarrierLeaveIfNoResponse = 1,

        [Description("Return Service Requested")]
        [ApiValue("1")]
        ReturnServiceRequested = 2,

        [Description("Forwarding Service Requested")]
        [ApiValue("2")]
        ForwardingServiceRequested = 3,

        [Description("Address Service Requested")]
        [ApiValue("3")]
        AddressServiceRequested = 4,

        [Description("Change Service Requested")]
        [ApiValue("4")]
        ChangeServiceRequested = 5
    }
}
