using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// UpsPromoFactoryFactory Inteface
    /// </summary>
    public interface IUpsPromoFootnoteFactoryFactory
    {
        /// <summary>
        /// Returns a UpsPromoFootnoteFactory if applicable - else null
        /// </summary>
        UpsPromoFootnoteFactory Get(ShipmentEntity shipment);
    }
}