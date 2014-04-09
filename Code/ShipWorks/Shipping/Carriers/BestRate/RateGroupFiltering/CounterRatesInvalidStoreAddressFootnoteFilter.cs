using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering
{
    /// <summary>
    /// An implementation of the IRateGroupFilter interface that will filter out the 
    /// any duplicate footnotes about an invalid store address being used when trying
    /// to gt counter rates that may be in a rate group when using best rate.
    /// </summary>
    public class CounterRatesInvalidStoreAddressFootnoteFilter : IRateGroupFilter
    {
        /// <summary>
        /// Method that filters a group of rates and returns a new group of the filtered rate results.
        /// </summary>
        /// <param name="rateGroup"></param>
        /// <returns>The filtered rate group.</returns>
        public RateGroup Filter(RateGroup rateGroup)
        {
            // Remove multiple footnotes for invalid store address when using counter rates
            List<IRateFootnoteFactory> footnoteFactories = rateGroup.FootnoteFactories.ToList();
            List<CounterRatesInvalidStoreAddressFootnoteFactory> invalidAddressFootnotes = footnoteFactories.OfType<CounterRatesInvalidStoreAddressFootnoteFactory>().ToList();

            if (invalidAddressFootnotes.Count() > 1)
            {
                // We have multiple invalid address footnotes; we only want to keep one of them
                footnoteFactories = footnoteFactories.Except(invalidAddressFootnotes).ToList();
                footnoteFactories.Add(invalidAddressFootnotes.First());
            }
            
            RateGroup filteredRateGroup = new RateGroup(rateGroup.Rates);
            footnoteFactories.ForEach(filteredRateGroup.AddFootnoteFactory);

            return filteredRateGroup;
        }
    }
}
