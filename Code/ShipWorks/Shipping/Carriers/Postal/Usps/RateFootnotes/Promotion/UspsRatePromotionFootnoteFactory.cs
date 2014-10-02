using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion
{
    public class UspsRatePromotionFootnoteFactory : IRateFootnoteFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRatePromotionFootnoteFactory"/> class.
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        public UspsRatePromotionFootnoteFactory(ShipmentType shipmentType)
        {
            ShipmentType = shipmentType;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        /// <param name="parameters">Parameters that allow footnotes to interact with the rates grid</param>
        /// <returns>A instance of a RateFoonoteControl.</returns>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new UspsRatePromotionFootnote();
        }
    }
}
