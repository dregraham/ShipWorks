using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Consolidate Express1 rates with Usps rates
    /// </summary>
    public class UspsExpress1RateConsolidator
    {
        /// <summary>
        /// Perform the consolidation
        /// </summary>
        /// <param name="uspsRateGroup">Contains the existing Usps rates</param>
        /// <param name="express1RateTask">Task that will contain the Express1 rates, if any</param>
        public RateGroup Consolidate(RateGroup uspsRateGroup, Task<RateGroup> express1RateTask)
        {
            try
            {
                express1RateTask.Wait();
                return MergeRates(uspsRateGroup, express1RateTask.Result);
            }
            catch (Exception ex)
            {
                uspsRateGroup.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(new Express1StampsShipmentType(), ex.GetBaseException()));
            }

            return uspsRateGroup;
        }

        /// <summary>
        /// Perform the merge of Express1 rates into Usps rates
        /// </summary>
        private static RateGroup MergeRates(RateGroup uspsRateGroup, RateGroup express1Rates)
        {   
            List<RateResult> mergedRates = uspsRateGroup.Rates
                .Select(uspsRate => GetCheaperMatchingExpress1Rate(express1Rates.Rates, uspsRate) ?? uspsRate)
                .OrderBy(rate => ((PostalRateSelection)rate.Tag).ServiceType)
                .ThenBy(rate => rate.Selectable)
                .ThenBy(rate => ((PostalRateSelection)rate.Tag).ConfirmationType)
                .ToList();

            RateGroup mergedRateResult = new RateGroup(mergedRates);
            foreach (IRateFootnoteFactory footnoteFactory in uspsRateGroup.FootnoteFactories)
            {
                mergedRateResult.AddFootnoteFactory(footnoteFactory);
            }

            return mergedRateResult;
        }

        /// <summary>
        /// Get a cheaper Express1 rate that matches the usps rate service and confirmation types
        /// </summary>
        private static RateResult GetCheaperMatchingExpress1Rate(IEnumerable<RateResult> express1Rates, RateResult uspsRate)
        {
            return express1Rates.Select(x => new {Rate = x, Tag = x.Tag as PostalRateSelection})
                .Where(x => ServicesMatch(x.Tag, uspsRate.Tag as PostalRateSelection))
                .Select(x => x.Rate)
                .FirstOrDefault(x => x.Amount < uspsRate.Amount);
        }

        /// <summary>
        /// Gets whether the two services match
        /// </summary>
        private static bool ServicesMatch(PostalRateSelection express1Tag, PostalRateSelection uspsTag)
        {
            if (express1Tag == null || uspsTag == null)
            {
                return false;
            }

            return express1Tag.ServiceType == uspsTag.ServiceType &&
                   express1Tag.ConfirmationType == uspsTag.ConfirmationType;
        }
    }
}