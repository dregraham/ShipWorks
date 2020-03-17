﻿using System;
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
    public class ShipEngineRequestFactory : IShipEngineRequestFactory
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
                    ShipTo = CreateAddress(shipment.ShipPerson),
                    ShipFrom = CreateAddress(shipment.OriginPerson),
                    ShipDate = shipment.ShipDate,
                    // TotalWeight = new Weight(shipment.TotalWeight, Weight.UnitEnum.Pound)
                }
            };
            return request;
        }

        /// <summary>
        /// Create a PurchaseLabelWithoutShipmentRequest from a shipment, packages and service code
        /// </summary>
        public PurchaseLabelWithoutShipmentRequest CreatePurchaseLabelWithoutShipmentRequest(ShipmentEntity shipment)
        {
            return new PurchaseLabelWithoutShipmentRequest()
            {
                LabelFormat = GetPurchaseLabelWithoutShipmentRequestLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat),
                LabelLayout = "4x6"
            };
        }

        /// <summary>
        /// Create a PurchaseLabelRequest from a shipment, packages and service code
        /// </summary>
        public PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment, List<IPackageAdapter> packages, string serviceCode, 
            Func<IPackageAdapter, string> getPackageCode, Action<ShipmentPackage, IPackageAdapter> addPackageInsurance)
        {
            PurchaseLabelRequest request = new PurchaseLabelRequest()
            {
                LabelFormat = GetPurchaseLabelRequestLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat),
                LabelLayout = "4x6",
                Shipment = new Shipment()
                {
                    ShipTo = CreateAddress(shipment.ShipPerson),
                    ShipFrom = CreateAddress(shipment.OriginPerson),
                    ShipDate = shipment.ShipDate,
                    // TotalWeight = new Weight(shipment.TotalWeight, Weight.UnitEnum.Pound),
                    Packages = CreatePackageForLabel(packages, getPackageCode, addPackageInsurance),
                    ServiceCode = serviceCode
                }
            };
            return request;
        }

        /// <summary>
        /// Gets an AddressDTO from a shipment
        /// </summary>
        private Address CreateAddress(PersonAdapter personAdapter)
        {
            string countryCode = personAdapter.CountryCode == "UK" ? "GB" : personAdapter.CountryCode;
            Address address = new Address
            {
                AddressResidentialIndicator = Address.AddressResidentialIndicatorEnum.Unknown,
                Name = personAdapter.UnparsedName,
                Phone = personAdapter.Phone,
                CompanyName = personAdapter.Company,
                AddressLine1 = personAdapter.Street1,
                AddressLine2 = personAdapter.Street2,
                AddressLine3 = personAdapter.Street3,
                CityLocality = personAdapter.City,
                StateProvince = personAdapter.StateProvCode,
                PostalCode = personAdapter.PostalCode,
                CountryCode =  countryCode
            };

            return address;
        }

        /// <summary>
        /// Creates ShipEngine api package DTOs for rating from a list of package adapters.
        /// </summary>
        public List<ShipmentPackage> CreatePackageForRating(List<IPackageAdapter> packages, Action<ShipmentPackage, IPackageAdapter> addPackageInsurance) =>
            CreatePackageInternal(packages, null, addPackageInsurance);

        /// <summary>
        /// Creates ShipEngine api package DTOs for a label request from a list of package adapters.
        /// </summary>
        public List<ShipmentPackage> CreatePackageForLabel(List<IPackageAdapter> packages,
            Func<IPackageAdapter, string> getPackageCode, Action<ShipmentPackage, IPackageAdapter> addPackageInsurance) =>
            CreatePackageInternal(packages, getPackageCode, addPackageInsurance);

        /// <summary>
        /// Creates ShipEngine api package DTOs for a label or rate request from a list of package adapters.
        /// </summary>
        private List<ShipmentPackage> CreatePackageInternal(List<IPackageAdapter> packages,
            Func<IPackageAdapter, string> getPackageCode, Action<ShipmentPackage, IPackageAdapter> addPackageInsurance)
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
                    Weight = new Weight(package.Weight, Weight.UnitEnum.Pound),
                    PackageCode = getPackageCode?.Invoke(package),
                    LabelMessages = new LabelMessages()
                };

                addPackageInsurance(apiPackage, package);
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
        private PurchaseLabelRequest.LabelFormatEnum? GetPurchaseLabelRequestLabelFormat(ThermalLanguage requestedLabelFormat)
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

        /// <summary>
        /// Return Api value for ThermalLanguage
        /// </summary>
        /// <remarks>
        /// ShipEngine doesn't support EPL if recieved, a ShipEngineException is thrown
        /// </remarks>
        private PurchaseLabelWithoutShipmentRequest.LabelFormatEnum? GetPurchaseLabelWithoutShipmentRequestLabelFormat(ThermalLanguage requestedLabelFormat)
        {
            switch (requestedLabelFormat)
            {
                case ThermalLanguage.None:
                    return PurchaseLabelWithoutShipmentRequest.LabelFormatEnum.Pdf;
                case ThermalLanguage.EPL:
                    throw new ShipEngineException("Carrier does not support EPL.");
                case ThermalLanguage.ZPL:
                    return PurchaseLabelWithoutShipmentRequest.LabelFormatEnum.Zpl;
            }

            throw new ArgumentOutOfRangeException($"Invalid Thermal Language in GetLabelFormat: '{requestedLabelFormat}'");
        }
    }
}
