using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.FedEx
{
    /// <summary>
    /// Enum representing FedExFreightCollectTerms
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum FedExFreightCollectTermsType
    {
        /// <summary>
        /// None selected
        /// </summary>
        [Description("None")]
        [ApiValue("")]
        None = 1,

        /// <summary>
        /// Standard
        /// </summary>
        [Description("Standard")]
        [ApiValue("STANDARD")]
        Standard = 2,

        /// <summary>
        /// Non-recourse shipper signed
        /// </summary>
        [Description("Non-recourse shipper signed")]
        [ApiValue("NON_RECOURSE_SHIPPER_SIGNED")]
        NonRecourseShipperSigned = 2,
    }
}
