using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes;

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
        public IUpsPromo Get(UpsAccountEntity account, bool existingAccount)
        {
            return null;
        }

        /// <summary>
        /// Returns null
        /// </summary>
        public UpsPromoFootnoteFactory GetFootnoteFactory(UpsAccountEntity account)
        {
            return null;
        }
    }
}
