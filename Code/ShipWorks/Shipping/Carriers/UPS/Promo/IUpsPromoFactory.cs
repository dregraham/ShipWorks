using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes;

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
        IUpsPromo Get(UpsAccountEntity account, bool existingAccount, bool newAccount);

        /// <summary>
        /// Creates the footnote factory for the account
        /// </summary>
        UpsPromoFootnoteFactory GetFootnoteFactory(UpsAccountEntity account);
    }
}