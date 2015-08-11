using System.Collections.Generic;
using Xunit;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.ScanForms;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Express1.ScanForm
{
    public class Express1ScanFormGatewayTest
    {
        private ScanFormBatch scanFormBatch;
        private Mock<IScanFormCarrierAccount> carrierAccount;

        private Express1EndiciaScanFormGateway testObject;

        public Express1ScanFormGatewayTest()
        {
            carrierAccount = new Mock<IScanFormCarrierAccount>();
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns(new EndiciaAccountEntity());

            scanFormBatch = new ScanFormBatch(carrierAccount.Object, null, null);

            testObject = new Express1EndiciaScanFormGateway();
        }

        [Fact]
        public void CreateScanForms_ThrowsExpress1Exception_WhenAccountEntityIsNull_Test()
        {
            // Setup the GetAccountEntity method to return a null value
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns((IEntity2)null);

            Assert.Throws<Express1EndiciaException>(() => testObject.CreateScanForms(scanFormBatch, new List<ShipmentEntity>()));
        }

        [Fact]
        public void CreateScanForms_ThrowsExpress1Exception_WhenShipmentsIsNull_Test()
        {
            Assert.Throws<Express1EndiciaException>(() => testObject.CreateScanForms(scanFormBatch, null));
        }

        [Fact]
        public void CreateScanForms_ThrowsExpress1Exception_WhenShipmentsIsEmpty_Test()
        {
            Assert.Throws<Express1EndiciaException>(() => testObject.CreateScanForms(scanFormBatch, new List<ShipmentEntity>()));
        }

        // Can't effectively unit test the rest of this class since it is calling into 
        // an external dependency that cannot be abstracted
    }
}
