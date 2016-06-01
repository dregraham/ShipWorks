using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering
{
    /// <summary>
    /// Filters rates that don't meet the specified service level
    /// </summary>
    public class BestRateServiceLevelFilter : IRateGroupFilter
    {
        private readonly ServiceLevelType serviceLevelType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceLevelType"></param>
        public BestRateServiceLevelFilter(ServiceLevelType serviceLevelType)
        {
            this.serviceLevelType = serviceLevelType;
        }

        /// <summary>
        /// Filters the rates in the rate group that don't meet the specified service level
        /// </summary>
        public RateGroup Filter(RateGroup rateGroup)
        {
            IEnumerable<RateResult> rateResults = rateGroup.Rates;

            ServiceLevelSpeedComparer serviceLevelSpeedComparer = new ServiceLevelSpeedComparer();

            if (serviceLevelType != ServiceLevelType.Anytime)
            {
                DateTime? maxDeliveryDate = rateResults
                    .Where(x => x.ServiceLevel != ServiceLevelType.Anytime)
                    .Where(x => x.ServiceLevel <= serviceLevelType)
                    .Max(x => x.ExpectedDeliveryDate);

                rateResults = rateResults.Where(x => x.ExpectedDeliveryDate <= maxDeliveryDate || serviceLevelSpeedComparer.Compare(x.ServiceLevel, serviceLevelType) <= 0);
            }

            // We want the cheapest rates to appear first, and any ties to be ordered by service level
            IEnumerable<RateResult> orderedRates = rateResults.OrderBy(r => r.AmountOrDefault).ThenBy(r => r.ServiceLevel, serviceLevelSpeedComparer);

            return rateGroup.CopyWithRates(orderedRates);
        }
    }
}
