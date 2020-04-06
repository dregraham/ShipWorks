using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Promo;

namespace ShipWorks.Shipping.Carriers.Ups.Promo
{
    /// <summary>
    /// Null Promo Factory
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.UPS.Promo.IUpsPromoFactory" />
    public class NullPromoFactory : IUpsPromoFactory
    {
        /// <summary>
        /// Returns null
        /// </summary>
        public IUpsPromo Get(UpsAccountEntity account, UpsPromoSource source, UpsPromoAccountType accountType)
        {
            return null;
        }
    }
}
