using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// A factory interface for being able to create footnote controls.
    /// </summary>
    public interface IRateFootnoteFactory
    {
        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        /// <param name="parameters">Parameters that allow footnotes to interact with the rates grid</param>
        /// <returns>A instance of a RateFoonoteControl.</returns>
        RateFootnoteControl CreateFootnote(FootnoteParameters parameters);

        /// <summary>
        /// Get a view model that represents this footnote
        /// </summary>
        object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter);

        /// <summary>
        /// Notes that this factory should or should not be used in BestRate
        /// For example, when using BestRate, we do not want Usps promo footnotes to display, so this will be set to false.
        /// </summary>
        bool AllowedForBestRate { get; }
    }
}
