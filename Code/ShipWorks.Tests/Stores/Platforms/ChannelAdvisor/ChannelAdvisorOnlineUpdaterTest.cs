﻿using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.ChannelAdvisor.OnlineUpdating;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Summary description for ChannelAdvisorOnlineUpdaterTest
    /// </summary>
    public class ChannelAdvisorOnlineUpdaterTest
    {
        private readonly ChannelAdvisorOrderEntity orderEntity;
        private readonly ChannelAdvisorStoreEntity storeEntity;
        private readonly FedExShipmentEntity fedExEntity;
        private readonly UpsShipmentEntity upsEntity;
        private ShipmentEntity shipmentEntity;
        private readonly PostalShipmentEntity postalShipmentEntity;
        private readonly EndiciaShipmentEntity endiciaShipmentEntity;
        private readonly UspsShipmentEntity uspsShipmentEntity;
        private readonly OtherShipmentEntity otherShipmentEntity;
        private AmazonSFPShipmentEntity amazonShipmentEntity;
        private readonly ChannelAdvisorOnlineUpdater testObject;


        public ChannelAdvisorOnlineUpdaterTest()
        {

            var mock = AutoMock.GetLoose();
            testObject = mock.Create<ChannelAdvisorOnlineUpdater>();
            orderEntity = new ChannelAdvisorOrderEntity { OrderNumber = 123456, MarketplaceNames = string.Empty };
            storeEntity = new ChannelAdvisorStoreEntity();
            storeEntity.ConsolidatorAsUsps = false;
            fedExEntity = new FedExShipmentEntity { Service = (int) FedExServiceType.FedExGround };
            upsEntity = new UpsShipmentEntity { Service = (int) UpsServiceType.UpsGround, UspsTrackingNumber = "mi tracking #" };
            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow, ShipmentType = (int) ShipmentTypeCode.FedEx, FedEx = fedExEntity };
            uspsShipmentEntity = new UspsShipmentEntity();
            postalShipmentEntity = new PostalShipmentEntity { Service = (int) PostalServiceType.FirstClass };
            otherShipmentEntity = new OtherShipmentEntity { Carrier = "Some other carrier", Service = "Fast Ground" };
            endiciaShipmentEntity = new EndiciaShipmentEntity();
            amazonShipmentEntity = new AmazonSFPShipmentEntity { ShippingServiceName = "UPS Ground", CarrierName = "UPS" };
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsConsolidator_WhenEndiciaAndConsolidatorServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int) PostalServiceType.ConsolidatorDomestic;

            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("CONSOLIDATOR", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsGlobalMail_WhenEndiciaAndDhlServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("DHL", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsGlobalMail_WhenUspsAndDhlServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("DHL", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsFirstClass_WhenEndiciaAndFirstClassServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int) PostalServiceType.FirstClass;

            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("FIRSTCLASS", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsFirstClass_WhenUspsAndFirstClassServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int) PostalServiceType.FirstClass;

            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("FIRSTCLASS", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsInternationalFirst_WhenUspsAndGlobalPostEconomyServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int) PostalServiceType.GlobalPostEconomyIntl;

            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("IFIRSTCLASS", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsInternationalFirst_WhenUspsAndGlobalPostSmartSaverEconomyServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int) PostalServiceType.GlobalPostSmartSaverEconomyIntl;

            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("IFIRSTCLASS", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsIPriority_WhenUspsAndGlobalPostPriorityServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int) PostalServiceType.GlobalPostStandardIntl;

            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("IPRIORITY", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsIPrioirty_WhenUspsAndGlobalPostSmartSaverPriorityServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int) PostalServiceType.GlobalPostSmartSaverStandardIntl;

            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("IPRIORITY", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsMi_WhenUpsAndMiServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.UpsOnLineTools);

            upsEntity.Service = (int) UpsServiceType.UpsMailInnovationsFirstClass;

            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("MI", code);

            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.UpsWorldShip;
            code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("MI", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsGround_WhenUpsAndGroundServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.UpsOnLineTools);

            upsEntity.Service = (int) UpsServiceType.UpsGround;

            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("GROUND", code);

            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.UpsWorldShip;
            code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("GROUND", code);
        }

        [Fact]
        public void GetCarrierCode_ReturnsConsolidator_WhenEndiciaAndConsolidatorServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int) PostalServiceType.ConsolidatorDomestic;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal("Consolidator", code);
        }

        [Fact]
        public void GetCarrierCode_ReturnsDHL_WhenEndiciaAndDhlServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal("DHL Global Mail", code);
        }

        [Fact]
        public void GetCarrierCode_ReturnsDHL_WhenUspsAndDhlServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal("DHL Global Mail", code);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenEndiciaAndFirstClassServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int) PostalServiceType.FirstClass;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal("USPS", code);
        }

        [Fact]
        public void GetCarrierCode_ReturnsDhl_WhenEndiciaAndDhlServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal("DHL Global Mail", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsDhlGlobalMail_WhenDhlEcommerce()
        {
            SetupShipmentDefaults(ShipmentTypeCode.DhlEcommerce);

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal("DHL Global Mail", code);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenUspsAndUspsServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int) PostalServiceType.FirstClass;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal("USPS", code);
        }

        [Fact]
        public void GetCarrierCode_ReturnsDhl_WhenUspsAndDhlServiceUsed()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal("DHL Global Mail", code);
        }

        [Fact]
        public void GetShipmentClassCode_ReturnsGlobalMail_WhenUspsAndDhlServiceUsedAndEbayOrder()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            var caOrder = shipmentEntity.Order as ChannelAdvisorOrderEntity;
            caOrder.MarketplaceNames = "ebay";


            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal("Global Mail", code);
        }

        [Theory]
        [InlineData("FedEx", "FedEx Priority Overnight®", "PRIORITY")]
        [InlineData("FedEx", "FedEx Standard Overnight®", "OVERNIGHT")]
        [InlineData("FedEx", "FedEx 2Day®A.M.", "2DAY")]
        [InlineData("FedEx", "FedEx 2Day®", "2DAY")]
        [InlineData("FedEx", "FedEx Express Saver®", "EXPSAVER")]
        [InlineData("FedEx", "FedEx Home Delivery®", "GROUND")]
        [InlineData("FedEx", "FedEx  Home Delivery®", "GROUND")]
        [InlineData("FedEx", "FedEx Ground®", "GROUND")]
        [InlineData("USPS", "USPS First Class", "FIRSTCLASS")]
        [InlineData("USPS", "USPS Priority Mail", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Flat Rate Box", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Small Flat Rate Box", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Large Flat Rate Box", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Flat Rate Envelope", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Express", "EXPRESS")]
        [InlineData("USPS", "USPS Priority Mail Express Flat Rate Envelope", "EXPRESS")]
        [InlineData("USPS", "USPS Parcel Select", "PARCELSELECT")]
        [InlineData("USPS", "USPS Bound Printed Matter", "BOUNDPRINTEDMATTER")]
        [InlineData("USPS", "USPS Express Mail", "EXPRESS")]
        [InlineData("USPS", "USPS Express Mail Flat Rate Envelope", "EXPRESS")]
        [InlineData("USPS", "USPS Express Mail Legal Flat Rate Envelope", "EXPRESS")]
        [InlineData("USPS", "USPS Media Mail", "MEDIA")]
        [InlineData("USPS", "USPS Priority Mail Legal Flat Rate Envelope", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Padded Flat Rate Envelope", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Regional Rate Box A", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Regional Rate Box B", "PRIORITY")]
        [InlineData("USPS", "USPS Priority Mail Regional Rate Box C", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS First Class", "FIRSTCLASS")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Flat Rate Box", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Small Flat Rate Box", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Large Flat Rate Box", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Flat Rate Envelope", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Express", "EXPRESS")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Express Flat Rate Envelope", "EXPRESS")]
        [InlineData("STAMPS_DOT_COM", "USPS Parcel Select", "PARCELSELECT")]
        [InlineData("STAMPS_DOT_COM", "USPS Bound Printed Matter", "BOUNDPRINTEDMATTER")]
        [InlineData("STAMPS_DOT_COM", "USPS Express Mail", "EXPRESS")]
        [InlineData("STAMPS_DOT_COM", "USPS Express Mail Flat Rate Envelope", "EXPRESS")]
        [InlineData("STAMPS_DOT_COM", "USPS Express Mail Legal Flat Rate Envelope", "EXPRESS")]
        [InlineData("STAMPS_DOT_COM", "USPS Media Mail", "MEDIA")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Legal Flat Rate Envelope", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Padded Flat Rate Envelope", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Regional Rate Box A", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Regional Rate Box B", "PRIORITY")]
        [InlineData("STAMPS_DOT_COM", "USPS Priority Mail Regional Rate Box C", "PRIORITY")]
        [InlineData("UPS", "UPS Ground", "GROUND")]
        [InlineData("UPS", "UPS Next Day Air", "NEXTDAY")]
        [InlineData("UPS", "UPS Next Day Air Saver", "NDAS")]
        [InlineData("UPS", "UPS 2nd Day Air", "2DAY")]
        [InlineData("UPS", "UPS 3 Day Select", "3DS")]
        [InlineData("unknowncarrier", "unknownservice", "unknownservice")]
        [InlineData("unknowncarrier", "UnknownCarrier unknownservice", "unknownservice")]
        public void GetShipmentClassCode_ReturnsCorrectValue_WhenAmazonShipment(string carrierName, string shippingServiceName, string expectedValue)
        {
            amazonShipmentEntity.CarrierName = carrierName;
            amazonShipmentEntity.ShippingServiceName = shippingServiceName;

            SetupShipmentDefaults(ShipmentTypeCode.AmazonSFP);

            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal(expectedValue, code);
        }

        [Theory]
        [InlineData("FedEx", "FEDEX")]
        [InlineData("FEDEX", "FEDEX")]
        [InlineData("UPS", "UPS")]
        [InlineData("Ups", "UPS")]
        [InlineData("USPS", "USPS")]
        [InlineData("Usps", "USPS")]
        [InlineData("Stamps_Dot_Com", "USPS")]
        [InlineData("STAMPS_DOT_COM", "USPS")]
        [InlineData("asdfasdfasdf", "asdfasdfasdf")]
        public void GetCarrierCode_ReturnsCorrectValue_WhenAmazonShipment(string carrierName, string expectedValue)
        {
            amazonShipmentEntity.CarrierName = carrierName;

            SetupShipmentDefaults(ShipmentTypeCode.AmazonSFP);

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity);

            Assert.Equal(expectedValue, code);
        }

        [Fact]
        public void GetCarrierCode_Throws_WhenAmazonShipmentIsnull()
        {
            amazonShipmentEntity = null;

            SetupShipmentDefaults(ShipmentTypeCode.AmazonSFP);

            Assert.Throws<ArgumentNullException>(
                () => ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity));
        }

        [Fact]
        public void GetShipmentClassCode_Throws_WhenAmazonShipmentIsnull()
        {
            amazonShipmentEntity = null;

            SetupShipmentDefaults(ShipmentTypeCode.AmazonSFP);

            Assert.Throws<ArgumentNullException>(
                () => ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity, storeEntity));
        }

        [Theory]
        [InlineData(FedExServiceType.FedExGround, "GROUND")]
        [InlineData(FedExServiceType.GroundHomeDelivery, "GROUND")]
        [InlineData(FedExServiceType.FedExInternationalGround, "GROUND")]
        [InlineData(FedExServiceType.FedEx2Day, "2Day")]
        [InlineData(FedExServiceType.OneRate2Day, "2Day")]
        [InlineData(FedExServiceType.PriorityOvernight, "PRIORITY")]
        [InlineData(FedExServiceType.OneRatePriorityOvernight, "PRIORITY")]
        [InlineData(FedExServiceType.FirstOvernight, "1STOVERNIGHT")]
        [InlineData(FedExServiceType.OneRateFirstOvernight, "1STOVERNIGHT")]
        [InlineData(FedExServiceType.StandardOvernight, "OVERNIGHT")]
        [InlineData(FedExServiceType.OneRateStandardOvernight, "OVERNIGHT")]
        [InlineData(FedExServiceType.FedExExpressSaver, "EXPSAVER")]
        [InlineData(FedExServiceType.OneRateExpressSaver, "EXPSAVER")]
        [InlineData(FedExServiceType.FirstFreight, "OVERNIGHTFREIGHT")]
        [InlineData(FedExServiceType.FedEx1DayFreight, "OVERNIGHTFREIGHT")]
        [InlineData(FedExServiceType.FedEx2DayFreight, "2DAYFREIGHT")]
        [InlineData(FedExServiceType.InternationalPriority, "INTLPRIORITY")]
        [InlineData(FedExServiceType.InternationalPriorityExpress, "INTLPRIORITY")]
        [InlineData(FedExServiceType.InternationalEconomy, "INTLECONOMY")]
        [InlineData(FedExServiceType.InternationalFirst, "INTLFIRST")]
        [InlineData(FedExServiceType.InternationalPriorityFreight, "Internaional Priority Freight")]
        [InlineData(FedExServiceType.InternationalEconomyFreight, "International Economy Freight")]
        [InlineData(FedExServiceType.SmartPost, "SmartPost")]
        [InlineData(FedExServiceType.FedExFreightEconomy, "Freight Economy")]
        [InlineData(FedExServiceType.FedExFreightPriority, "Freight Priority")]
        public void GetShipmentClassCode_ReturnsCorrectValue_ForFedExServices(FedExServiceType service, string expected)
        {
            SetupShipmentDefaults(ShipmentTypeCode.FedEx);
            fedExEntity.Service = (int) service;

            string code = testObject.GetShipmentClassCode(shipmentEntity, storeEntity);

            Assert.Equal(expected, code);
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
                ShipmentType = (int) shipmentTypeCode,
                Postal = postalShipmentEntity,
                Other = otherShipmentEntity,
                FedEx = fedExEntity,
                Ups = upsEntity,
                AmazonSFP = amazonShipmentEntity
            };
        }
    }
}
