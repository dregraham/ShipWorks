using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// An enumeration for denoting how ShipSense may or may not have been applied to a shipment.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipSenseStatus
    {
        /// <summary>
        /// The value denoting that ShipSense was not applied to a shipment.
        /// </summary>
        [Description("Not applied")]
        [ImageResourceAttribute("delete16")]
        NotApplied = 0,

        /// <summary>
        /// The value denoting that ShipSense was applied to a shipment.
        /// </summary>
        [Description("Applied")]
        [ImageResourceAttribute("sw_cubes_16")]
        Applied = 1,

        /// <summary>
        /// The value denoting that ShipSense was applied to a shipment, but the ShipSense data was overwritten.
        /// </summary>
        [Description("Applied and overwritten")]
        [ImageResourceAttribute("pencil2_small")]
        Overwritten = 2
    }
}
