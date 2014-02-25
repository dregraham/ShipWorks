using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Defines a filter for a collection of brokers
    /// </summary>
    public interface IShippingBrokerFilter
    {
        /// <summary>
        /// Filters the incoming list of brokers
        /// </summary>
        /// <param name="brokers">List of brokers that will be filtered</param>
        /// <returns>Filtered list of brokers</returns>
        /// <remarks>The incoming list of brokers should not be modified</remarks>
        IEnumerable<IBestRateShippingBroker> Filter(IEnumerable<IBestRateShippingBroker> brokers);
    }
}
