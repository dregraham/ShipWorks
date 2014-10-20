using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.BestRate
{
    /// <summary>
    /// Filters for only Stamps.com rates.
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

            StampsCounterRatesBroker stampsCounterRatesBroker = brokers.OfType<StampsCounterRatesBroker>().FirstOrDefault();
            if (stampsCounterRatesBroker != null)
            {
                bestRateShippingBrokers.Add(stampsCounterRatesBroker);
            }

            return bestRateShippingBrokers;
        }
    }
}