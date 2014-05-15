using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.BestRate
{
    /// <summary>
    /// Filters for only Endicia and Express1 Endicia rates.
    /// </summary>
    internal class PostalOnlyBrokerFilter : IShippingBrokerFilter
    {
        /// <summary>
        /// Filters the incoming list of brokers
        /// </summary>
        public IEnumerable<IBestRateShippingBroker> Filter(IEnumerable<IBestRateShippingBroker> brokers)
        {
            List<IBestRateShippingBroker> bestRateShippingBrokers = new List<IBestRateShippingBroker>();

            EndiciaCounterRatesBroker endiciaCounterRatesBroker = brokers.OfType<EndiciaCounterRatesBroker>().FirstOrDefault();
            if (endiciaCounterRatesBroker != null)
            {
                bestRateShippingBrokers.Add(endiciaCounterRatesBroker);
            }

            Express1EndiciaBestRateBroker express1EndiciaBestRateBroker = brokers.OfType<Express1EndiciaBestRateBroker>().FirstOrDefault();
            if (express1EndiciaBestRateBroker != null)
            {
                bestRateShippingBrokers.Add(express1EndiciaBestRateBroker);
            }

            return bestRateShippingBrokers;
        }
    }
}