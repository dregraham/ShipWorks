using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            IEnumerable<RateResult> orderedRates = rateResults.OrderBy(r => r.Amount).ThenBy(r => r.ServiceLevel, serviceLevelSpeedComparer);

            // Now group by the ResultKey, so that we can then get the cheapest rate per group
            orderedRates = orderedRates.GroupBy(reateResult => ((BestRateResultTag)reateResult.Tag).ResultKey,
                                                (rateResultSource, rateResultSelector) => rateResultSelector.Aggregate(RateResultsGroupBySelector));

            return orderedRates;
        }

        /// <summary>
        /// Helper method to be able to sort/filter shipment types.  If there is a tie in cost in a ResultKey group,
        /// we currently want Express1 types to win over Endicia, and Endicia over all others.
        /// </summary>
        private static int CarrierSortValue(RateResult rateResult)
        {
            switch ((ShipmentTypeCode)rateResult.ShipmentType)
            {
                case ShipmentTypeCode.Express1Endicia:
                    return 0;
                case ShipmentTypeCode.Express1Stamps:
                    return 1;
                case ShipmentTypeCode.Endicia:
                    return 2;
                default:
                    return 50;
            }
        }

        /// <summary>
        /// Method used by the GroupBy Aggregate function to find the service types to return.
        /// Lowest cost will be returned.  If the cost of two service types are the same, Endicia will be returned.
        /// </summary>
        private RateResult RateResultsGroupBySelector(RateResult currentRateResult, RateResult nextRateResult)
        {
            if (currentRateResult.Amount < nextRateResult.Amount)
            {
                return currentRateResult;
            }
            
            if (currentRateResult.Amount > nextRateResult.Amount)
            {
                return nextRateResult;
            }

            int currentRateResultCarrierSort = CarrierSortValue(currentRateResult);
            int nextRateResultCarrierSort = CarrierSortValue(nextRateResult);

            if (currentRateResultCarrierSort < nextRateResultCarrierSort)
            {
                return currentRateResult;
            }

            if (nextRateResultCarrierSort < currentRateResultCarrierSort)
            {
                return nextRateResult;
            }

            // The carrier rate sorts were equal, chose the current rate as it should be the faster service type.
            return currentRateResult;
        }
    }
}
