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
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipEngineNonDeliveryType
    {
        [Description("Treat As Abandoned")]
        [ApiValue("treat_as_abandoned")]
        TreatAsAbandoned = 0,

        [Description("Return To Sender")]
        [ApiValue("return_to_sender")]
        ReturnToSender = 1
    }
}
