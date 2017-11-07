using System.Collections.Generic;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.ShipEngine
{
    public abstract class ShipEngineShipmentRequestFactory : ICarrierShipmentRequestFactory
    {
        private readonly IShipEngineRequestFactory shipmentElementFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineShipmentRequestFactory(IShipEngineRequestFactory shipmentElementFactory,
            IShipmentTypeManager shipmentTypeManager)
        {
            this.shipmentElementFactory = shipmentElementFactory;
            this.shipmentTypeManager = shipmentTypeManager;
        }

        public PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            EnsureCarrierShipmentIsNotNull(shipment);

            string serviceApiValue = GetServiceApiValue(shipment);
            List<IPackageAdapter> packages = GetPackages(shipment);

            PurchaseLabelRequest request = shipmentElementFactory.CreatePurchaseLabelRequest(shipment, packages, serviceApiValue);
            request.Shipment.CarrierId = GetShipEngineCarrierID(shipment);
            request.Shipment.AdvancedOptions = CreateAdvancedOptions(shipment);
            request.Shipment.Customs = CreateCustoms(shipment);

            return request;
        }

        public RateShipmentRequest CreateRateShipmentRequest(ShipmentEntity shipment)
        {
            if (shipmentTypeManager.Get(shipment.ShipmentTypeCode).SupportsGetRates)
            {
                MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
                EnsureCarrierShipmentIsNotNull(shipment);

                RateShipmentRequest request = shipmentElementFactory.CreateRateRequest(shipment);
                request.RateOptions = new RateRequest() { CarrierIds = new List<string> { GetShipEngineCarrierID(shipment) } };

                request.Shipment.AdvancedOptions = CreateAdvancedOptions(shipment);
                request.Shipment.Customs = CreateCustoms(shipment);

                List<IPackageAdapter> packages = GetPackages(shipment);
                request.Shipment.Packages = shipmentElementFactory.CreatePackages(packages);

                return request;
            }

            return new RateShipmentRequest();
        }

        protected abstract void EnsureCarrierShipmentIsNotNull(ShipmentEntity shipment);

        protected abstract string GetShipEngineCarrierID(ShipmentEntity shipment);

        protected abstract InternationalOptions CreateCustoms(ShipmentEntity shipment);

        protected abstract List<IPackageAdapter> GetPackages(ShipmentEntity shipment);

        protected abstract string GetServiceApiValue(ShipmentEntity shipment);

        protected abstract Dictionary<string, object> CreateAdvancedOptions(ShipmentEntity shipment);
    }
}
