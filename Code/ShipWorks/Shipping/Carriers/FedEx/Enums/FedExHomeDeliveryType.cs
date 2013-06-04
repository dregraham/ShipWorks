using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// FedEx home delivery extra options
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExHomeDeliveryType
    {
        [Description("None")]
        None = 0,

        [Description("Evening")]
        Evening = 1,

        [Description("Appointment")]
        Appointment = 2,

        [Description("Date Certain")]
        DateCertain = 3
    }
}
