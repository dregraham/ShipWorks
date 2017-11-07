using ShipWorks.Shipping.ShipEngine;
using System.Collections.Generic;
using System.Linq;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Services;
using Interapptive.Shared.Enums;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Factory for creating Asendia RateShipmentRequests
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

        protected override void EnsureCarrierShipmentIsNotNull(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.Asendia, nameof(shipment.Asendia));
        }

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
        /// Creates customs for a ShipEngine request
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

        protected override List<IPackageAdapter> GetPackages(ShipmentEntity shipment)
        {
            return shipmentTypeManager.Get(ShipmentTypeCode.Asendia).GetPackageAdapters(shipment).ToList();
        }

        protected override string GetServiceApiValue(ShipmentEntity shipment)
        {
            return EnumHelper.GetApiValue(shipment.Asendia.Service);
        }

        protected override Dictionary<string, object> CreateAdvancedOptions(ShipmentEntity shipment)
        {
            return new Dictionary<string, object>()
            {
                {"non_machinable", shipment.Asendia.NonMachinable}
            };
        }
    }
}
