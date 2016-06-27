using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Rating
{
    /// <summary>
    /// Factory that creates IRateSelections 
    /// </summary>
    public interface IRateSelectionFactory
    {
        /// <summary>
        /// Creates an IRateSelection based on given RateResult
        /// </summary>
        IRateSelection CreateRateSelection(RateResult rateResult);
    }
}
