using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.WebTools.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.BestRate
{
    public class PostalCounterBrokerFilter : IShippingBrokerFilter
    {
        public IEnumerable<IBestRateShippingBroker> Filter(IEnumerable<IBestRateShippingBroker> brokers)
        {
            List<IBestRateShippingBroker> filteredBrokers = brokers.ToList();
            WebToolsCounterRatesBroker webToolsBroker = filteredBrokers.OfType<EndiciaCounterRatesBroker>().FirstOrDefault();

            if (webToolsBroker == null)
            {
                webToolsBroker = filteredBrokers.OfType<StampsCounterRatesBroker>().FirstOrDefault();
            }

            return filteredBrokers.Where(x => !(x is WebToolsCounterRatesBroker) || x == webToolsBroker).ToList();
        }
    }
}
