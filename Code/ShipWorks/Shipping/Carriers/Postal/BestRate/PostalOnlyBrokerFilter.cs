using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.BestRate
{
    /// <summary>
    /// Filters for only Usps rates.
    /// </summary>
    internal class PostalOnlyBrokerFilter : IShippingBrokerFilter
    {
        /// <summary>
        /// Filters the incoming list of brokers
        /// </summary>
        public IEnumerable<IBestRateShippingBroker> Filter(IEnumerable<IBestRateShippingBroker> brokers)
        {
            List<IBestRateShippingBroker> bestRateShippingBrokers = new List<IBestRateShippingBroker>();

            UspsCounterRatesBroker uspsCounterRatesBroker = brokers.OfType<UspsCounterRatesBroker>().FirstOrDefault();
            if (uspsCounterRatesBroker != null)
            {
                bestRateShippingBrokers.Add(uspsCounterRatesBroker);
            }

            return bestRateShippingBrokers;
        }
    }
}