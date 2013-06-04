using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Subjects that can be used for quantum view notify emails
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsEmailNotificationSubject
    {
        [Description("Tracking Number")]
        TrackingNumber = 0,

        [Description("Reference Number")]
        ReferenceNumber = 1
    }
}
