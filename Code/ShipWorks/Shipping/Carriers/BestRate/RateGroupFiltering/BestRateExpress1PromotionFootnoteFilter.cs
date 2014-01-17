using ShipWorks.Shipping.Editing;

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
        /// Method that filters a group of rates and returns a new group of the filtered rate results.
        /// </summary>
        /// <param name="rateGroup"></param>
        /// <returns></returns>
        public RateGroup Filter(RateGroup rateGroup)
        {
            // TODO: Filter the promo footnotes
            return rateGroup;
        }
    }
}
