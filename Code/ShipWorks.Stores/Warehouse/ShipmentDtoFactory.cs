using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
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
        public Shipment CreateHubShipment(ShipmentEntity shipmentEntity)
        {
            InsuranceProvider insuranceType = (InsuranceProvider) shipmentEntity.InsuranceProvider;

            ICarrierShipmentAdapter shipmentAdapter = shipmentAdapterFactory.Get(shipmentEntity);

            Shipment shipment = new Shipment
            {
                TangoShipmentId = shipmentEntity.OnlineShipmentID,
                ShipworksShipmentId = shipmentEntity.ShipmentID.ToString(),
                Carrier = EnumHelper.GetDescription(shipmentEntity.ShipmentTypeCode),
                Service = shipmentAdapter.ServiceTypeName,
                TrackingNumber = shipmentEntity.TrackingNumber,
                ShipDate = shipmentEntity.ShipDate,
                CarrierCost = shipmentEntity.ShipmentCost,
                ShipworksInsured = Convert.ToInt32(insuranceType == InsuranceProvider.ShipWorks),
                CarrierInsured = Convert.ToInt32(insuranceType == InsuranceProvider.Carrier),
                IsReturn = Convert.ToInt32(shipmentEntity.ReturnShipment),
                RecipientAddress = new RecipientAddress
                {
                    Name = shipmentEntity.ShipUnparsedName,
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
                LabelFormat = shipmentEntity.ActualLabelFormat ?? 0,
                //EstimatedDeliveryDate = this is all over the place, skipping for now
                Packages = CreatePackages(shipmentAdapter.GetPackageAdapters())
            };

            return shipment;
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
                    PackagingType = packageAdapter.PackagingType
                });
    }
}
