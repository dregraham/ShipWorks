using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.BestRate
{
    /// <summary>
    /// An implementation of the IShippingBrokerFilter that will remove duplicate
    /// Express1 counter rate brokers with a preference for Stamps.com. Express1 counter
    /// rate brokers are also removed if there is at least one "real" Express1 broker.
    /// </summary>
    public class Express1BrokerFilter : IShippingBrokerFilter
    {
        /// <summary>
        /// Filters the incoming list of brokers
        /// </summary>
        /// <param name="brokers">List of brokers that will be filtered</param>
        /// <returns>Filtered list of brokers</returns>
        /// <remarks>The incoming list of brokers should not be modified</remarks>
        public IEnumerable<IBestRateShippingBroker> Filter(IEnumerable<IBestRateShippingBroker> brokers)
        {
            List<IBestRateShippingBroker> filteredBrokers = new List<IBestRateShippingBroker>(brokers);
            
            filteredBrokers = RemoveCounterRateBrokersWhenRealBrokersExist(filteredBrokers);
            filteredBrokers = RemoveDuplicateCounterRateBrokers(filteredBrokers);
            
            return filteredBrokers;
        }

        /// <summary>
        /// Removes the counter rate brokers when real brokers exist.
        /// </summary>
        /// <param name="brokers">The brokers to be filtered.</param>
        /// <returns>The filtered list of brokers.</returns>
        private List<IBestRateShippingBroker> RemoveCounterRateBrokersWhenRealBrokersExist(IEnumerable<IBestRateShippingBroker> brokers)
        {
            List<IBestRateShippingBroker> filteredBrokers = new List<IBestRateShippingBroker>(brokers);

            // Remove any Express1 counter brokers if a real Express1 broker exists
            if (filteredBrokers.OfType<Express1StampsBestRateBroker>().Any(b => !b.IsCounterRate) || filteredBrokers.OfType<Express1EndiciaBestRateBroker>().Any(b => !b.IsCounterRate))
            {
                // We have at least one real Express1 broker, so remove any Express1 counter rates brokers
                filteredBrokers = filteredBrokers.Where(b => b.GetType() != typeof(Express1StampsCounterRatesBroker)).ToList();
                filteredBrokers = filteredBrokers.Where(b => b.GetType() != typeof(Express1EndiciaCounterRatesBroker)).ToList();
            }

            return filteredBrokers;
        }

        /// <summary>
        /// Removes any duplicate counter rate brokers with a preference for keeping Stamps.
        /// </summary>
        /// <param name="brokers">The brokers to be filtered.</param>
        /// <returns>The filtered list of brokers.</returns>
        private List<IBestRateShippingBroker> RemoveDuplicateCounterRateBrokers(IEnumerable<IBestRateShippingBroker> brokers)
        {
            List<IBestRateShippingBroker> filteredBrokers = new List<IBestRateShippingBroker>(brokers);
            if (filteredBrokers.OfType<Express1StampsCounterRatesBroker>().Any() && filteredBrokers.OfType<Express1StampsCounterRatesBroker>().Any())
            {
                // Remove the Endicia counter broker if a Stamps.com counter broker exists
                filteredBrokers = filteredBrokers.Where(b => b.GetType() != typeof(Express1EndiciaCounterRatesBroker)).ToList();
            }

            return filteredBrokers;
        }
    }
}
