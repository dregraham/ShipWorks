using System.Linq;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Defines a rate result that has non-compete rules for displaying service type around other carriers
    /// </summary>
    public class NonCompetitiveRateResult : RateResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="originalRate">RateResult from which to create this result</param>
        public NonCompetitiveRateResult(RateResult originalRate) :
            base(originalRate.Description, originalRate.Days, originalRate.Amount, originalRate.Tag)
        {
            AmountFootnote = originalRate.AmountFootnote;
            ServiceLevel = originalRate.ServiceLevel;
        }

        /// <summary>
        /// Mask the description of the rate, if necessary
        /// </summary>
        /// <param name="rates">Collection of all rates, including this one</param>
        public override void MaskDescription(System.Collections.Generic.IEnumerable<RateResult> rates)
        {
            if (!rates.All(x => x is NonCompetitiveRateResult))
            {
                Description = "Undisclosed " + EnumHelper.GetDescription(ServiceLevel);
            }
        }
    }
}