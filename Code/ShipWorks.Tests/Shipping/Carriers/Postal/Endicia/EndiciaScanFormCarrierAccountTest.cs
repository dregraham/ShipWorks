﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.ScanForms;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping;
using log4net;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia
{
    [TestClass]
    public class EndiciaScanFormCarrierAccountTest
    {
        private EndiciaAccountEntity accountEntity;
        private Mock<IScanFormRepository> repository;
        private Mock<ILog> logger;

        private EndiciaScanFormCarrierAccount testObject;

        string errorMessageFromLogger;

        [TestInitialize]
        public void Initialize()
        {
            accountEntity = new EndiciaAccountEntity()
            {
                EndiciaAccountID = 123456789,
                Description = "this is a description"
            };

            repository = new Mock<IScanFormRepository>();
            repository.Setup(r => r.GetShipmentIDs(It.IsAny<RelationPredicateBucket>())).Returns(new List<long>());

            logger = new Mock<ILog>();
            logger
                .Setup(l => l.Error(It.IsAny<string>()))
                .Callback((object errorMessage) => errorMessageFromLogger = (string) errorMessage);
            
            Mock<IScanFormShipmentTypeName> scanFormShipmentTypeName = new Mock<IScanFormShipmentTypeName>();
            scanFormShipmentTypeName
                .Setup(x => x.GetShipmentTypeName(It.IsAny<ShipmentTypeCode>()))
                .Returns("USPS (Endicia)");

            testObject = new EndiciaScanFormCarrierAccount(repository.Object, accountEntity, logger.Object, scanFormShipmentTypeName.Object);
        }

        [TestMethod]
        public void ShippingCarrierName_Test()
        {
            Assert.AreEqual("USPS (Endicia)", testObject.ShippingCarrierName);
        }

        [TestMethod]
        public void ShipmentTypeCode_Test()
        {
            Assert.AreEqual(ShipmentTypeCode.Endicia, testObject.ShipmentTypeCode);
        }

        [TestMethod]
        public void GetDescription_ReturnsDescriptionFieldValue_Test()
        {
            Assert.AreEqual("USPS (Endicia) - this is a description", testObject.GetDescription());
        }

        [TestMethod]
        public void GetGateway_ReturnsEndiciaScanFormGateway_Test()
        {
            Assert.IsInstanceOfType(testObject.GetGateway(), typeof(EndiciaScanFormGateway));
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
            ScanFormBatch batch = new ScanFormBatch(null, null);
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
            const string expectedMessage = "ShipWorks was unable to create a SCAN form through USPS (Endicia) at this time. Please try again later. (A null scan form batch tried to be saved.)";
            Assert.AreEqual(expectedMessage, errorMessageFromLogger);
        }
    }
}
