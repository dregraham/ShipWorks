using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Tests.Stores.Amazon
{
    [TestClass]
    public class MwsClientTests
    {
        private ShipmentEntity shipmentEntity;
        private AmazonOrderEntity orderEntity;
        private PostalShipmentEntity postalShipmentEntity;
        private OtherShipmentEntity otherShipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            orderEntity = new AmazonOrderEntity { OrderNumber = 123456 };
            postalShipmentEntity = new PostalShipmentEntity { Service = (int)PostalServiceType.FirstClass };
            otherShipmentEntity = new OtherShipmentEntity { Carrier = "Some other carrier", Service = "Fast Ground" };
            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow, ShipmentType = (int)ShipmentTypeCode.Usps, Postal = postalShipmentEntity };
        }

        [TestMethod]
        public void ClockSyncTest()
        {
            using (AmazonMwsClient client = new AmazonMwsClient(new AmazonStoreEntity {AmazonApiRegion = "US"}))
            {
                Assert.IsTrue(client.ClockInSyncWithMWS());   
            }
        }

        [TestMethod]
        public void ValidateSigning()
        {
            string stringToSign = "POST\nmws.amazonservices.com\n/Orders/2011-01-01\nAWSAccessKeyId=AKIAJU465C2O2U6WNQTA&Action=ListOrderItems&AmazonOrderId=SHIPWORKS_CONNECT_ATTEMPT&SellerId=A2OIXR4TA6XKOD&SignatureMethod=HmacSHA256&SignatureVersion=2&Timestamp=2011-08-19T15%3A55%3A39Z&Version=2011-01-01";
            string signature = "XKab7VXEklL8EYE0HcqRF97fgjqTNsM7NFe/gJ7eWLE=";

            string signed = RequestSignature.CreateRequestSignature(stringToSign, "0+NcTFU11/qooQriFFO7k0JxY64t/P38DIAAAgeW", SigningAlgorithm.SHA256);

            Assert.AreEqual(signature, signed);
        }

        [TestMethod]
        public void GetCarrierName_ReturnsUsps_WhenUspsAndFirstClass_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            string carrierName = AmazonMwsClient.GetCarrierName(shipmentEntity, ShipmentTypeCode.Usps);

            Assert.AreEqual("USPS", carrierName);
        }

        [TestMethod]
        public void GetCarrierName_ReturnsUsps_WhenEndiciaAndFirstClass_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;
            string carrierName = AmazonMwsClient.GetCarrierName(shipmentEntity, ShipmentTypeCode.Usps);

            Assert.AreEqual("USPS", carrierName);
        }

        [TestMethod]
        public void GetCarrierName_ReturnsDhlGlobalMail_WhenEndiciaAndDhl_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelStandard;

            string carrierName = AmazonMwsClient.GetCarrierName(shipmentEntity, ShipmentTypeCode.Endicia);

            Assert.AreEqual("DHL Global Mail", carrierName);
        }

        [TestMethod]
        public void GetCarrierName_ReturnsDhlGlobalMail_WhenUspsAndDhl_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelStandard;

            string carrierName = AmazonMwsClient.GetCarrierName(shipmentEntity, ShipmentTypeCode.Usps);

            Assert.AreEqual("DHL Global Mail", carrierName);
        }

        [TestMethod]
        public void GetCarrierName_ReturnsConsolidator_WhenEndiciaAndConsolidator_Test()
        {
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Endicia;
            postalShipmentEntity.Service = (int)PostalServiceType.ConsolidatorDomestic;

            string carrierName = AmazonMwsClient.GetCarrierName(shipmentEntity, ShipmentTypeCode.Endicia);

            Assert.AreEqual("Consolidator", carrierName);
        }

        [TestMethod]
        public void GetCarrierName_ReturnsOtherCarrierDesc_WhenOther_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            shipmentEntity.Other = otherShipmentEntity;

            string carrierName = AmazonMwsClient.GetCarrierName(shipmentEntity, ShipmentTypeCode.Endicia);

            Assert.AreEqual("Some other carrier", carrierName);
        }

    }
}
