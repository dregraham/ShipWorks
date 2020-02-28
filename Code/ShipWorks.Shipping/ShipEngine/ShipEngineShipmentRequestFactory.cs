﻿using System.Collections.Generic;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Services;
using System.Linq;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Base class for creating ShipEngine shipment requests
    /// </summary>
    public abstract class ShipEngineShipmentRequestFactory : ICarrierShipmentRequestFactory
    {
        private readonly IShipEngineRequestFactory shipmentElementFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ShipEngineShipmentRequestFactory(IShipEngineRequestFactory shipmentElementFactory,
            IShipmentTypeManager shipmentTypeManager)
        {
            this.shipmentElementFactory = shipmentElementFactory;
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// Create a PurchaseLabelWithoutShipmentRequest
        /// </summary>
        public PurchaseLabelWithoutShipmentRequest CreatePurchaseLabelWithoutShipmentRequest(ShipmentEntity shipment)
        {
            return shipmentElementFactory.CreatePurchaseLabelWithoutShipmentRequest(shipment);
        }

        /// <summary>
        /// Creates a ShipEngine purchase label request
        /// </summary>
        public virtual PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            EnsureCarrierShipmentIsNotNull(shipment);

            string serviceApiValue = GetServiceApiValue(shipment);
            List<IPackageAdapter> packages = GetPackages(shipment);

            PurchaseLabelRequest request = shipmentElementFactory.CreatePurchaseLabelRequest(shipment, packages, serviceApiValue, GetPackagingCode);
            request.Shipment.CarrierId = GetShipEngineCarrierID(shipment);
            request.Shipment.AdvancedOptions = CreateAdvancedOptions(shipment);
            request.ValidateAddress = PurchaseLabelRequest.ValidateAddressEnum.NoValidation;

            if (shipmentTypeManager.Get(shipment.ShipmentTypeCode).IsCustomsRequired(shipment))
            {
                request.Shipment.Customs = CreateCustoms(shipment);
            }

            return request;
        }

        /// <summary>
        /// Creates a ShipEngine rate request
        /// </summary>
        public virtual RateShipmentRequest CreateRateShipmentRequest(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            EnsureCarrierShipmentIsNotNull(shipment);

            ShipmentType shipmentType = shipmentTypeManager.Get(shipment.ShipmentTypeCode);
            if (shipmentType.SupportsGetRates)
            {
                RateShipmentRequest request = shipmentElementFactory.CreateRateRequest(shipment);
                request.RateOptions = new RateRequest() { CarrierIds = new List<string> { GetShipEngineCarrierID(shipment) } };

                request.Shipment.AdvancedOptions = CreateAdvancedOptions(shipment);

                if (shipmentType.IsCustomsRequired(shipment))
                {
                    request.Shipment.Customs = CreateCustoms(shipment);
                }
                List<IPackageAdapter> packages = GetPackages(shipment);
                request.Shipment.Packages = shipmentElementFactory.CreatePackages(packages, GetPackagingCode);

                return request;
            }

            return new RateShipmentRequest();
        }

        /// <summary>
        /// Get the given package adapters package code
        /// </summary>
        protected virtual string GetPackagingCode(IPackageAdapter package) => string.Empty;

        /// <summary>
        /// Set insurance info for the given ShipmentPackage based onthe package adapter
        /// </summary>
        protected virtual void SetPackageInsurance(ShipmentPackage shipmentPackage, IPackageAdapter packageAdapter)
        {

        }

        /// <summary>
        /// Ensures the carrier specific shipment (ex. shipment.Dhl) is not null
        /// </summary>
        protected abstract void EnsureCarrierShipmentIsNotNull(ShipmentEntity shipment);

        /// <summary>
        /// Gets the ShipEngine carrier ID from the carrier specific shipment
        /// </summary>
        protected abstract string GetShipEngineCarrierID(ShipmentEntity shipment);

        /// <summary>
        /// Gets the api value for the carrier specific service
        /// </summary>
        protected abstract string GetServiceApiValue(ShipmentEntity shipment);

        /// <summary>
        /// Creates the ShipEngine advanced options node
        /// </summary>
        protected abstract AdvancedOptions CreateAdvancedOptions(ShipmentEntity shipment);

        /// <summary>
        /// Creates the ShipEngine customs node
        /// </summary>
        protected abstract InternationalOptions CreateCustoms(ShipmentEntity shipment);
        
        /// <summary>
        /// Gets the carrier specific packages
        /// </summary>
        private List<IPackageAdapter> GetPackages(ShipmentEntity shipment)
        {
            return shipmentTypeManager.Get(shipment.ShipmentTypeCode).GetPackageAdapters(shipment).ToList();
        }
    }
}
