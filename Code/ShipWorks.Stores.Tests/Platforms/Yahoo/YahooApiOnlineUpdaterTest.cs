using System;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Yahoo
{
    public class YahooApiOnlineUpdaterTest
    {
        private readonly ShipmentEntity shipmentEntity;
        private readonly OtherShipmentEntity otherShipmentEntity;
        private readonly PostalShipmentEntity postalShipmentEntity;
        private readonly UpsShipmentEntity upsEntity;
        private readonly YahooApiOnlineUpdater testObject;
        readonly Mock<YahooStoreEntity> yahooApiStore = new Mock<YahooStoreEntity>();
        readonly Mock<IShippingManager> shippingManager = new Mock<IShippingManager>();

        public YahooApiOnlineUpdaterTest()
        {
            yahooApiStore.Setup(x => x.TypeCode).Returns(2);
            yahooApiStore.Setup(x => x.YahooStoreID).Returns("ysht-123456789");

            OrderEntity orderEntity = new YahooOrderEntity { OrderNumber = 123456 };
            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow };
            postalShipmentEntity = new PostalShipmentEntity { Service = (int)PostalServiceType.FirstClass };
            otherShipmentEntity = new OtherShipmentEntity { Carrier = "Some other carrier", Service = "Fast Ground" };
            upsEntity = new UpsShipmentEntity { Service = (int)UpsServiceType.UpsGround, UspsTrackingNumber = "mi tracking #" };


            shippingManager.Setup(x => x.EnsureShipmentLoaded(shipmentEntity));

            testObject = new YahooApiOnlineUpdater(LogManager.GetLogger(typeof(YahooApiOnlineUpdater)), new YahooApiWebClient(yahooApiStore.Object), shippingManager.Object);
        }

        [Fact]
        public void GetCarrierCode_ReturnsDhl_WhenEndiciaAndDhlServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = testObject.GetCarrierCode(shipmentEntity);

            Assert.Equal("dhl", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsDhl_WhenUspsAndDhlServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = testObject.GetCarrierCode(shipmentEntity);

            Assert.Equal("dhl", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenEndiciaAndConsolidatorServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.ConsolidatorDomestic;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = testObject.GetCarrierCode(shipmentEntity);

            Assert.Equal("usps", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenEndiciaAndFirstClassServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = testObject.GetCarrierCode(shipmentEntity);

            Assert.Equal("usps", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenUspsAndFirstClassServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = testObject.GetCarrierCode(shipmentEntity);

            Assert.Equal("usps", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsEmptyString_WhenOtherAndNotSupportedCarrier_Test()
        {
            otherShipmentEntity.Carrier = "something else";
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            shipmentEntity.Other = otherShipmentEntity;

            string carrierCode = testObject.GetCarrierCode(shipmentEntity);

            Assert.Equal("", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUps_WhenOtherAndUps_Test()
        {
            otherShipmentEntity.Carrier = "Ups";
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            shipmentEntity.Other = otherShipmentEntity;

            string carrierCode = testObject.GetCarrierCode(shipmentEntity);

            Assert.Equal("ups", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsFedex_WhenOtherAndFedEx_Test()
        {
            otherShipmentEntity.Carrier = "fedex";
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            shipmentEntity.Other = otherShipmentEntity;

            string carrierCode = testObject.GetCarrierCode(shipmentEntity);

            Assert.Equal("fedex", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsDhl_WhenOtherAndDhl_Test()
        {
            otherShipmentEntity.Carrier = "dhl";
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            shipmentEntity.Other = otherShipmentEntity;

            string carrierCode = testObject.GetCarrierCode(shipmentEntity);

            Assert.Equal("dhl", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenUpsAndMi_Test()
        {
            upsEntity.Service = (int)UpsServiceType.UpsMailInnovationsFirstClass;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.UpsOnLineTools;
            shipmentEntity.Ups = upsEntity;

            string carrierCode = testObject.GetCarrierCode(shipmentEntity);

            Assert.Equal("usps", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUps_WhenUpsAndGround_Test()
        {
            upsEntity.Service = (int)UpsServiceType.UpsGround;
            upsEntity.UspsTrackingNumber = "usps tracking num";
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.UpsOnLineTools;
            shipmentEntity.Ups = upsEntity;

            string carrierCode = testObject.GetCarrierCode(shipmentEntity);

            Assert.Equal("ups", carrierCode);

            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.UpsWorldShip;
            carrierCode = testObject.GetCarrierCode(shipmentEntity);

            Assert.Equal("ups", carrierCode);
        }
    }
}
