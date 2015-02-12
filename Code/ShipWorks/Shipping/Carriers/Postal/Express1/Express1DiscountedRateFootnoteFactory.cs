using System.Collections.Generic;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Creates Express1 Rate Discounted footnote controls
    /// </summary>
    public class Express1DiscountedRateFootnoteFactory : IRateFootnoteFactory
    {
        private readonly List<RateResult> originalRates;
        private readonly List<RateResult> discountedRates;

        /// <summary>
        /// Construct a new Express1DiscountedRateFootnoteFactory object
        /// </summary>
        /// <param name="shipmentType">Type of shipment that instantiated this factory</param>
        /// <param name="originalRates">Original rates from the carrier</param>
        /// <param name="discountedRates">Express1 rates</param>
        public Express1DiscountedRateFootnoteFactory(ShipmentType shipmentType, List<RateResult> originalRates, List<RateResult> discountedRates)
        {
            this.originalRates = originalRates;
            this.discountedRates = discountedRates;
            ShipmentType = shipmentType;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// Notes that this factory should not be used in BestRate
        /// </summary>
        public bool AllowedForBestRate
        {
            get { return false; }
        }

        /// <summary>
        /// Create an Express1 rate discounted control
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new Express1RateDiscountedFootnote(originalRates, discountedRates);
        }
    }
}
