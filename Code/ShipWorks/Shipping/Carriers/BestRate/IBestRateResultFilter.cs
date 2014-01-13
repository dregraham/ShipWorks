using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Interface to allow carriers to implement a filter to apply to a list of RateResults
    /// </summary>
    public interface IBestRateResultFilter
    {
        /// <summary>
        /// Method that filters rate results and returns a new list of the filtered rate results.
        /// </summary>
        IEnumerable<RateResult> FilterRates(IEnumerable<RateResult> rateResults, ServiceLevelType serviceLevelType);
    }
}
