using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Consolidate a collection of usps rates
    /// </summary>
    public class UspsRateConsolidator
    {
        /// <summary>
        /// Perform the consolidation
        /// </summary>
        public List<RateResult> Consolidate(IEnumerable<List<RateResult>> rates)
        {
            List<RateResult> consolidatedRates = new List<RateResult>();

            foreach (RateResult rateResult in rates.SelectMany(r => r))
            {
                RateResult foundConsolidatedRate = consolidatedRates.SingleOrDefault(consolidatedRate => ServiceMatches(consolidatedRate, rateResult));

                if (foundConsolidatedRate == null)
                {
                    consolidatedRates.Add(rateResult);
                }
                else if (foundConsolidatedRate.Amount > rateResult.Amount)
                {
                    int indexOfFoundConsolidatedRate = consolidatedRates.IndexOf(foundConsolidatedRate);
                    consolidatedRates[indexOfFoundConsolidatedRate] = rateResult;
                }
                else if (foundConsolidatedRate.Amount == rateResult.Amount)
                {
                    AddAccounts(foundConsolidatedRate, rateResult);
                }
            }

            return consolidatedRates
                .OrderBy(GetServiceType)
                .ThenBy(r => r.Selectable)
                .ToList();
        }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        private PostalServiceType GetServiceType(RateResult rateResult)
        {
            UspsPostalRateSelection rateResultTag = (UspsPostalRateSelection)rateResult.Tag;

            return rateResultTag.ServiceType;
        }

        /// <summary>
        /// Adds the accounts specified in the sourceRate to the targetRate's accounts.
        /// </summary>
        private void AddAccounts(RateResult targetRate, RateResult sourceRate)
        {
            UspsPostalRateSelection targetRateTag = (UspsPostalRateSelection)targetRate.Tag;
            UspsPostalRateSelection sourceRateTag = (UspsPostalRateSelection)sourceRate.Tag;

            targetRateTag.Accounts.AddRange(sourceRateTag.Accounts);
        }

        /// <summary>
        /// If the ConfirmationType and ServiceType of both rates match, return true
        /// </summary>
        public bool ServiceMatches(RateResult rateA, RateResult rateB)
        {
            UspsPostalRateSelection rateATag = (UspsPostalRateSelection)rateA.Tag;
            UspsPostalRateSelection rateBTag = (UspsPostalRateSelection)rateB.Tag;

            return rateATag.ServiceType == rateBTag.ServiceType &&
                rateATag.ConfirmationType == rateBTag.ConfirmationType;
        }
    }
}