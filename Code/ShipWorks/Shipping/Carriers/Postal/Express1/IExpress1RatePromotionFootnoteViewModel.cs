using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Express1 rate promotion
    /// </summary>
    public interface IExpress1RatePromotionFootnoteViewModel
    {
        /// <summary>
        /// Settings for the Express 1 dialog
        /// </summary>
        IExpress1SettingsFacade Settings { get; set; }

        /// <summary>
        /// Shipment associated with the current rates
        /// </summary>
        ICarrierShipmentAdapter ShipmentAdapter { get; set; }
    }
}