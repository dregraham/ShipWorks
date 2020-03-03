using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Ups.ShipEngine;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Factory for creating IUpsShipmentValidators
    /// </summary>
    [Component]
    public class UpsShipmentValidatorFactory : IUpsShipmentValidatorFactory
    {
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsShipmentValidatorFactory(ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Create a shipment validator for the given shipment
        /// </summary>
        public IUpsShipmentValidator Create(IShipmentEntity shipment) => 
            accountRepository.GetAccountReadOnly(shipment)?.ShipEngineCarrierId != null
                ? new UpsShipEngineShipmentValidator()
                : (IUpsShipmentValidator) new UpsOltShipmentValidator();
    }
}
