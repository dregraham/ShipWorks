﻿using System;
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
                Ups = upsEntity
            };
        }
    }
}
