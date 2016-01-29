using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
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
    public class EndiciaScanFormCarrierAccountTest
    {
        private EndiciaAccountEntity accountEntity;
        private Mock<IScanFormRepository> repository;
        private Mock<ILog> logger;

        private EndiciaScanFormCarrierAccount testObject;

        string errorMessageFromLogger;

        public EndiciaScanFormCarrierAccountTest()
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
                .Callback((object errorMessage) => errorMessageFromLogger = (string)errorMessage);

            Mock<IScanFormShipmentTypeName> scanFormShipmentTypeName = new Mock<IScanFormShipmentTypeName>();
            scanFormShipmentTypeName
                .Setup(x => x.GetShipmentTypeName(It.IsAny<ShipmentTypeCode>()))
                .Returns("USPS (Endicia)");

            testObject = new EndiciaScanFormCarrierAccount(repository.Object, accountEntity, logger.Object, scanFormShipmentTypeName.Object);
        }

        [Fact]
        public void ShippingCarrierName()
        {
            Assert.Equal("USPS (Endicia)", testObject.ShippingCarrierName);
        }

        [Fact]
        public void ShipmentTypeCode()
        {
            Assert.Equal(ShipmentTypeCode.Endicia, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void GetDescription_ReturnsDescriptionFieldValue()
        {
            Assert.Equal("USPS (Endicia) - this is a description", testObject.GetDescription());
        }

        [Fact]
        public void GetGateway_ReturnsEndiciaScanFormGateway()
        {
            Assert.IsAssignableFrom<EndiciaScanFormGateway>(testObject.GetGateway());
        }

        [Fact]
        public void GetPrinter_ReturnsDefaultScanFormPrinter()
        {
            Assert.IsAssignableFrom<DefaultScanFormPrinter>(testObject.GetPrinter());
        }

        [Fact]
        public void GetExistingScanFormBatches_DelegatesToRepository()
        {
            testObject.GetExistingScanFormBatches();

            repository.Verify(r => r.GetExistingScanFormBatches(testObject), Times.Once());
        }

        [Fact]
        public void GetEligibleShipmentIDs_DelegatesToRepository()
        {
            testObject.GetEligibleShipmentIDs();

            repository.Verify(r => r.GetShipmentIDs(It.IsAny<RelationPredicateBucket>()), Times.Once());
        }

        [Fact]
        public void GetEligibleShipmentIDs_Bucket()
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

        [Fact]
        public void Save_DelegatesToRepository()
        {
            ScanFormBatch batch = new ScanFormBatch(null, null, null);
            testObject.Save(batch);

            repository.Verify(r => r.Save(batch), Times.Once());
        }

        [Fact]
        public void Save_ThrowsShippingException_WhenScanFormBatchIsNull()
        {
            Assert.Throws<ShippingException>(() => testObject.Save(null));
        }

        [Fact]
        public void Save_LogsMessage_WhenScanFormBatchIsNull()
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
            Assert.Equal(expectedMessage, errorMessageFromLogger);
        }
    }
}
