﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ScanForms;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping;
using log4net;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps
{
    [TestClass]
    public class StampsScanFormCarrierAccountTest
    {
        private StampsScanFormCarrierAccount testObject;
        private Mock<IScanFormRepository> repository;
        private Mock<ILog> logger;

        string errorMessageFromLogger;

        [TestInitialize]
        public void Initialize()
        {
            StampsAccountEntity accountEntity = new StampsAccountEntity()
            {
                StampsAccountID = 123456,
                Username = "testUsername",
                Password = "password",
                PostalCode = "63102",
                Street1 = "1 Memorial Drive"
            };

            repository = new Mock<IScanFormRepository>();
            repository
                .Setup(r => r.GetShipmentIDs(It.IsAny<RelationPredicateBucket>()))
                .Returns(new List<long>());

            logger = new Mock<ILog>();
            logger.Setup(l => l.Error(It.IsAny<string>()))
                  .Callback((object errorMessage) => errorMessageFromLogger = (string) errorMessage);

            testObject = new StampsScanFormCarrierAccount(repository.Object, accountEntity, logger.Object);
        }

        [TestMethod]
        public void ShippingCarrierName_ReturnsStampsDotCom_Test()
        {
            Assert.AreEqual("USPS (Stamps.com)", testObject.ShippingCarrierName);
        }

        [TestMethod]
        public void ShipmentTypeCode_Test()
        {
            Assert.AreEqual(ShipmentTypeCode.Stamps, testObject.ShipmentTypeCode);
        }

        [TestMethod]
        public void GetDescription_Test()
        {
            Assert.AreEqual("USPS (Stamps.com) - testUsername", testObject.GetDescription());
        }

        [TestMethod]
        public void GetGateway_ReturnsStampsScanFormGateway_Test()
        {
            Assert.IsInstanceOfType(testObject.GetGateway(), typeof(StampsScanFormGateway));
        }
        
        [TestMethod]
        public void GetPrinter_ReturnsDefaultScanFormPrinter_Test()
        {
            Assert.IsInstanceOfType(testObject.GetPrinter(), typeof(DefaultScanFormPrinter));
        }

        [TestMethod]
        public void GetExistingScanFormBatches_DelegatesToRepository_Test()
        {
            testObject.GetExistingScanFormBatches();

            repository.Verify(r => r.GetExistingScanFormBatches(testObject), Times.Once());
        }

        [TestMethod]
        public void GetEligibleShipmentIDs_DelegatesToRepository_Test()
        {
            testObject.GetEligibleShipmentIDs();

            repository.Verify(r => r.GetShipmentIDs(It.IsAny<RelationPredicateBucket>()), Times.Once());
        }

        [TestMethod]
        public void GetEligibleShipmentIDs_Bucket_Test()
        {
            testObject.GetEligibleShipmentIDs();

            // Verify the predicate bucket passed to the repository is configured correctly
            repository.Verify
                (
                    r => r.GetShipmentIDs(It.Is<RelationPredicateBucket>
                    (
                        // Would be nice to test the actual predicate expressions, but I was unable 
                        // to determine how to access the predicate expressions that line up with 
                        // those built in the method being tested
                        b => b.Relations.Count == 2
                    )
                ), Times.Once());
            
        }

        [TestMethod]
        public void Save_DelegatesToRepository_Test()
        {
            ScanFormBatch batch = new ScanFormBatch(null, null, null);
            testObject.Save(batch);

            repository.Verify(r => r.Save(batch), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ShippingException))]
        public void Save_ThrowsShippingException_WhenScanFormBatchIsNull_Test()
        {
            testObject.Save(null);
        }

        [TestMethod]        
        public void Save_LogsMessage_WhenScanFormBatchIsNull_Test()
        {
            try
            {
                testObject.Save(null);
            }
            // Exception is caught so we can inspect the logger
            catch (ShippingException)
            { }

            // Verify the correct message was logged
            string expectedMessage = "ShipWorks was unable to create a SCAN form through USPS (Stamps.com) at this time. Please try again later. (A null scan form batch tried to be saved.)";
            Assert.AreEqual(expectedMessage, errorMessageFromLogger);
        }
    }
}
