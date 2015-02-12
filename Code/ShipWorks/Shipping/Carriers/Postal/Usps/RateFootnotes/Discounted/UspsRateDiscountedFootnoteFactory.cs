using System.Collections.Generic;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Discounted
{
    public class UspsRateDiscountedFootnoteFactory : IRateFootnoteFactory
    {
        private readonly List<RateResult> originalRates;
        private readonly List<RateResult> discountedRates;

        /// <summary>
        /// Construct a new UspsRateDiscountedFootnoteFactory object
        /// </summary>
        /// <param name="shipmentType">Type of shipment that instantiated this factory</param>
        /// <param name="originalRates">Original rates from the carrier</param>
        /// <param name="discountedRates">Express1 rates</param>
        public UspsRateDiscountedFootnoteFactory(ShipmentType shipmentType, List<RateResult> originalRates, List<RateResult> discountedRates)
        {
            this.originalRates = originalRates;
            this.discountedRates = discountedRates;

            ShipmentType = shipmentType;
        }

        /// <summary>
        /// Notes that this factory should not be used in BestRate
        /// </summary>
        public bool AllowedForBestRate
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// Create an Express1 rate discounted control
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new UspsRateDiscountedFootnote(originalRates, discountedRates);
        }
    }
}
