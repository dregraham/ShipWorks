using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// Interface for DHL eCom account repo
    /// </summary>
    public interface IDhlEcommerceAccountRepository : ICarrierAccountRepository<DhlEcommerceAccountEntity, IDhlEcommerceAccountEntity>
    {
        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        IEnumerable<DhlEcommerceAccountEntity> Accounts { get; }
    }
}
