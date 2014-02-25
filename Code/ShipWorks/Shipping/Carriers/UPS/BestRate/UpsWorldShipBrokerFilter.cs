using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    /// <summary>
    /// An implementation of the IShippingBrokerFilter that will remove WorldShip brokers
    /// when there are multiple UPS brokers in order to prevent duplicates rate requests/results.
    /// </summary>
    public class UpsWorldShipBrokerFilter : IShippingBrokerFilter
    {
        /// <summary>
        /// Filters the incoming list of brokers
        /// </summary>
        /// <param name="brokers">List of brokers that will be filtered</param>
        /// <returns>Filtered list of brokers</returns>
        /// <remarks>The incoming list of brokers should not be modified</remarks>
        public IEnumerable<IBestRateShippingBroker> Filter(IEnumerable<IBestRateShippingBroker> brokers)
        {
            IEnumerable<IBestRateShippingBroker> bestRateShippingBrokers = brokers as IList<IBestRateShippingBroker> ?? brokers.ToList();

            // Since the WorldShip broker inherits from UpsBestRateBroker, we need to compare the counts of 
            // UPS brokers and see see if any of those are for WorldShip.
            IEnumerable<UpsBestRateBroker> upsBrokers = bestRateShippingBrokers.OfType<UpsBestRateBroker>();

            if (upsBrokers.Count() > 1 && upsBrokers.OfType<WorldShipBestRateBroker>().Any())
            {
                // Both UPS OnlineTools AND WorldShip are selected, remove WorldShip so we don't get double rates requested/returned
                bestRateShippingBrokers = bestRateShippingBrokers.Where(b => b.GetType() != typeof(WorldShipBestRateBroker));
            }

            return bestRateShippingBrokers;
        }
    }
}
