using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Repository for DHL Express accounts
    /// </summary>
    public interface IDhlExpressAccountRepository
    {
        /// <summary>
        /// Saves the given DHL Express account entity to the underlying data source.
        /// </summary>
        void Save(DhlExpressAccountEntity account);
    }
}