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
    public class AsendiaShipmentRequestFactory : ICarrierShipmentRequestFactory
    {
        private readonly IAsendiaAccountRepository accountRepository;
        private readonly IShipEngineRequestFactory shipmentElementFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Creates a PurchaseLabelRequest with Asendia specific details from the given shipment
        /// </summary>
        public PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Asendia, nameof(shipment.Asendia));

            AsendiaAccountEntity account = accountRepository.GetAccount(shipment);

            if (account == null)
            {
                throw new AsendiaException("Invalid account associated with shipment.");
            }

            List<IPackageAdapter> packages = shipmentTypeManager.Get(ShipmentTypeCode.Asendia).GetPackageAdapters(shipment).ToList();

            AsendiaServiceType service = (AsendiaServiceType) shipment.Asendia.Service;
            string serviceApiValue = EnumHelper.GetApiValue(service);

            PurchaseLabelRequest request = shipmentElementFactory.CreatePurchaseLabelRequest(shipment, packages, serviceApiValue);
            request.Shipment.CarrierId = account.ShipEngineCarrierId;

            request.Shipment.AdvancedOptions = new Dictionary<string, object>();
            request.Shipment.AdvancedOptions.Add("non_machinable", shipment.Asendia.NonMachinable);

            request.Shipment.Customs = CreateCustoms(shipment);

            return request;
        }

        /// <summary>
        /// Asendia does not support rates, so return empty rate request
        /// </summary>
        public RateShipmentRequest CreateRateShipmentRequest(ShipmentEntity shipment)
        {
            return new RateShipmentRequest();
        }
        
        /// <summary>
        /// Creates customs for a ShipEngine request
        /// </summary>
        private InternationalOptions CreateCustoms(ShipmentEntity shipment)
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
