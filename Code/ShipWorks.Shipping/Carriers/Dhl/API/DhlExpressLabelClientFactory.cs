using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Dhl.API.ShipEngine;
using ShipWorks.Shipping.Carriers.Dhl.API.Stamps;

namespace ShipWorks.Shipping.Carriers.Dhl.API
{
    /// <summary>
    /// Factory for creating DHL Express label clients
    /// </summary>
    [Component]
    public class DhlExpressLabelClientFactory : IDhlExpressLabelClientFactory
    {
        private readonly Func<DhlExpressShipEngineLabelClient> createShipEngineLabelClient;
        private readonly Func<DhlExpressStampsLabelClient> createStampsLabelClient;
        private readonly IDhlExpressAccountRepository accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressLabelClientFactory(Func<DhlExpressShipEngineLabelClient> createShipEngineLabelClient,
                                            Func<DhlExpressStampsLabelClient> createStampsLabelClient,
                                            IDhlExpressAccountRepository accountRepository)
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
