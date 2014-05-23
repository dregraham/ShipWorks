using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// An enumeration for the different states of an order in relation to the ShipSense knowledge base.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipSenseOrderRecognitionStatus
    {
        /// <summary>
        /// A status for the case where an order's hash key has been calculated, but
        /// ShipSense does not have a knowledge base entry for the hash.
        /// </summary>
        [Description("Won't be Applied")]
        NotRecognized = 0,

        /// <summary>
        /// An order's contents are recognized by ShipSense and contained in the knowledge base.
        /// </summary>
        [Description("Will be Applied")]
        [ImageResourceAttribute("check2")]
        Recognized = 1,
        
        /// <summary>
        /// The order is not applicable to the knowledge base. This will be the case for
        /// pre-ShipSense orders that were not backfilled by the ShipSense loader.
        /// </summary>
        [Description("Not analyzed")]
        NotApplicable = 2
    }
}
