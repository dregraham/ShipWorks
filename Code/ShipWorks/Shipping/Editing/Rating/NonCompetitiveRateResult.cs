using System.Linq;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Defines a rate result that has non-compete rules for displaying service type around other carriers
    /// </summary>
    public class NoncompetitiveRateResult : RateResult
    {
        private readonly string maskedServiceTypeDescription;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoncompetitiveRateResult"/> class.
        /// </summary>
        /// <param name="originalRate">The original rate.</param>
        /// <param name="maskedServiceTypeDescription">The masked service type description.</param>
        public NoncompetitiveRateResult(RateResult originalRate, string maskedServiceTypeDescription) 
            : base(originalRate.Description, originalRate.Days, originalRate.Amount, originalRate.Tag)
        {
            this.maskedServiceTypeDescription = maskedServiceTypeDescription;
            AmountFootnote = originalRate.AmountFootnote;
            ServiceLevel = originalRate.ServiceLevel;
            OriginalRate = originalRate;
            ExpectedDeliveryDate = originalRate.ExpectedDeliveryDate;
            CarrierDescription = originalRate.CarrierDescription;
            ShipmentType = originalRate.ShipmentType;
            IsCounterRate = originalRate.IsCounterRate;
        }

        /// <summary>
        /// Gets the RateResult from which this object was created
        /// </summary>
        /// <remarks>This is to make testing simpler, since we're replacing results to extend them</remarks>
        public RateResult OriginalRate { get; private set; }

        /// <summary>
        /// Mask the description of the rate, if necessary
        /// </summary>
        /// <param name="rates">Collection of all rates, including this one</param>
        public override void MaskDescription(System.Collections.Generic.IEnumerable<RateResult> rates)
        {
            if (!rates.All(x => x is NoncompetitiveRateResult))
            {
                Description = "Undisclosed " + maskedServiceTypeDescription;
                CarrierDescription = "Undisclosed";
            }
        }
    }
}