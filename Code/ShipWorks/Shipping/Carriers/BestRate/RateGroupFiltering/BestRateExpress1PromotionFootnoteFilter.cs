using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering
{
    /// <summary>
    /// An implementation of the IRateGroupFilter interface that will filter out the 
    /// any duplicate Express1 promotion footnotes that may be in a rate group from
    /// when using best rate.
    /// </summary>
    public class BestRateExpress1PromotionFootnoteFilter : IRateGroupFilter
    {
        /// <summary>
        /// Remove any duplicate Express1 promotion footnotes that may be in a rate group from
        /// when using best rate with a preference for Endicia.
        /// </summary>
        public RateGroup Filter(RateGroup rateGroup)
        {
            List<IRateFootnoteFactory> footnoteFactories = rateGroup.FootnoteFactories.ToList();
            
            // Remove the express1 promotion footnote
            if (footnoteFactories.Count(f => f.GetType() == typeof(Express1PromotionRateFootnoteFactory)) > 1)
            {
                // We have two express1 promotional footnote controls; we only want to
                // keep the one that is for Endicia
                footnoteFactories.RemoveAll(f => f.ShipmentTypeCode != ShipmentTypeCode.Endicia && f.GetType() == typeof(Express1PromotionRateFootnoteFactory));
            }

            RateGroup filteredRateGroup = new RateGroup(rateGroup.Rates);
            footnoteFactories.ForEach(filteredRateGroup.AddFootnoteFactory);

            return filteredRateGroup;
        }
    }
}
