using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Factory for creating shipment dtos to send to SW hub
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ShipmentDtoFactory
    {
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDtoFactory(ICarrierShipmentAdapterFactory shipmentAdapterFactory)
        {
            this.shipmentAdapterFactory = shipmentAdapterFactory;
        }

        /// <summary>
        /// Create a shipment to send to SW hub
        /// </summary>
        public Shipment CreateHubShipment(ShipmentEntity shipmentEntity, string tangoShipmentID)
        {
            ICarrierShipmentAdapter shipmentAdapter = shipmentAdapterFactory.Get(shipmentEntity);

            InsuranceProvider insuranceType = (InsuranceProvider) shipmentEntity.InsuranceProvider;

            int shipworksInsured = 0;
            int carrierInsured = 0;

            if (shipmentEntity.Insurance)
            {
                shipworksInsured = Convert.ToInt32(insuranceType == InsuranceProvider.ShipWorks);
                carrierInsured = Convert.ToInt32(insuranceType == InsuranceProvider.Carrier);
            }

            Shipment shipment = new Shipment
            {
                TangoShipmentId = Convert.ToInt64(tangoShipmentID),
                ShipworksShipmentId = shipmentEntity.ShipmentID,
                ShippingProviderId = shipmentEntity.ShipmentType,
                Carrier = GetCarrierName(shipmentEntity),
                Service = shipmentAdapter.ServiceTypeName,
                TrackingNumber = shipmentEntity.TrackingNumber,
                ShipDate = shipmentEntity.ShipDate,
                CarrierCost = shipmentEntity.ShipmentCost,
                ShipworksInsured = shipworksInsured,
                CarrierInsured = carrierInsured,
                IsReturn = Convert.ToInt32(shipmentEntity.ReturnShipment),
                RecipientAddress = new RecipientAddress
                {
                    FirstName = shipmentEntity.ShipFirstName,
                    LastName = shipmentEntity.ShipLastName,
                    Street1 = shipmentEntity.ShipStreet1,
                    Street2 = shipmentEntity.ShipStreet2,
                    Street3 = shipmentEntity.ShipStreet3,
                    City = shipmentEntity.ShipCity,
                    StateCode = shipmentEntity.ShipStateProvCode,
                    PostalCode = shipmentEntity.ShipPostalCode,
                    CountryCode = shipmentEntity.ShipCountryCode
                },
                OriginPostalCode = shipmentEntity.OriginPostalCode,
                OriginCountryCode = shipmentEntity.OriginCountryCode,
                ShippingAccount = shipmentAdapter.AccountId.ToString(),
                LabelFormat = GetLabelFormat(shipmentEntity),
                Packages = CreatePackages(shipmentAdapter.GetPackageAdapters())
            };

            return shipment;
        }

        /// <summary>
        /// Gets the label format
        /// </summary>
        private static string GetLabelFormat(ShipmentEntity shipmentEntity)
        {
            if (shipmentEntity.ActualLabelFormat == null)
            {
                return "Unknown";
            }
            return EnumHelper.GetDescription((ThermalLanguage) (shipmentEntity.ActualLabelFormat));
        }

        /// <summary>
        /// Create hub packages using package adapters
        /// </summary>
        private static IEnumerable<Package> CreatePackages(IEnumerable<IPackageAdapter> packageAdapters) =>
            packageAdapters.Select(packageAdapter => new Package
                {
                    WeightInPounds = packageAdapter.Weight,
                    LengthInInches = packageAdapter.DimsLength,
                    WidthInInches = packageAdapter.DimsWidth,
                    HeightInInches = packageAdapter.DimsHeight,
                    PackagingType = packageAdapter.PackagingTypeName
                });

        /// <summary>
        /// Get carrier name for the shipment
        /// </summary>
        private string GetCarrierName(ShipmentEntity shipment)
        {
            ShipmentTypeCode shipmentType = shipment.ShipmentTypeCode;

            return shipmentType == ShipmentTypeCode.Other ?
                shipment.Other.Carrier :
                EnumHelper.GetDescription(shipmentType);
        }
    }
}
