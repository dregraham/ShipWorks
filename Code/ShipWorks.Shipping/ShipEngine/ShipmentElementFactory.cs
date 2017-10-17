using System;
using System.Collections.Generic;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Business;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Factory to create a RateShipmentRequest from a ShipmentEntity
    /// </summary>
    [Component(SingleInstance = true)]
    public class ShipmentElementFactory : IShipmentElementFactory
    {
        /// <summary>
        ///  Create a RateShipmentRequest from a ShipmentEntity
        /// </summary>
        public RateShipmentRequest CreateRateRequest(ShipmentEntity shipment)
        {
            RateShipmentRequest request = new RateShipmentRequest()
            {
                Shipment = new AddressValidatingShipment()
                {
                    ShipTo = CreateAddress(shipment, "Ship"),
                    ShipFrom = CreateAddress(shipment, "Origin"),
                    TotalWeight = new Weight(shipment.TotalWeight, Weight.UnitEnum.Pound)
                }
            };
            return request;
        }

        /// <summary>
        /// Create a PurchaseLabelRequest from a shipment, packages and service code
        /// </summary>
        public PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment, List<IPackageAdapter> packages, string serviceCode)
        {
            PurchaseLabelRequest request = new PurchaseLabelRequest()
            {
                LabelFormat = GetLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat),
                LabelLayout = "4x6",
                Shipment = new Shipment()
                {
                    ShipTo = CreateAddress(shipment, "Ship"),
                    ShipFrom = CreateAddress(shipment, "Origin"),
                    TotalWeight = new Weight(shipment.TotalWeight, Weight.UnitEnum.Pound),
                    Packages = CreatePackages(packages),
                    ServiceCode = serviceCode
                }
            };
            return request;
        }

        /// <summary>
        /// Gets an AddressDTO from a shipment
        /// </summary>
        private AddressDTO CreateAddress(ShipmentEntity shipment, string fieldPrefix)
        {
            PersonAdapter personAdapter = new PersonAdapter(shipment, fieldPrefix);

            AddressDTO address = new AddressDTO()
            {
                AddressResidentialIndicator = AddressDTO.AddressResidentialIndicatorEnum.Unknown,
                Name = personAdapter.UnparsedName,
                Phone = personAdapter.Phone,
                CompanyName = personAdapter.Company,
                AddressLine1 = personAdapter.Street1,
                AddressLine2 = personAdapter.Street2,
                AddressLine3 = personAdapter.Street3,
                CityLocality = personAdapter.City,
                StateProvince = personAdapter.StateProvCode,
                PostalCode = personAdapter.PostalCode,
                CountryCode = personAdapter.CountryCode
            };

            return address;
        }

        /// <summary>
        /// Creates ShipEngine api package DTOs from a list of package adapters.
        /// </summary>
        public List<ShipmentPackage> CreatePackages(List<IPackageAdapter> packages)
        {
            List<ShipmentPackage> apiPackages = new List<ShipmentPackage>();
            foreach (IPackageAdapter package in packages)
            {
                ShipmentPackage apiPackage = new ShipmentPackage()
                {
                    Dimensions = new Dimensions()
                    {
                        Length = package.DimsLength,
                        Width = package.DimsWidth,
                        Height = package.DimsHeight,
                        Unit = Dimensions.UnitEnum.Inch
                    },
                    Weight = new Weight(package.Weight, Weight.UnitEnum.Pound)
                };
                apiPackages.Add(apiPackage);
            }

            return apiPackages;
        }

        /// <summary>
        /// Create the Customs Items
        /// </summary>
        public List<CustomsItem> CreateCustomsItems(ShipmentEntity shipment)
        {
            List<CustomsItem> customItems = new List<CustomsItem>();

            foreach (ShipmentCustomsItemEntity item in shipment.CustomsItems)
            {
                CustomsItem apiItem = new CustomsItem()
                {
                    CountryOfOrigin = item.CountryOfOrigin,
                    Description = item.Description,
                    HarmonizedTariffCode = item.HarmonizedCode,
                    Quantity = (int) Math.Round(item.Quantity, 0, MidpointRounding.AwayFromZero),
                    Value = (double) item.UnitValue
                };
                customItems.Add(apiItem);
            }

            return customItems;
        }

        /// <summary>
        /// Return Api value for ThermalLanguage
        /// </summary>
        /// <remarks>
        /// ShipEngine doesn't support EPL if recieved, a ShipEngineException is thrown
        /// </remarks>
        private PurchaseLabelRequest.LabelFormatEnum? GetLabelFormat(ThermalLanguage requestedLabelFormat)
        {
            switch (requestedLabelFormat)
            {
                case ThermalLanguage.None:
                    return PurchaseLabelRequest.LabelFormatEnum.Pdf;
                case ThermalLanguage.EPL:
                    throw new ShipEngineException("Carrier does not support EPL.");
                case ThermalLanguage.ZPL:
                    return PurchaseLabelRequest.LabelFormatEnum.Zpl;
            }

            throw new ArgumentOutOfRangeException($"Invalid Thermal Language in GetLabelFormat: '{requestedLabelFormat}'");
        }
    }
}
