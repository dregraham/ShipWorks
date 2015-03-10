using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.WebTools.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.BestRate
{
    /// <summary>
    /// Filter postal counter rate brokers so that only one is used
    /// </summary>
    public class PostalCounterBrokerFilter : IShippingBrokerFilter
    {
        /// <summary>
        /// Perform the filter
        /// </summary>
        public IEnumerable<IBestRateShippingBroker> Filter(IEnumerable<IBestRateShippingBroker> brokers)
        {
            List<IBestRateShippingBroker> filteredBrokers = brokers.ToList();

            // Get the first Usps broker
            WebToolsCounterRatesBroker webToolsBroker = filteredBrokers.OfType<UspsCounterRatesBroker>().FirstOrDefault();

            return filteredBrokers.Where(x => !(x is WebToolsCounterRatesBroker) || x == webToolsBroker).ToList();
        }
    }
}
