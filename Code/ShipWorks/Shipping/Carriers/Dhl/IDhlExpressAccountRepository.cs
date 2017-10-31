using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Repository for DHL Express accounts
    /// </summary>
    public interface IDhlExpressAccountRepository : ICarrierAccountRepository<ShipEngineAccountEntity, IShipEngineAccountEntity>
    {
    }
}