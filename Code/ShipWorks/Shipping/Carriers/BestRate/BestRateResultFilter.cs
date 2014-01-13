using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Implementation of IBestRateResultFilter.  Used to filter Rate Results.
    /// </summary>
    public class BestRateResultFilter : IBestRateResultFilter
    {
        /// <summary>
        /// Method that filters rate results and returns a new list of the filtered rate results.
        /// </summary>
        public IEnumerable<RateResult> FilterRates(List<RateResult> rateResults)
        {
            throw new NotImplementedException();
        }
    }
}
