using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// Represents a factory for creating UpsPromo resources
    /// </summary>
    public interface IUpsPromoFactory
    {
        /// <summary>
        /// Gets a UpsPromo
        /// </summary>
        IUpsPromo Get(UpsAccountEntity account, UpsPromoSource source, UpsPromoAccountType accountType);
    }
}