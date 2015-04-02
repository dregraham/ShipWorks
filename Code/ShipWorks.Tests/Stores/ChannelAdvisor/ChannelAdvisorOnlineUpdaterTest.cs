﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestClass]
    public class ChannelAdvisorOnlineUpdaterTest
    {
        private ChannelAdvisorOrderEntity orderEntity;
        private FedExShipmentEntity fedExEntity;
        private UpsShipmentEntity upsEntity;
        private ShipmentEntity shipmentEntity;
        private PostalShipmentEntity postalShipmentEntity;
        private EndiciaShipmentEntity endiciaShipmentEntity;
        private UspsShipmentEntity uspsShipmentEntity;
        private OtherShipmentEntity otherShipmentEntity;
        private IParcelShipmentEntity iparcelShipmentEntity;
        private OnTracShipmentEntity ontracShipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            orderEntity = new ChannelAdvisorOrderEntity { OrderNumber = 123456 };
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

        [TestMethod]
        public void GetShipmentClassCode_ReturnsConsolidator_WhenEndiciaAndConsolidatorServiceUsed_Test()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int)PostalServiceType.ConsolidatorDomestic;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity);

            Assert.AreEqual("CONSOLIDATOR", code);
        }

        [TestMethod]
        public void GetShipmentClassCode_ReturnsGlobalMail_WhenEndiciaAndDhlServiceUsed_Test()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity);

            Assert.AreEqual("Global Mail", code);
        }

        [TestMethod]
        public void GetShipmentClassCode_ReturnsGlobalMail_WhenUspsAndDhlServiceUsed_Test()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity);

            Assert.AreEqual("Global Mail", code);
        }

        [TestMethod]
        public void GetShipmentClassCode_ReturnsFirstClass_WhenEndiciaAndFirstClassServiceUsed_Test()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity);

            Assert.AreEqual("FIRSTCLASS", code);
        }

        [TestMethod]
        public void GetShipmentClassCode_ReturnsFirstClass_WhenUspsAndFirstClassServiceUsed_Test()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity);

            Assert.AreEqual("FIRSTCLASS", code);
        }

        [TestMethod]
        public void GetShipmentClassCode_ReturnsMi_WhenUpsAndMiServiceUsed_Test()
        {
            SetupShipmentDefaults(ShipmentTypeCode.UpsOnLineTools);

            upsEntity.Service = (int) UpsServiceType.UpsMailInnovationsFirstClass;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity);

            Assert.AreEqual("MI", code);

            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.UpsWorldShip;
            code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity);

            Assert.AreEqual("MI", code);
        }

        [TestMethod]
        public void GetShipmentClassCode_ReturnsGround_WhenUpsAndGroundServiceUsed_Test()
        {
            SetupShipmentDefaults(ShipmentTypeCode.UpsOnLineTools);

            upsEntity.Service = (int)UpsServiceType.UpsGround;

            string code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity);

            Assert.AreEqual("GROUND", code);

            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.UpsWorldShip;
            code = ChannelAdvisorOnlineUpdater.GetShipmentClassCode(shipmentEntity);

            Assert.AreEqual("GROUND", code);
        }

        [TestMethod]
        public void GetCarrierCode_ReturnsConsolidator_WhenEndiciaAndConsolidatorServiceUsed_Test()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int)PostalServiceType.ConsolidatorDomestic;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity);

            Assert.AreEqual("Consolidator", code);
        }

        [TestMethod]
        public void GetCarrierCode_ReturnsDHL_WhenEndiciaAndDhlServiceUsed_Test()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity);

            Assert.AreEqual("DHL", code);
        }

        [TestMethod]
        public void GetCarrierCode_ReturnsDHL_WhenUspsAndDhlServiceUsed_Test()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity);

            Assert.AreEqual("DHL", code);
        }

        [TestMethod]
        public void GetCarrierCode_ReturnsUsps_WhenEndiciaAndFirstClassServiceUsed_Test()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Endicia);

            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity);

            Assert.AreEqual("USPS", code);
        }

        [TestMethod]
        public void GetCarrierCode_ReturnsUsps_WhenUspsAndDhlServiceUsed_Test()
        {
            SetupShipmentDefaults(ShipmentTypeCode.Usps);

            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;

            string code = ChannelAdvisorOnlineUpdater.GetCarrierCode(shipmentEntity);

            Assert.AreEqual("USPS", code);
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