using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    public class ShippingAccountRequiredForRatingFootnoteFactory : IRateFootnoteFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InformationFootnoteFactory"/> class.
        /// </summary>
        public ShippingAccountRequiredForRatingFootnoteFactory(ShipmentTypeCode shipmentTypeCode)
        {
            ShipmentTypeCode = shipmentTypeCode;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        /// <param name="parameters">Parameters that allow footnotes to interact with the rates grid</param>
        /// <returns>A instance of a RateFoonoteControl.</returns>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new ShippingAccountRequiredForRatingFootnoteControl();
        }

        /// <summary>
        /// Notes that this factory should or should not be used in BestRate
        /// For example, when using BestRate, we do not want Usps promo footnotes to display, so this will be set to false.
        /// </summary>
        public bool AllowedForBestRate
        {
            get { return true; }
        }
    }
}
