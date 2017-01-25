using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes
{
    public interface IUpsPromoFootnoteViewModel
    {
        /// <summary>
        /// The UPS promo.
        /// </summary>
        IUpsPromo UpsPromo { get; set; }

        /// <summary>
        /// Shipment adapter associated with the current rates
        /// </summary>
        ICarrierShipmentAdapter ShipmentAdapter { get; set; }
    }
}