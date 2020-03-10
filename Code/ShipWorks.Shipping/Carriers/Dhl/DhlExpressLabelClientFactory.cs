using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Factory for creating DHL Express label clients
    /// </summary>
    [Component]
    public class DhlExpressLabelClientFactory : IDhlExpressLabelClientFactory
    {
        private readonly Func<DhlExpressShipEngineLabelClient> createShipEngineLabelClient;
        private readonly Func<DhlExpressStampsLabelClient> createStampsLabelClient;
        private readonly ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressLabelClientFactory(Func<DhlExpressShipEngineLabelClient> createShipEngineLabelClient, Func<DhlExpressStampsLabelClient> createStampsLabelClient,
                                            ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> accountRepository)
        {
            this.createShipEngineLabelClient = createShipEngineLabelClient;
            this.createStampsLabelClient = createStampsLabelClient;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Get a label client for the given shipment
        /// </summary>
        public IDhlExpressLabelClient Create(IShipmentEntity shipment)
        {
            IDhlExpressAccountEntity account = accountRepository.GetAccount(shipment);

            if (account?.ShipEngineCarrierId != null)
            {
                return createShipEngineLabelClient();
            }

            return createStampsLabelClient();
        }
    }
}
