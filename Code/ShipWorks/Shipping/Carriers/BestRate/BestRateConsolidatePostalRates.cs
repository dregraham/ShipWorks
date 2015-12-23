using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Used when asking for a BestRateSHippingBrokerFactory
    /// Specifies which broker filters to use
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum BestRateConsolidatePostalRates
    {
        /// <summary>
        /// Returns default broker filters
        /// </summary>
        [Description("No")]
        No = 0,

        /// <summary>
        /// Returns broker filters to consolidate usps rates
        /// </summary>
        [Description("Yes")]
        Yes = 1
    }
}