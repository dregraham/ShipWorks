using System.Reflection;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Status of the shipment
    /// </summary>
    [Obfuscation]
    public enum ShipmentStatus
    {
        /// <summary>
        /// Shipment is not processed
        /// </summary>
        Unprocessed,

        /// <summary>
        /// Shipment has been processed, but not voided
        /// </summary>
        Processed,

        /// <summary>
        /// Shipment has been voided
        /// </summary>
        Voided,

        /// <summary>
        /// No status
        /// </summary>
        None
    }
}
