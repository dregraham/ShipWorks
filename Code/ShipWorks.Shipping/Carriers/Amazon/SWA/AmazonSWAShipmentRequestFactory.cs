using ShipWorks.Shipping.ShipEngine;
using System.Collections.Generic;
using System.Linq;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Factory for creating AmazonSWA ShipmentRequests
    /// </summary>
    [KeyedComponent(typeof(ICarrierShipmentRequestFactory), ShipmentTypeCode.AmazonSWA)]
    public class AmazonSWAShipmentRequestFactory : ShipEngineShipmentRequestFactory
    {
        private readonly IAmazonSWAAccountRepository accountRepository;
        private readonly IShipEngineRequestFactory shipmentElementFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSWAShipmentRequestFactory(IAmazonSWAAccountRepository accountRepository,
            IShipEngineRequestFactory shipmentElementFactory,
            IShipmentTypeManager shipmentTypeManager)
            : base(shipmentElementFactory, shipmentTypeManager)
        {
            this.accountRepository = accountRepository;
            this.shipmentElementFactory = shipmentElementFactory;
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// Ensures the AmazonSWA shipment is not null
        /// </summary>
        protected override void EnsureCarrierShipmentIsNotNull(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.AmazonSWA, nameof(shipment.AmazonSWA));
        }

        /// <summary>
        /// Gets the ShipEngine carrier ID from the AmazonSWA shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment)
        {
            AmazonSWAAccountEntity account = accountRepository.GetAccount(shipment);

            if (account == null)
            {
                throw new AmazonSWAException("Invalid account associated with shipment.");
            }

            return account.ShipEngineCarrierId;
        }

        /// <summary>
        /// Gets the api value for the AmazonSWA service
        /// </summary>
        protected override string GetServiceApiValue(ShipmentEntity shipment)
        {
            return EnumHelper.GetApiValue((AmazonSWAServiceType) shipment.AmazonSWA.Service);
        }

        /// <summary>
        /// Creates the AmazonSWA advanced options node
        /// </summary>
        protected override AdvancedOptions CreateAdvancedOptions(ShipmentEntity shipment)
        {
            return new AdvancedOptions();
        }

        /// <summary>
        /// Creates the AmazonSWA customs node
        /// </summary>
        protected override InternationalOptions CreateCustoms(ShipmentEntity shipment)
        {
            InternationalOptions customs = new InternationalOptions()
            {
                Contents = (InternationalOptions.ContentsEnum) shipment.AmazonSWA.Contents,
                CustomsItems = shipmentElementFactory.CreateCustomsItems(shipment),
                NonDelivery = (InternationalOptions.NonDeliveryEnum) shipment.AmazonSWA.NonDelivery
            };

            return customs;
        }
    }
}
