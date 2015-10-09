using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    public interface IUpsPromoFactory
    {
        /// <summary>
        /// Gets a UpsPromo
        /// </summary>
        UpsPromo Get(UpsAccountEntity account);
    }
}