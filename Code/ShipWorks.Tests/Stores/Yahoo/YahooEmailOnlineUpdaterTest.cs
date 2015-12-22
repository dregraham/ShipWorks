using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration;

namespace ShipWorks.Tests.Stores.Yahoo
{
    /// <summary>
    /// Summary description for YahooOnlineUpdaterTest
    /// </summary>
    public class YahooEmailOnlineUpdaterTest
    {
        private ShipmentEntity shipmentEntity;
        private OrderEntity orderEntity;
        private OtherShipmentEntity otherShipmentEntity;
        private PostalShipmentEntity postalShipmentEntity;
        private UpsShipmentEntity upsEntity;

        public YahooEmailOnlineUpdaterTest()
        {
            orderEntity = new NeweggOrderEntity { OrderNumber = 123456 };
            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow };
            postalShipmentEntity = new PostalShipmentEntity { Service = (int)PostalServiceType.FirstClass };
            otherShipmentEntity = new OtherShipmentEntity { Carrier = "Some other carrier", Service = "Fast Ground" };
            upsEntity = new UpsShipmentEntity { Service = (int)UpsServiceType.UpsGround, UspsTrackingNumber = "mi tracking #" };
        }

        [Fact]
        public void GetCarrierCode_ReturnsDhl_WhenEndiciaAndDhlServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Endicia;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = YahooEmailOnlineUpdater.GetShipperString(shipmentEntity);

            Assert.Equal("Dhl", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsDhl_WhenUspsAndDhlServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = YahooEmailOnlineUpdater.GetShipperString(shipmentEntity);

            Assert.Equal("Dhl", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsConsolidator_WhenEndiciaAndConsolidatorServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.ConsolidatorDomestic;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = YahooEmailOnlineUpdater.GetShipperString(shipmentEntity);

            Assert.Equal("Consolidator", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenEndiciaAndFirstClassServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = YahooEmailOnlineUpdater.GetShipperString(shipmentEntity);

            Assert.Equal("Usps", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenUspsAndFirstClassServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = YahooEmailOnlineUpdater.GetShipperString(shipmentEntity);

            Assert.Equal("Usps", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenOtherAndNotUpsFedExOrDhl_Test()
        {
            otherShipmentEntity.Carrier = "something else";
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            shipmentEntity.Other = otherShipmentEntity;

            string carrierCode = YahooEmailOnlineUpdater.GetShipperString(shipmentEntity);

            Assert.Equal("Usps", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUps_WhenOtherAndUps_Test()
        {
            otherShipmentEntity.Carrier = "Ups";
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            shipmentEntity.Other = otherShipmentEntity;

            string carrierCode = YahooEmailOnlineUpdater.GetShipperString(shipmentEntity);

            Assert.Equal("Ups", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsFedex_WhenOtherAndFedEx_Test()
        {
            otherShipmentEntity.Carrier = "FedEx";
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            shipmentEntity.Other = otherShipmentEntity;

            string carrierCode = YahooEmailOnlineUpdater.GetShipperString(shipmentEntity);

            Assert.Equal("Fedex", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsDhl_WhenOtherAndDhl_Test()
        {
            otherShipmentEntity.Carrier = "Dhl";
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            shipmentEntity.Other = otherShipmentEntity;

            string carrierCode = YahooEmailOnlineUpdater.GetShipperString(shipmentEntity);

            Assert.Equal("DHL", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenUpsAndMi_Test()
        {
            upsEntity.Service = (int) UpsServiceType.UpsMailInnovationsFirstClass;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.UpsOnLineTools;
            shipmentEntity.Ups = upsEntity;

            string carrierCode = YahooEmailOnlineUpdater.GetShipperString(shipmentEntity);

            Assert.Equal("Usps", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUps_WhenUpsAndGround_Test()
        {
            upsEntity.Service = (int)UpsServiceType.UpsGround;
            upsEntity.UspsTrackingNumber = "usps tracking num";
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.UpsOnLineTools;
            shipmentEntity.Ups = upsEntity;

            string carrierCode = YahooEmailOnlineUpdater.GetShipperString(shipmentEntity);

            Assert.Equal("Ups", carrierCode);

            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.UpsWorldShip;
            carrierCode = YahooEmailOnlineUpdater.GetShipperString(shipmentEntity);

            Assert.Equal("Ups", carrierCode);
        }
    }
}
