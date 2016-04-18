using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// An implementation of the IRateFootnoteFactory interface intended to be used to display a footnote
    /// indicating that a shipping account is required to get rates.
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Editing.Rating.IRateFootnoteFactory" />
    public class ShippingAccountRequiredForRatingFootnoteFactory : IRateFootnoteFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingAccountRequiredForRatingFootnoteFactory"/> class.
        /// </summary>
        /// <param name="shipmentTypeCode">The shipment type code that the footnote pertains to.</param>
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
        /// <returns>A instance of a RateFootnoteControl.</returns>
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
