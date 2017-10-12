using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Factory to create a RateShipmentRequest from a ShipmentEntity
    /// </summary>
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

        public List<Package> CreatePackages(List<IPackageAdapter> packages)
        {
            List<Package> apiPackages = new List<Package>();
            foreach (IPackageAdapter package in packages)
            {
                Package apiPackage = new Package()
                {
                    Dimensions = new Dimensions()
                    {
                        Length = package.DimsLength,
                        Width = package.DimsWidth,
                        Height = package.DimsHeight,
                        Unit = Dimensions.UnitEnum.Inch
                    }
                };
                apiPackages.Add(apiPackage);
            }

            return apiPackages;
        }

        /// <summary>
        /// Creates customs for a ShipEngine request
        /// </summary>
        public InternationalOptions CreateCustoms(ShipmentEntity shipment, IShipEngineShipment shipEngineShipment)
        {
            InternationalOptions customs = new InternationalOptions()
            {
                Contents = (InternationalOptions.ContentsEnum) shipEngineShipment.Contents,
                CustomsItems = CreateCustomsItems(shipment),
                NonDelivery = (InternationalOptions.NonDeliveryEnum) shipEngineShipment.NonDelivery
            };

            return customs;
        }

        /// <summary>
        /// Create the Customs Items
        /// </summary>
        private List<CustomsItem> CreateCustomsItems(ShipmentEntity shipment)
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
    }
}
