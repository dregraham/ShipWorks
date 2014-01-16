using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Defines a rate result that has non-compete rules for displaying service type around other carriers
    /// </summary>
    public class NoncompetitiveRateResult : RateResult
    {
        private readonly string maskedServiceTypeDescription;

        /// <summary>
        /// Constructor
        /// </summary>
        public NoncompetitiveRateResult(RateResult originalRate, string maskedServiceTypeDescription) :
            base(originalRate.Description, originalRate.Days, originalRate.Amount, originalRate.Tag)
        {
            this.maskedServiceTypeDescription = maskedServiceTypeDescription;
            AmountFootnote = originalRate.AmountFootnote;
            ServiceLevel = originalRate.ServiceLevel;
            OriginalRate = originalRate;
            ExpectedDeliveryDate = originalRate.ExpectedDeliveryDate;
            CarrierDescription = originalRate.CarrierDescription;
            ShipmentType = originalRate.ShipmentType;
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
                ShipmentType = ShipmentTypeCode.Other;
            }
        }
    }
}