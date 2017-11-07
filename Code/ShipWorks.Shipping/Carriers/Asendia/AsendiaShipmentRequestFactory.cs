using ShipWorks.Shipping.ShipEngine;
using System.Collections.Generic;
using System.Linq;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Factory for creating Asendia ShipmentRequests
    /// </summary>
    [KeyedComponent(typeof(ICarrierShipmentRequestFactory), ShipmentTypeCode.Asendia)]
    public class AsendiaShipmentRequestFactory : ShipEngineShipmentRequestFactory
    {
        private readonly IAsendiaAccountRepository accountRepository;
        private readonly IShipEngineRequestFactory shipmentElementFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public AsendiaShipmentRequestFactory(IAsendiaAccountRepository accountRepository,
            IShipEngineRequestFactory shipmentElementFactory,
            IShipmentTypeManager shipmentTypeManager) 
            : base(shipmentElementFactory, shipmentTypeManager)
        {
            this.accountRepository = accountRepository;
            this.shipmentElementFactory = shipmentElementFactory;
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// Ensures the Asendia shipment is not null
        /// </summary>
        protected override void EnsureCarrierShipmentIsNotNull(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.Asendia, nameof(shipment.Asendia));
        }

        /// <summary>
        /// Gets the ShipEngine carrier ID from the Asendia shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment)
        {
            AsendiaAccountEntity account = accountRepository.GetAccount(shipment);

            if (account == null)
            {
                throw new AsendiaException("Invalid account associated with shipment.");
            }

            return account.ShipEngineCarrierId;
        }

        /// <summary>
        /// Gets the api value for the Asendia service
        /// </summary>
        protected override string GetServiceApiValue(ShipmentEntity shipment)
        {
            return EnumHelper.GetApiValue(shipment.Asendia.Service);
        }

        /// <summary>
        /// Creates the Asendia advanced options node
        /// </summary>
        protected override Dictionary<string, object> CreateAdvancedOptions(ShipmentEntity shipment)
        {
            return new Dictionary<string, object>()
            {
                {"non_machinable", shipment.Asendia.NonMachinable}
            };
        }

        /// <summary>
        /// Creates the Asendia customs node
        /// </summary>
        protected override InternationalOptions CreateCustoms(ShipmentEntity shipment)
        {
            InternationalOptions customs = new InternationalOptions()
            {
                Contents = (InternationalOptions.ContentsEnum) shipment.Asendia.Contents,
                CustomsItems = shipmentElementFactory.CreateCustomsItems(shipment),
                NonDelivery = (InternationalOptions.NonDeliveryEnum) shipment.Asendia.NonDelivery
            };

            return customs;
        }
    }
}
