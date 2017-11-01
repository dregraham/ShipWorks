using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping.ShipEngine
{
        public interface IShipEngineAccountRepository : ICarrierAccountRepository<ShipEngineAccountEntity, IShipEngineAccountEntity>
        {
        }
}
