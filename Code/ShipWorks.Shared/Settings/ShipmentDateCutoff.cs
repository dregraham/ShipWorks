using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Settings
{
    /// <summary>
    /// Shipment date cutoff DTO
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class ShipmentDateCutoff
    {
        public static readonly ShipmentDateCutoff Default = new ShipmentDateCutoff(false, TimeSpan.FromHours(17));

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

        /// <summary>
        /// Compare two ShipmentDateCutoff for equality
        /// </summary>
        public override bool Equals(object compareTo)
        {
            ShipmentDateCutoff sdcToCompare = compareTo as ShipmentDateCutoff;

            if (sdcToCompare == null)
            {
                return false;
            }

            return Enabled == sdcToCompare.Enabled && CutoffTime == sdcToCompare.CutoffTime;
        }

        /// <summary>
        /// Get a hash code for this instance
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Enabled.GetHashCode() ^ CutoffTime.GetHashCode();
        }

        /// <summary>
        /// Compare two ShipmentDateCutoff for equality
        /// </summary>
        public static bool operator == (ShipmentDateCutoff obj1, ShipmentDateCutoff obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }

            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return obj1.Equals(obj2);
        }

        /// <summary>
        /// Compare two ShipmentDateCutoff for inequality
        /// </summary>
        public static bool operator != (ShipmentDateCutoff obj1, ShipmentDateCutoff obj2)
        {
            return !(obj1 == obj2);
        }
    }
}
