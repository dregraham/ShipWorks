using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
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
                return MergeRateGroups(uspsRateGroup, express1RateTask.Result);
            }
            catch (Exception ex)
            {
                uspsRateGroup.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Express1Usps, ex.GetBaseException()));
            }

            return uspsRateGroup;
        }

        /// <summary>
        /// Perform the merge of Express1 rates into Usps rates
        /// </summary>
        private static RateGroup MergeRateGroups(RateGroup uspsRateGroup, RateGroup express1Rates)
        {   
            List<RateResult> mergedRates = uspsRateGroup.Rates
                .Select(uspsRate => GetCheaperMatchingExpress1Rate(express1Rates.Rates, uspsRate) ?? uspsRate)
                .OrderBy(rate => ((PostalRateSelection)rate.Tag).ServiceType)
                .ThenBy(rate => rate.Selectable)
                .ThenBy(rate => ((PostalRateSelection)rate.Tag).ConfirmationType)
                .ToList();

            MergeIcons(express1Rates, mergedRates);

            RateGroup mergedRateResult = new RateGroup(mergedRates);

            MergeFooters(uspsRateGroup, mergedRateResult);

            return mergedRateResult;
        }

        /// <summary>
        /// Merges the footers.
        /// </summary>
        private static void MergeFooters(RateGroup uspsRateGroup, RateGroup mergedRateResult)
        {
            foreach (IRateFootnoteFactory footnoteFactory in uspsRateGroup.FootnoteFactories)
            {
                mergedRateResult.AddFootnoteFactory(footnoteFactory);
            }
        }

        /// <summary>
        /// Find any "header" rates with no USPS icons and replace the header icon with the express1 header icon
        /// </summary>
        private static void MergeIcons(RateGroup express1Rates, List<RateResult> mergedRates)
        {
            //Get all header rates
            foreach (RateResult headerRate in mergedRates.Where(rate => !rate.Selectable))
            {
                PostalRateSelection headerRateTag = headerRate.Tag as PostalRateSelection;

                // Get the child rates of the header rate
                IEnumerable<RateResult> childRates = mergedRates.Where(rate => rate.Selectable && (rate.Tag as PostalRateSelection).ServiceType == headerRateTag.ServiceType);

                // If all the child rates are not USPS
                if (childRates.All(childRate => childRate.ShipmentType != ShipmentTypeCode.Usps))
                {
                    // Find the corresponding express1 header rate
                    RateResult expres1HeaderRateResult = express1Rates.Rates
                        .SingleOrDefault(express1Rate => !express1Rate.Selectable &&
                            (express1Rate.Tag as PostalRateSelection).ServiceType == headerRateTag.ServiceType);

                    //If found, use that logo
                    if (expres1HeaderRateResult != null)
                    {
                        headerRate.ProviderLogo = expres1HeaderRateResult.ProviderLogo;
                    }
                }
            }
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