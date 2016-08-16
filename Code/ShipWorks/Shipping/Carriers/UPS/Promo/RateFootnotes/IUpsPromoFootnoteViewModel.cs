using ShipWorks.Shipping.Carriers.UPS.Promo.API;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes
{
    public interface IUpsPromoFootnoteViewModel
    {
        /// <summary>
        /// The UPS promo.
        /// </summary>
        IUpsPromo UpsPromo { get; set; }
    }
}