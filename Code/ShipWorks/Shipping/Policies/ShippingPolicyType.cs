using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// An enumeration for the different type of shipping policies/extension points where
    /// behavior can be dynamically altered.
    /// </summary>
    public enum ShippingPolicyType
    {
        /// <summary>
        /// A policy type indicating that the quantity of rate results that get displayed
        /// for a shipment type should be altered.
        /// </summary>
        [ApiValue("RateResultCount")]
        RateResultCount,

        /// <summary>
        /// A policy type to indicate whether UPS should be used in the BestRate shipment type.
        /// </summary>
        [ApiValue("BestRateUpsRestriction")]
        BestRateUpsRestriction
    }
}
