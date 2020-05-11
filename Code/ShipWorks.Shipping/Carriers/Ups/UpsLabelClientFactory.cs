using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Ups.OneBalance;
using ShipWorks.Shipping.Carriers.Ups.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Factory for creating Ups Label Clients
    /// </summary>
    [Component]
    public class UpsLabelClientFactory : IUpsLabelClientFactory
    {
        private readonly Func<UpsShipEngineLabelClient> seLabelClientFactory;
        private readonly Func<UpsOltLabelClient> oltLabelClientFactory;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLabelClientFactory(Func<UpsShipEngineLabelClient> seLabelClientFactory, Func<UpsOltLabelClient> oltLabelClientFactory, 
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository)
        {
            this.seLabelClientFactory = seLabelClientFactory;
            this.oltLabelClientFactory = oltLabelClientFactory;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Get a label client for the given shipment
        /// </summary>
        public IUpsLabelClient GetClient(IShipmentEntity shipment)
        {
            IUpsAccountEntity account = accountRepository.GetAccount(shipment);

            if (account?.ShipEngineCarrierId != null)
            {
                return seLabelClientFactory();
            }

            return oltLabelClientFactory();
        }
    }
}
