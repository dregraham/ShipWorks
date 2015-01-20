using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// An enumeration for the different type of shipping policies/extension points where
    /// behavior can be dynamically altered.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShippingPolicyType
    {
        /// <summary>
        /// A policy type indicating that the quantity of rate results that get displayed
        /// for a shipment type should be altered.
        /// </summary>
        [Description("Policy that limits how many results are shown in the rate grid")]
        [ApiValue("RateResultCount")]
        RateResultCount,

        /// <summary>
        /// A policy type to indicate whether UPS should be used in the BestRate shipment type.
        /// </summary>
        [Description("Defines whether UPS should be excluded from best rates")]
        [ApiValue("BestRateUpsRestriction")]
        BestRateUpsRestriction
    }
}
