using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Consolidate a collection of usps rates
    /// </summary>
    public class UspsRateConsolidator
    {
        /// <summary>
        /// Perform the consolidation
        /// </summary>
        public List<RateResult> Consolidate(IEnumerable<List<RateResult>> rates)
        {
            //TODO: Implement correct merging logic; this just flattens the list
            return rates.SelectMany(x => x).ToList();
        }
    }
}