using Interapptive.Shared.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Enumeration of instructions one could send with customs to ShipEngine
    /// </summary>
    /// <remarks>
    /// The numeric values of the enum values correspond to the numeric values of the enum values 
    /// of InternationalOptions.NonDeliveryEnum
    /// </remarks>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipEngineNonDeliveryType
    {
        [Description("Return To Sender")]
        [ApiValue("return_to_sender")]
        ReturnToSender = 0,

        [Description("Treat As Abandoned")]
        [ApiValue("treat_as_abandoned")]
        TreatAsAbandoned = 1,
    }
}
