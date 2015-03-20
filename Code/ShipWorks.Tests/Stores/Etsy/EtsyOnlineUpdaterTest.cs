using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Stores.Platforms.Etsy;

namespace ShipWorks.Tests.Stores.Etsy
{
    /// <summary>
    /// Summary description for NeweggWebClientTest
    /// </summary>
    [TestClass]
    public class EtsyOnlineUpdaterTest
    {
        private EtsyOrderEntity orderEntity;
        private ShipmentEntity shipmentEntity;
        private PostalShipmentEntity postalShipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            orderEntity = new EtsyOrderEntity { OrderNumber = 123456 };
            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow };
            postalShipmentEntity = new PostalShipmentEntity { Service = (int)PostalServiceType.FirstClass };
        }

        [TestMethod]
        public void GetEtsyCarrierCode_ReturnsDhl_WhenEndiciaAndDhlServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = EtsyOnlineUpdater.GetEtsyCarrierCode(shipmentEntity);

            Assert.AreEqual("dhl", carrierCode);
        }

        [TestMethod]
        public void GetEtsyCarrierCode_ReturnsDhl_WhenUspsAndDhlServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = EtsyOnlineUpdater.GetEtsyCarrierCode(shipmentEntity);

            Assert.AreEqual("dhl", carrierCode);
        }

        [TestMethod]
        public void GetEtsyCarrierCode_ReturnsUsps_WhenEndiciaAndFirstClassServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = EtsyOnlineUpdater.GetEtsyCarrierCode(shipmentEntity);

            Assert.AreEqual("usps", carrierCode);
        }

        [TestMethod]
        public void GetEtsyCarrierCode_ReturnsUsps_WhenUspsAndFirstClassServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = EtsyOnlineUpdater.GetEtsyCarrierCode(shipmentEntity);

            Assert.AreEqual("usps", carrierCode);
        }

        [TestMethod]
        public void GetEtsyCarrierCode_ReturnsUsps_WhenOther_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;

            string carrierCode = EtsyOnlineUpdater.GetEtsyCarrierCode(shipmentEntity);

            Assert.AreEqual("other", carrierCode);
        }
    }
}
