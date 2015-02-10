using System.Collections.Generic;
using System.Threading.Tasks;
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
        public RateGroup Consolidate(RateGroup uspsRateGroup, Task<List<RateResult>> express1RateTask)
        {
            express1RateTask.Wait();
            if (express1RateTask.Exception == null)
            {
                //TODO: Merge the results of the task into the usps rates
            }
            else
            {
                //TODO: Add exception details to footer
            }

            return uspsRateGroup;
        }
    }
}