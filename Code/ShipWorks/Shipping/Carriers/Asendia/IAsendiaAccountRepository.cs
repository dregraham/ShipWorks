using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Repository for Asendia accounts
    /// </summary>
    public interface IAsendiaAccountRepository : ICarrierAccountRepository<ShipEngineAccountEntity, IShipEngineAccountEntity>
    {
    }
}