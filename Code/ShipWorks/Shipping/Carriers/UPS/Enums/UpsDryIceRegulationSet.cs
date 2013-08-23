using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Defines which regulation set to use when shipping dry ice via UPS
    /// </summary>
    public enum UpsDryIceRegulationSet
    {
        /// <summary>
        /// USDT regulation, or ground to Canada
        /// </summary>
        [Description("CFR")]
        Cfr,

        /// <summary>
        /// Worldwide air movement
        /// </summary>
        [Description("IATA")]
        Iata
    }
}
