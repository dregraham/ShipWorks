using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.FedEx
{
    /// <summary>
    /// Enum for FedEx freight roles
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum FedExFreightShipmentRoleType
    {
        /// <summary>
        /// None selected
        /// </summary>
        [Description("None")]
        [ApiValue("")]
        None = 0,

        /// <summary>
        /// Consignee
        /// </summary>
        [Description("Consignee")]
        [ApiValue("CONSIGNEE")]
        Consignee = 1,

        /// <summary>
        /// Shipper
        /// </summary>
        [Description("Shipper")]
        [ApiValue("SHIPPER")]
        Shipper = 2,
    }
}
