using System;
using System.Collections.Generic;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using Interapptive.Shared.ComponentRegistration;

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
                    ShipTo = new AddressDTO()
                    {
                        AddressResidentialIndicator = AddressDTO.AddressResidentialIndicatorEnum.Unknown,
                        Name = shipment.ShipUnparsedName,
                        Phone = shipment.ShipPhone,
                        CompanyName = shipment.ShipCompany,
                        AddressLine1 = shipment.ShipStreet1,
                        AddressLine2 = shipment.ShipStreet2,
                        AddressLine3 = shipment.ShipStreet3,
                        CityLocality = shipment.ShipCity,
                        StateProvince = shipment.ShipStateProvCode,
                        PostalCode = shipment.ShipPostalCode,
                        CountryCode = shipment.ShipCountryCode
                    },
                    ShipFrom= new AddressDTO()
                    {
                        AddressResidentialIndicator = AddressDTO.AddressResidentialIndicatorEnum.Unknown,
                        Name = shipment.OriginUnparsedName,
                        Phone = shipment.OriginPhone,
                        CompanyName = shipment.OriginCompany,
                        AddressLine1 = shipment.OriginStreet1,
                        AddressLine2 = shipment.OriginStreet2,
                        AddressLine3 = shipment.OriginStreet3,
                        CityLocality = shipment.OriginCity,
                        StateProvince = shipment.OriginStateProvCode,
                        PostalCode = shipment.OriginPostalCode,
                        CountryCode = shipment.OriginCountryCode
                    },
                    Customs=null,
                    TotalWeight = new Weight(shipment.TotalWeight, Weight.UnitEnum.Pound)
                }
            };
            return request;
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
        /// Create a PurchaseLabelRequest from a shipment, packages and service code
        /// </summary>
        public PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment, List<IPackageAdapter> packages, string serviceCode)
        {
            throw new NotImplementedException();
        }
    }
}
