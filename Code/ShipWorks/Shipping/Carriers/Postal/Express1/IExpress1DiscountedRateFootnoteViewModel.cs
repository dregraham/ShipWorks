using System.Collections.Generic;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Express1 discounted rate footnote view model
    /// </summary>
    public interface IExpress1DiscountedRateFootnoteViewModel
    {
        /// <summary>
        /// List of discounted rates
        /// </summary>
        List<RateResult> DiscountedRates { get; set; }

        /// <summary>
        /// List or original rates
        /// </summary>
        List<RateResult> OriginalRates { get; set; }
    }
}