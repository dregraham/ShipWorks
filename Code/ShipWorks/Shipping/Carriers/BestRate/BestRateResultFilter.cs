using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Implementation of IBestRateResultFilter.  Used to filter Rate Results.
    /// </summary>
    public class BestRateResultFilter : IBestRateResultFilter
    {
        /// <summary>
        /// Method that filters rate results and returns a new list of the filtered rate results.
        /// </summary>
        public IEnumerable<RateResult> FilterRates(IEnumerable<RateResult> rateResults, ServiceLevelType serviceLevelType)
        {
            var serviceLevelSpeedComparer = new ServiceLevelSpeedComparer();

            if (serviceLevelType != ServiceLevelType.Anytime)
            {
                DateTime? maxDeliveryDate = rateResults
                    .Where(x => x.ServiceLevel != ServiceLevelType.Anytime)
                    .Where(x => x.ServiceLevel <= serviceLevelType)
                    .Max(x => x.ExpectedDeliveryDate);

                rateResults = rateResults.Where(x => x.ExpectedDeliveryDate <= maxDeliveryDate || serviceLevelSpeedComparer.Compare(x.ServiceLevel, serviceLevelType) <= 0);
            }

            // We want the cheapest rates to appear first, and any ties to be ordered by service level
            // and return the top 5
            IEnumerable<RateResult> orderedRates = rateResults.OrderBy(r => r.Amount).ThenBy(r => r.ServiceLevel, serviceLevelSpeedComparer);
            List<RateResult> orderedRatesList = orderedRates.Take(5).ToList();

            return orderedRatesList;
        }
    }
}
