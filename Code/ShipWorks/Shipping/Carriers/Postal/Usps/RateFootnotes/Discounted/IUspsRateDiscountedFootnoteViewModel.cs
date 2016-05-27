using System.Collections.Generic;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Discounted
{
    /// <summary>
    /// Usps discounted rate footnote view model
    /// </summary>
    public interface IUspsRateDiscountedFootnoteViewModel
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