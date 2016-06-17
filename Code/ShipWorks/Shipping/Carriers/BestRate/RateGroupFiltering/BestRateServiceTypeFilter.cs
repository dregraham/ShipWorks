using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering
{
    /// <summary>
    /// Filters rates by service type so that only one rate per service type is returned
    /// </summary>
    public class BestRateServiceTypeFilter : IRateGroupFilter
    {
        /// <summary>
        /// Method that filters rate results and returns a new list of the filtered rate results.
        /// </summary>
        public RateGroup Filter(RateGroup rateGroup)
        {
            IEnumerable<RateResult> orderedRates = rateGroup.Rates
                .GroupBy(RateResultKey)
                .Select(PreferredCheapestRate);

            return rateGroup.CopyWithRates(orderedRates);
        }

        /// <summary>
        /// Get the preferred cheapest rate from a group of rates
        /// </summary>
        private RateResult PreferredCheapestRate(IGrouping<string, RateResult> ratesForKey)
        {
            return ratesForKey.OrderBy(x => x.Amount)
                .ThenBy(PreferredCarrier)
                .FirstOrDefault();
        }

        /// <summary>
        /// Get the result key from a rate
        /// </summary>
        private string RateResultKey(RateResult rate)
        {
            return (rate.Tag as BestRateResultTag)?.ResultKey;
        }

        /// <summary>
        /// Helper method to be able to sort/filter shipment types.  If there is a tie in cost in a ResultKey group,
        /// we currently want Express1 types to win over Endicia, and Endicia over all others.
        /// </summary>
        private static int PreferredCarrier(RateResult rateResult)
        {
            switch (rateResult.ShipmentType)
            {
                case ShipmentTypeCode.Usps: return 0;
                case ShipmentTypeCode.Endicia: return 1;
                default: return 8;
            }
        }
    }
}
