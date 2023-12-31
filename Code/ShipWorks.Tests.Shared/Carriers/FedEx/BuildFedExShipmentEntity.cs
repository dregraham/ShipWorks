using System;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Tests.Shared.Carriers.FedEx
{
    /// <summary>
    /// A Utility class for building Test ShipmentEntities
    /// </summary>
    public static class BuildFedExShipmentEntity
    {
        /// <summary>
        /// Create's a shipment entity
        /// </summary>
        /// <returns>Creates a shipment entity with a mocked FedExShipment and 2 empty FedExPackages</returns>
        public static ShipmentEntity SetupBaseShipmentEntity()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity
                {
                    Signature = (int) FedExSignatureType.NoSignature,
                    Service = (int) FedExServiceType.FedExGround,
                    NonStandardContainer = true,
                },
                ShipmentID = 77,
                OriginCountryCode = "US",
            };

            shipmentEntity.FedEx.Packages.AddRange(new[]
            {
                new FedExPackageEntity { DimsHeight = 2, DimsWidth = 2, DimsLength = 2 },
                new FedExPackageEntity { DimsHeight = 2, DimsWidth = 2, DimsLength = 2 }
            });

            return shipmentEntity;
        }

        public static ShipmentEntity SetupRequestShipmentEntity()
        {
            ShipmentEntity shipment = SetupBaseShipmentEntity();

            shipment.ShipCity = "AURORA";
            shipment.ShipCompany = "RTC";
            shipment.ShipCountryCode = "US";
            shipment.ShipEmail = "";
            shipment.ShipFirstName = "James";
            shipment.ShipLastName = "Weston";
            shipment.ShipMiddleName = "";
            shipment.ShipPhone = "9012633035";
            shipment.ShipPostalCode = "44202";
            shipment.ShipStateProvCode = "OH";
            shipment.ShipStreet1 = "1751 THOMPSON ST";
            shipment.ShipStreet2 = "Suite 200";
            shipment.ShipStreet3 = "Next to the furnace";
            shipment.ShipNameParseStatus = 0;

            shipment.OriginCity = "NORTH LAS VEGAS";
            shipment.OriginCompany = "CSP testing";
            shipment.OriginCountryCode = "US";
            shipment.OriginEmail = "";
            shipment.OriginFirstName = "323196";
            shipment.OriginLastName = "";
            shipment.OriginMiddleName = "";
            shipment.OriginPhone = "9012633035";
            shipment.OriginPostalCode = "89030";
            shipment.OriginStateProvCode = "NV";
            shipment.OriginStreet1 = "78 Fedex parkway";
            shipment.OriginStreet2 = "Suite 2000";
            shipment.OriginStreet3 = "Fruit cube";
            shipment.OriginNameParseStatus = 0;

            shipment.ShipDate = DateTime.Now;

            shipment.FedEx.PackagingType = (int) FedExPackagingType.Custom;
            shipment.FedEx.WeightUnitType = (int) WeightUnitOfMeasure.Pounds;
            shipment.FedEx.LinearUnitType = (int) FedExLinearUnitOfMeasure.IN;

            shipment.FedEx.Packages[0] = new FedExPackageEntity(1)
            {
                DimsLength = 2,
                DimsWidth = 4,
                DimsHeight = 8,
                // total weight should be 48
                DimsWeight = 16,
                DimsAddWeight = true,
                Weight = 32,
                DeclaredValue = 64
            };

            shipment.FedEx.Packages[1] = new FedExPackageEntity(1)
            {
                DimsLength = 3,
                DimsWidth = 6,
                DimsHeight = 12,
                // total weight should be 72
                DimsWeight = 24,
                DimsAddWeight = true,
                Weight = 48,
                DeclaredValue = 96
            };

            shipment.FedEx.Shipment = shipment;

            return shipment;
        }
    }
}