using System.Collections.Generic;
using Xunit;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using Moq;
using ShipWorks.Shipping.ScanForms;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia
{
    public class EndiciaScanFormGatewayTest
    {
        private ScanFormBatch scanFormBatch;
        private Mock<IScanFormCarrierAccount> carrierAccount;
        
        private EndiciaScanFormGateway testObject;

        [TestInitialize]
        public void Initialize()
        {
            carrierAccount = new Mock<IScanFormCarrierAccount>();
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns(new EndiciaAccountEntity());

            scanFormBatch = new ScanFormBatch(carrierAccount.Object, null, null);

            testObject = new EndiciaScanFormGateway();
        }

        [Fact]
        [ExpectedException(typeof(EndiciaException))]
        public void CreateScanForms_ThrowsEndiciaException_WhenAccountEntityIsNull_Test()
        {
            // Setup the GetAccountEntity method to return a null value
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns((IEntity2)null);

            testObject.CreateScanForms(scanFormBatch, new List<ShipmentEntity>());
        }

        [Fact]
        [ExpectedException(typeof(EndiciaException))]
        public void CreateScanForms_ThrowsEndiciaException_WhenShipmentsIsNull_Test()
        {
            testObject.CreateScanForms(scanFormBatch, null);
        }

        [Fact]
        [ExpectedException(typeof(EndiciaException))]
        public void CreateScanForms_ThrowsEndiciaException_WhenShipmentsIsEmpty_Test()
        {
            testObject.CreateScanForms(scanFormBatch, new List<ShipmentEntity>());
        }

        // Can't effectively unit test the rest of this class since it is calling into 
        // an external dependency that cannot be abstracted
    }
}
