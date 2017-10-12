using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Repository for DHL Express accounts
    /// </summary>
    public interface IDhlExpressAccountRepository : ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity>
    {
        /// <summary>
        /// Gets all dhl accounts in database
        /// </summary>
        IEnumerable<DhlExpressAccountEntity> Accounts { get; }

        /// <summary>
        /// Saves the given DHL Express account entity to the underlying data source.
        /// </summary>
        void Save(DhlExpressAccountEntity account);
    }
}