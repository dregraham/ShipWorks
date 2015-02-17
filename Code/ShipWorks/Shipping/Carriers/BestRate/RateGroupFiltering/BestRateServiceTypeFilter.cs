using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Editing;
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
            // Now group by the ResultKey, so that we can then get the cheapest rate per group
            IEnumerable<RateResult> orderedRates = rateGroup.Rates.GroupBy(reateResult => ((BestRateResultTag)reateResult.Tag).ResultKey,
                                                (rateResultSource, rateResultSelector) => rateResultSelector.Aggregate(RateResultsGroupBySelector));

            return rateGroup.CopyWithRates(orderedRates);
        }

        /// <summary>
        /// Helper method to be able to sort/filter shipment types.  If there is a tie in cost in a ResultKey group,
        /// we currently want Express1 types to win over Endicia, and Endicia over all others.
        /// </summary>
        private static int CarrierSortValue(RateResult rateResult)
        {
            switch (rateResult.ShipmentType)
            {
                case ShipmentTypeCode.Express1Endicia:
                    return 0;
                case ShipmentTypeCode.Express1Usps:
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
