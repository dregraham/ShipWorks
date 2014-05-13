using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.BestRate
{
    /// <summary>
    /// Filters for only Endicia and Express1 Endicia rates.
    /// </summary>
    class UspsOnlyBrokerFilter : IShippingBrokerFilter
    {
        /// <summary>
        /// Filters the incoming list of brokers
        /// </summary>
        public IEnumerable<IBestRateShippingBroker> Filter(IEnumerable<IBestRateShippingBroker> brokers)
        {
            List<IBestRateShippingBroker> bestRateShippingBrokers = new List<IBestRateShippingBroker>();

            bestRateShippingBrokers.Add(brokers.OfType<EndiciaCounterRatesBroker>().First());
            bestRateShippingBrokers.Add(brokers.OfType<Express1EndiciaBestRateBroker>().First());

            return bestRateShippingBrokers;
        }
    }
}
