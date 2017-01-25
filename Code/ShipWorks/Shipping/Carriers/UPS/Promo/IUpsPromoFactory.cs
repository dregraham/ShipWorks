using ShipWorks.Data.Model.EntityClasses;

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