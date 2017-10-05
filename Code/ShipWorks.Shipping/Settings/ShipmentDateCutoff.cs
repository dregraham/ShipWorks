using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Shipment date cutoff DTO
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class ShipmentDateCutoff
    {
        /// <summary>
        /// Constructor
        /// </summary>
        [JsonConstructor]
        public ShipmentDateCutoff(bool enabled, TimeSpan cutoffTime)
        {
            Enabled = enabled;
            CutoffTime = cutoffTime;
        }

        /// <summary>
        /// Is this cutoff enabled
        /// </summary>
        public bool Enabled { get; }

        /// <summary>
        /// Timespan for this cutoff
        /// </summary>
        public TimeSpan CutoffTime { get; }
    }
}
