using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Defines which regulation set to use when shipping dry ice via UPS
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsDryIceRegulationSet
    {
        /// <summary>
        /// USDT regulation, or ground to Canada
        /// </summary>
        [Description("CFR"), ApiValue("CFR")]
        Cfr,

        /// <summary>
        /// Worldwide air movement
        /// </summary>
        [Description("IATA"), ApiValue("IATA")]
        Iata
    }
}
