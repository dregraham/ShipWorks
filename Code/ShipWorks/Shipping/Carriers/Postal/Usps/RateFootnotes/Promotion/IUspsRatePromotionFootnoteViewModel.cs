using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion
{
    /// <summary>
    /// USPS promotion footnote
    /// </summary>
    public interface IUspsRatePromotionFootnoteViewModel
    {
        /// <summary>
        /// Shipment associated with the promotion
        /// </summary>
        ICarrierShipmentAdapter ShipmentAdapter { get; set; }

        /// <summary>
        /// Should the single account dialog be displayed
        /// </summary>
        bool ShowSingleAccountDialog { get; set; }
    }
}