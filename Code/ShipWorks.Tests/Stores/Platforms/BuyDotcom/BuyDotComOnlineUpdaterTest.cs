﻿using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.BuyDotCom;
using ShipWorks.Stores.Platforms.BuyDotCom.Fulfillment;
using System;
using Xunit;

namespace ShipWorks.Tests.Stores.BuyDotcom
{
    /// <summary>
    /// Summary description for BuyDotComOnlineUpdaterTest
    /// </summary>
    public class BuyDotComOnlineUpdaterTest
    {
        private BuyDotComStoreEntity store;
        private BuyDotComOnlineUpdater updater;

        private OrderEntity orderEntity;
        private FedExShipmentEntity fedExEntity;
        private UpsShipmentEntity upsEntity;
        private ShipmentEntity shipmentEntity;
        private PostalShipmentEntity postalShipmentEntity;
        private EndiciaShipmentEntity endiciaShipmentEntity;
        private UspsShipmentEntity uspsShipmentEntity;
        private OtherShipmentEntity otherShipmentEntity;

        public BuyDotComOnlineUpdaterTest()
        {
            store = new BuyDotComStoreEntity();
            updater = new BuyDotComOnlineUpdater(store);

            orderEntity = new OrderEntity { OrderNumber = 123456 };
            fedExEntity = new FedExShipmentEntity { Service = (int)FedExServiceType.FedExGround };
            upsEntity = new UpsShipmentEntity { Service = (int)UpsServiceType.UpsGround, UspsTrackingNumber = "mi tracking #" };
            uspsShipmentEntity = new UspsShipmentEntity();
            postalShipmentEntity = new PostalShipmentEntity { Service = (int)PostalServiceType.FirstClass };
            otherShipmentEntity = new OtherShipmentEntity { Carrier = "Some other carrier", Service = "Fast Ground" };
            endiciaShipmentEntity = new EndiciaShipmentEntity();

            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow };
        }

        [Fact]
        public void GetTrackingType_ReturnsDhlGlobalMail_WhenEndiciaAndDhlServiceUsed()
        {
            postalShipmentEntity.Endicia = endiciaShipmentEntity;
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;

            shipmentEntity.Postal = postalShipmentEntity;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;

            BuyDotComTrackingType buyDotComTrackingType = updater.GetTrackingType(shipmentEntity);

            Assert.Equal(BuyDotComTrackingType.DHLGlobalMail, buyDotComTrackingType);
        }

        [Fact]
        public void GetTrackingType_ReturnsOther_WhenEndiciaAndConsolidatorServiceUsed()
        {
            postalShipmentEntity.Endicia = endiciaShipmentEntity;
            postalShipmentEntity.Service = (int)PostalServiceType.ConsolidatorDomestic;

            shipmentEntity.Postal = postalShipmentEntity;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;

            BuyDotComTrackingType buyDotComTrackingType = updater.GetTrackingType(shipmentEntity);

            Assert.Equal(BuyDotComTrackingType.Other, buyDotComTrackingType);
        }

        [Fact]
        public void GetTrackingType_ReturnsDhlGlobalMail_WhenUspsAndDhlServiceUsed()
        {
            postalShipmentEntity.Usps = uspsShipmentEntity;
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;

            shipmentEntity.Postal = postalShipmentEntity;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;

            BuyDotComTrackingType buyDotComTrackingType = updater.GetTrackingType(shipmentEntity);

            Assert.Equal(BuyDotComTrackingType.DHLGlobalMail, buyDotComTrackingType);
        }

        [Fact]
        public void GetTrackingType_ReturnsUsps_WhenUspsAndGlobalPostServiceUsed()
        {
            postalShipmentEntity.Usps = uspsShipmentEntity;
            postalShipmentEntity.Service = (int)PostalServiceType.GlobalPostPriority;

            shipmentEntity.Postal = postalShipmentEntity;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;

            BuyDotComTrackingType buyDotComTrackingType = updater.GetTrackingType(shipmentEntity);

            Assert.Equal(BuyDotComTrackingType.Usps, buyDotComTrackingType);
        }

        [Fact]
        public void GetTrackingType_ReturnsUsps_WhenUspsAndGlobalPostSmartSaverPriorityServiceUsed()
        {
            postalShipmentEntity.Usps = uspsShipmentEntity;
            postalShipmentEntity.Service = (int)PostalServiceType.GlobalPostSmartSaverPriority;

            shipmentEntity.Postal = postalShipmentEntity;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;

            BuyDotComTrackingType buyDotComTrackingType = updater.GetTrackingType(shipmentEntity);

            Assert.Equal(BuyDotComTrackingType.Usps, buyDotComTrackingType);
        }

        [Fact]
        public void GetTrackingType_ReturnsUsps_WhenEndiciaAndFirstClassServiceUsed()
        {
            postalShipmentEntity.Endicia = endiciaShipmentEntity;
            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;

            shipmentEntity.Postal = postalShipmentEntity;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;

            BuyDotComTrackingType buyDotComTrackingType = updater.GetTrackingType(shipmentEntity);

            Assert.Equal(BuyDotComTrackingType.Usps, buyDotComTrackingType);
        }

        [Fact]
        public void GetTrackingType_ReturnsUsps_WhenUspsAndFirstClassServiceUsed()
        {
            postalShipmentEntity.Usps = uspsShipmentEntity;
            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;

            shipmentEntity.Postal = postalShipmentEntity;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;

            BuyDotComTrackingType buyDotComTrackingType = updater.GetTrackingType(shipmentEntity);

            Assert.Equal(BuyDotComTrackingType.Usps, buyDotComTrackingType);
        }



        [Fact]
        public void GetTrackingType_ReturnsUsps_WhenOtherAndFirstClassServiceUsed()
        {
            shipmentEntity.Other = otherShipmentEntity;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;

            BuyDotComTrackingType buyDotComTrackingType = updater.GetTrackingType(shipmentEntity);

            Assert.Equal(BuyDotComTrackingType.Other, buyDotComTrackingType);
        }

        [Fact]
        public void GetTrackingType_ReturnsDhlGlobalMail_WhenOtherAndDhlServiceUsed()
        {
            otherShipmentEntity.Carrier = "dhl";
            shipmentEntity.Other = otherShipmentEntity;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;

            BuyDotComTrackingType buyDotComTrackingType = updater.GetTrackingType(shipmentEntity);

            Assert.Equal(BuyDotComTrackingType.DHLGlobalMail, buyDotComTrackingType);
        }
    }
}
