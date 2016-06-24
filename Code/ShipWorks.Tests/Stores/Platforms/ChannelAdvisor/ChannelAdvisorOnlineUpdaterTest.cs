using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.ChannelAdvisor;

namespace ShipWorks.Tests.Stores.ChannelAdvisor
{
    /// <summary>
    /// Summary description for ChannelAdvisorOnlineUpdaterTest
    /// </summary>
    public class ChannelAdvisorOnlineUpdaterTest
    {
        private ChannelAdvisorOrderEntity orderEntity;
        private ChannelAdvisorStoreEntity storeEntity;
        private FedExShipmentEntity fedExEntity;
        private UpsShipmentEntity upsEntity;
        private ShipmentEntity shipmentEntity;
        private PostalShipmentEntity postalShipmentEntity;
        private EndiciaShipmentEntity endiciaShipmentEntity;
        private UspsShipmentEntity uspsShipmentEntity;
        private OtherShipmentEntity otherShipmentEntity;
        private AmazonShipmentEntity amazonShipmentEntity;
        private IParcelShipmentEntity iparcelShipmentEntity;
        private OnTracShipmentEntity ontracShipmentEntity;

        public ChannelAdvisorOnlineUpdaterTest()
        {
            orderEntity = new ChannelAdvisorOrderEntity { OrderNumber = 123456};
            storeEntity = new ChannelAdvisorStoreEntity();
            storeEntity.ConsolidatorAsUsps = false;
            fedExEntity = new FedExShipmentEntity { Service = (int)FedExServiceType.FedExGround };
            upsEntity = new UpsShipmentEntity { Service = (int)UpsServiceType.UpsGround, UspsTrackingNumber = "mi tracking #" };
            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow, ShipmentType = (int)ShipmentTypeCode.FedEx, FedEx = fedExEntity };
            uspsShipmentEntity = new UspsShipmentEntity();
            postalShipmentEntity = new PostalShipmentEntity { Service = (int)PostalServiceType.FirstClass };
            otherShipmentEntity = new OtherShipmentEntity { Carrier = "Some other carrier", Service = "Fast Ground" };
            endiciaShipmentEntity = new EndiciaShipmentEntity();
            iparcelShipmentEntity = new IParcelShipmentEntity {Service = (int) iParcelServiceType.Saver };
            ontracShipmentEntity = new OnTracShipmentEntity {Service = (int) OnTracServiceType.Ground };
            amazonShipmentEntity = new AmazonShipmentEntity { ShippingServiceName = "UPS Ground", CarrierName = "UPS" };
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsConsolidator_WhenEndiciaAndConsolidatorServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int)PostalServiceType.ConsolidatorDomestic;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("CONSOLIDATOR", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsGlobalMail_WhenEndiciaAndDhlServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("Global Mail", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsGlobalMail_WhenUspsAndDhlServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("Global Mail", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsFirstClass_WhenEndiciaAndFirstClassServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("FIRSTCLASS", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsFirstClass_WhenUspsAndFirstClassServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("FIRSTCLASS", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsMi_WhenUpsAndMiServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.UpsOnLineTools);

            upsEntity.Service = (int) UpsServiceType.UpsMailInnovationsFirstClass;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("MI", code);

            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.UpsWorldShip;
            code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("MI", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsGround_WhenUpsAndGroundServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.UpsOnLineTools);

            upsEntity.Service = (int)UpsServiceType.UpsGround;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("GROUND", code);

            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.UpsWorldShip;
            code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("GROUND", code);
        }

        [Fact]
        public void GetCarrierCode_ReturnsConsolidator_WhenEndiciaAndConsolidatorServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int)PostalServiceType.ConsolidatorDomestic;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal("Consolidator", code);
        }

        [Fact]
        public void GetCarrierCode_ReturnsDHL_WhenEndiciaAndDhlServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal("DHL", code);
        }

