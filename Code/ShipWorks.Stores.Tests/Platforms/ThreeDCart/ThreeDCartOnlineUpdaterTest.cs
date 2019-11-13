﻿using System;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ThreeDCart
{
    public class ThreeDCartOnlineUpdaterTest
    {
        private readonly ShipmentEntity shipment;
        private readonly OtherShipmentEntity otherShipment;
        private readonly PostalShipmentEntity postalShipment;
        private readonly FedExShipmentEntity fedExShipment;
        private readonly OnTracShipmentEntity onTracShipment;
        private readonly UpsShipmentEntity upsShipment;
        private readonly IParcelShipmentEntity iParcelShipment;
        private readonly ThreeDCartRestOnlineUpdater testObject;
        private readonly AutoMock mock;

        public ThreeDCartOnlineUpdaterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            var store = new ThreeDCartStoreEntity { RestUser = true, TypeCode = (int) StoreTypeCode.ThreeDCart };
            
            OrderEntity orderEntity = new ThreeDCartOrderEntity() { ThreeDCartOrderID = 100 };
            shipment = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow };
            postalShipment = new PostalShipmentEntity { Service = (int) PostalServiceType.FirstClass };
            otherShipment = new OtherShipmentEntity { Carrier = "Some other carrier", Service = "Fast Ground" };
            upsShipment = new UpsShipmentEntity { Service = (int) UpsServiceType.UpsGround, UspsTrackingNumber = "mi tracking #" };
            fedExShipment = new FedExShipmentEntity { Service = (int) FedExServiceType.FedEx2Day };
            onTracShipment = new OnTracShipmentEntity { Service = (int) OnTracServiceType.Ground };
            iParcelShipment = new IParcelShipmentEntity { Service = (int) iParcelServiceType.Immediate };

            
            testObject = mock.Create<ThreeDCartRestOnlineUpdater>(TypedParameter.From(store));
        }

        [Fact]
        public void GetShipmentMethod_ReturnsDhl_WhenEndiciaAndDhlServiceUsed()
        {
            postalShipment.Service = (int) PostalServiceType.DhlParcelGround;
            shipment.ShipmentType = (int) ShipmentTypeCode.Endicia;
            shipment.Postal = postalShipment;

            string actualShippingMethod = testObject.GetShipmentMethod(shipment);
            string expectedShippingMethod =
                $"{EnumHelper.GetDescription(PostalServiceType.DhlParcelGround)}";

            Assert.Equal(expectedShippingMethod, actualShippingMethod);
        }

        [Fact]
        public void GetShipmentMethod_ReturnsDhl_WhenUspsAndDhlServiceUsed()
        {
            postalShipment.Service = (int) PostalServiceType.DhlParcelGround;
            shipment.ShipmentType = (int) ShipmentTypeCode.Usps;
            shipment.Postal = postalShipment;

            string actualShippingMethod = testObject.GetShipmentMethod(shipment);
            string expectedShippingMethod =
                $"{EnumHelper.GetDescription(PostalServiceType.DhlParcelGround)}";

            Assert.Equal(expectedShippingMethod, actualShippingMethod);
        }

        [Fact]
        public void GetShipmentMethod_ReturnsUsps_WhenEndiciaAndConsolidatorServiceUsed()
        {
            postalShipment.Service = (int) PostalServiceType.ConsolidatorDomestic;
            shipment.ShipmentType = (int) ShipmentTypeCode.Endicia;
            shipment.Postal = postalShipment;

            string actualShippingMethod = testObject.GetShipmentMethod(shipment);
            string expectedShippingMethod =
                $"USPS - {EnumHelper.GetDescription(PostalServiceType.ConsolidatorDomestic)}";

            Assert.Equal(expectedShippingMethod, actualShippingMethod);
        }

        [Fact]
        public void GetShipmentMethod_ReturnsUsps_WhenEndiciaAndFirstClassServiceUsed()
        {
            postalShipment.Service = (int) PostalServiceType.FirstClass;
            shipment.ShipmentType = (int) ShipmentTypeCode.Endicia;
            shipment.Postal = postalShipment;

            string actualShippingMethod = testObject.GetShipmentMethod(shipment);
            string expectedShippingMethod =
                $"USPS - {EnumHelper.GetDescription(PostalServiceType.FirstClass)}";

            Assert.Equal(expectedShippingMethod, actualShippingMethod);
        }

        [Fact]
        public void GetShipmentMethod_ReturnsFedEx_WhenFedExUsed()
        {
            fedExShipment.Service = (int) FedExServiceType.FedEx2Day;
            shipment.ShipmentType = (int) ShipmentTypeCode.FedEx;
            shipment.FedEx = fedExShipment;

            string actualShippingMethod = testObject.GetShipmentMethod(shipment);
            string expectedShippingMethod = "FedEx 2Day";

            Assert.Equal(expectedShippingMethod, actualShippingMethod);
        }

        [Fact]
        public void GetShipmentMethod_ReturnsUpsMI_WhenUpsMailInnovationsUsed()
        {
            upsShipment.Service = (int) UpsServiceType.UpsMailInnovationsExpedited;
            shipment.ShipmentType = (int) ShipmentTypeCode.UpsOnLineTools;
            shipment.Ups = upsShipment;

            string actualShippingMethod = testObject.GetShipmentMethod(shipment);
            string expectedShippingMethod = "UPS MI - UPS Expedited Mail Innovations";

            Assert.Equal(expectedShippingMethod, actualShippingMethod);
        }

        [Fact]
        public void GetShipmentMethod_ReturnsUps_WhenUpsUsed()
        {
            upsShipment.Service = (int) UpsServiceType.Ups2DayAir;
            shipment.ShipmentType = (int) ShipmentTypeCode.UpsOnLineTools;
            shipment.Ups = upsShipment;

            string actualShippingMethod = testObject.GetShipmentMethod(shipment);
            string expectedShippingMethod = "UPS 2nd Day Air";

            Assert.Equal(expectedShippingMethod, actualShippingMethod);
        }

        [Fact]
        public void GetShipmentMethod_ReturnsOnTrac_WhenOnTracUsed()
        {
            onTracShipment.Service = (int) OnTracServiceType.Ground;
            shipment.ShipmentType = (int) ShipmentTypeCode.OnTrac;
            shipment.OnTrac = onTracShipment;

            string actualShippingMethod = testObject.GetShipmentMethod(shipment);
            string expectedShippingMethod =
                $"OnTrac - {EnumHelper.GetDescription(OnTracServiceType.Ground)}";

            Assert.Equal(expectedShippingMethod, actualShippingMethod);
        }

        [Fact]
        public void GetShipmentMethod_ReturnsIParcel_WhenIParcelUsed()
        {
            iParcelShipment.Service = (int) iParcelServiceType.Immediate;
            shipment.ShipmentType = (int) ShipmentTypeCode.iParcel;
            shipment.IParcel = iParcelShipment;

            string actualShippingMethod = testObject.GetShipmentMethod(shipment);
            string expectedShippingMethod =
                $"iParcel - {EnumHelper.GetDescription(iParcelServiceType.Immediate)}";

            Assert.Equal(expectedShippingMethod, actualShippingMethod);
        }

        [Fact]
        public void GetShipmentMethod_ReturnsOtherCarrierAndService_WhenOtherCarrierUsed()
        {
            otherShipment.Carrier = "Carrier";
            otherShipment.Service = "Service";
            shipment.ShipmentType = (int) ShipmentTypeCode.Other;
            shipment.Other = otherShipment;

            string actualShippingMethod = testObject.GetShipmentMethod(shipment);
            string expectedShippingMethod = "Carrier - Service";

            Assert.Equal(expectedShippingMethod, actualShippingMethod);
        }

        [Fact]
        public void GetShipmentMethod_ReturnsEmptyString_WhenCarrierIsNotFound()
        {
            shipment.ShipmentType = (int) ShipmentTypeCode.None;

            string actualShippingMethod = testObject.GetShipmentMethod(shipment);
            string expectedShippingMethod = string.Empty;

            Assert.Equal(expectedShippingMethod, actualShippingMethod);
        }

        [Fact]
        public void GetShipmentMethod_RemovesNonAlphaNumericChars_ShippingMethodHasNonAlphaChars()
        {
            otherShipment.Carrier = "Carrier";
            otherShipment.Service = "FedEx One Rate® (First Overnight)";
            shipment.ShipmentType = (int) ShipmentTypeCode.Other;
            shipment.Other = otherShipment;

            string actualShippingMethod = testObject.GetShipmentMethod(shipment);
            string expectedShippingMethod = "Carrier - FedEx One Rate (First Overnight)";

            Assert.Equal(expectedShippingMethod, actualShippingMethod);
        }
    }
}