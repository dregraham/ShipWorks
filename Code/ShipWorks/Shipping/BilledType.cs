using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Text;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Various ways a shipment is billed.  New shipments start as Unknown.  After processing, if we can determine
    /// from the carrier if it was rated based on actual weight or dimensional weight, we'll reset it appropriately.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum BilledType
    {
        /// <summary>
        /// Initial BilledType of a shipment.  Only changed to another value if we can determine it.
        /// </summary>
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// Shipment was rated based on actual weight.
        /// </summary>
        [Description("Actual weight")]
        ActualWeight = 1,

        /// <summary>
        /// Shipment was rated based on dimensional weight.
        /// </summary>
        [Description("Dimensional weight")]
        DimensionalWeight = 2
    }
}