        [Fact]
        public void GetCarrierCode_ReturnsDHL_WhenUspsAndDhlServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal("DHL", code);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenEndiciaAndFirstClassServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal("USPS", code);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenUspsAndDhlServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal("USPS", code);
        }

        [Theory]
        [InlineData("FedEx", "FedEx Priority Overnight®", "PRIORITY")]
        [InlineData("FedEx", "FedEx Standard Overnight®", "OVERNIGHT")]
        [InlineData("FedEx", "FedEx 2Day®A.M.", "2DAY")]
        [InlineData("FedEx", "FedEx 2Day®", "2DAY")]
        [InlineData("FedEx", "FedEx Express Saver®", "EXPSAVER")]
        [InlineData("FedEx", "FedEx Home Delivery®", "GROUND")]
        [InlineData("USPS", "USPS First Class", "FIRSTCLASS")]
        [InlineData("USPS", "USPS Priority Mail", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Flat Rate Box", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Small Flat Rate Box", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Large Flat Rate Box", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Flat Rate Envelope", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Express", "EXPRESS")]
        [InlineData("USPS", "USPS Priority Mail Express Flat Rate Envelope", "EXPRESS")]
        [InlineData("USPS", "USPS Parcel Select", "PARCELSELECT")]
        [InlineData("STAMPS_DOT_COM", "USPS First Class", "FIRSTCLASS")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Flat Rate Box", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Small Flat Rate Box", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Large Flat Rate Box", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Flat Rate Envelope", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Express", "EXPRESS")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Express Flat Rate Envelope", "EXPRESS")]
        [InlineData("STAMPS_DOT_COM", "USPS Parcel Select", "PARCELSELECT")]
        [InlineData("UPS", "UPS Ground", "GROUND")]
        [InlineData("UPS", "UPS Next Day Air", "NEXTDAY")]
        [InlineData("UPS", "UPS Next Day Air Saver", "NDAS")]
        [InlineData("asdfasdfasdf", "asdfasdfasdf", "NONE")]
        public void GetShipmentClassCode_ReturnsCorrectValue_WhenAmazonShipment(string carrierName, string shippingServiceName, string expectedValue)
        {
            amazonShipmentEntity.CarrierName = carrierName;
            amazonShipmentEntity.ShippingServiceName = shippingServiceName;

            SetupShipmentDefaults(ShipmentTypeCode.Amazon);

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal(expectedValue, code);
        }

        [Theory]
        [InlineData("FedEx", "FEDEX")]
        [InlineData("FEDEX", "FEDEX")]
        [InlineData("UPS", "UPS")]
        [InlineData("Ups", "UPS")]
        [InlineData("USPS", "USPS")]
        [InlineData("Usps", "USPS")]
        [InlineData("asdfasdfasdf", "None")]
        public void GetCarrierCode_ReturnsCorrectValue_WhenAmazonShipment(string carrierName, string expectedValue)
        {
            amazonShipmentEntity.CarrierName = carrierName;

            SetupShipmentDefaults(ShipmentTypeCode.Amazon);

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal(expectedValue, code);
        }

        [Fact]
        public void GetCarrierCode_Throws_WhenAmazonShipmentIsnull()
        {
            amazonShipmentEntity = null;

            SetupShipmentDefaults(ShipmentTypeCode.Amazon);

            Assert.Throws<ArgumentNullException>(
                () => ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity));
        }
        [Fact]
        public void GetShipmentClassCode_Throws_WhenAmazonShipmentIsnull()
        {
            amazonShipmentEntity = null;

            SetupShipmentDefaults(ShipmentTypeCode.Amazon);

            Assert.Throws<ArgumentNullException>(
                () => ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity));
        }

        private void SetupShipmentDefaults(ShipmentTypeCode shipmentTypeCode)
        {
            postalShipmentEntity.Endicia = endiciaShipmentEntity;
            postalShipmentEntity.Usps = uspsShipmentEntity;

            shipmentEntity = new ShipmentEntity
            {
                Processed = true,
                Order = orderEntity,
                TrackingNumber = "ABCD1234",
                ShipDate = DateTime.UtcNow,
                ShipmentType = (int)shipmentTypeCode,
                Postal = postalShipmentEntity,
                Other = otherShipmentEntity,
                FedEx = fedExEntity,
                Ups = upsEntity,
                Amazon = amazonShipmentEntity
            };
        }
    }
}
