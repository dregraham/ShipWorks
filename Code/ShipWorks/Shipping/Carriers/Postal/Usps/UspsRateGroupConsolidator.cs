using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Consolidate a collection of usps rates
    /// </summary>
    public class UspsRateGroupConsolidator
    {
        /// <summary>
        /// Perform the consolidation
        /// </summary>
        public RateGroup Consolidate(List<RateGroup> rateGroupsToConsolidate)
        {
            List<RateResult> consolidatedRates = new List<RateResult>();

            // Loop through all the rates in the rate group
            foreach (RateResult rateResult in rateGroupsToConsolidate.SelectMany(r => r.Rates))
            {
                // Find rate in new consolidated rate list.
                RateResult foundConsolidatedRate = consolidatedRates.SingleOrDefault(consolidatedRate => ServiceMatches(consolidatedRate, rateResult));

                // if not found, add it.
                if (foundConsolidatedRate == null)
                {
                    consolidatedRates.Add(rateResult);
                }
                // if better rate found, us it instead
                else if (foundConsolidatedRate.Amount > rateResult.Amount)
                {
                    int indexOfFoundConsolidatedRate = consolidatedRates.IndexOf(foundConsolidatedRate);
                    consolidatedRates[indexOfFoundConsolidatedRate] = rateResult;
                }
                // if it is the same rate, add this account to the result
                else if (foundConsolidatedRate.Amount == rateResult.Amount)
                {
                    AddAccounts(foundConsolidatedRate, rateResult);
                }
            }

            List<RateResult> sortedConsolidatedRates = consolidatedRates
                .OrderBy(GetServiceType)
                .ThenBy(r => r.Selectable)
                .ThenBy(rate => ((PostalRateSelection)rate.Tag).ConfirmationType)
                .ToList();

            RateGroup consolidatedRateGroup = new RateGroup(sortedConsolidatedRates);

            rateGroupsToConsolidate.SelectMany(r=>r.FootnoteFactories)
                .GroupBy(footnoteFactory=>footnoteFactory.GetType())
                // exclude UspsRatePromotionFootnoteFactory if not all rate groups have it
                .Where(group=> !(group.First() is UspsRatePromotionFootnoteFactory) || group.Count() == rateGroupsToConsolidate.Count )
                .Select(group=>group.First())
                .ToList()
                .ForEach(consolidatedRateGroup.AddFootnoteFactory);

            return consolidatedRateGroup;
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