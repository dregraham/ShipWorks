using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Interface to allow carriers to implement a filter to apply to a list of RateResults
    /// </summary>
    public interface IRateGroupFilter
    {
        /// <summary>
        /// Method that filters rate results and returns a new list of the filtered rate results.
        /// </summary>
        RateGroup FilterRates(RateGroup rateGroup, ServiceLevelType serviceLevelType);
    }
}
