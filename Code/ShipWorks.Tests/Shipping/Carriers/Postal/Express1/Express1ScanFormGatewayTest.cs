using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using Moq;
using ShipWorks.Shipping.ScanForms;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Express1
{
    [TestClass]
    public class Express1ScanFormGatewayTest
    {
        private ScanFormBatch scanFormBatch;
        private Mock<IScanFormCarrierAccount> carrierAccount;

        private Express1EndiciaScanFormGateway testObject;

        [TestInitialize]
        public void Initialize()
        {
            carrierAccount = new Mock<IScanFormCarrierAccount>();
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns(new EndiciaAccountEntity());

            scanFormBatch = new ScanFormBatch(carrierAccount.Object, null);

            testObject = new Express1EndiciaScanFormGateway();
        }

        [TestMethod]
        [ExpectedException(typeof(Express1EndiciaException))]
        public void CreateScanForms_ThrowsExpress1Exception_WhenAccountEntityIsNull_Test()
        {
            // Setup the GetAccountEntity method to return a null value
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns((IEntity2)null);

            testObject.CreateScanForms(scanFormBatch, new List<ShipmentEntity>());
        }
        
        [TestMethod]
        [ExpectedException(typeof(Express1EndiciaException))]
        public void CreateScanForms_ThrowsExpress1Exception_WhenShipmentsIsNull_Test()
        {
            testObject.CreateScanForms(scanFormBatch, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Express1EndiciaException))]
        public void CreateScanForms_ThrowsExpress1Exception_WhenShipmentsIsEmpty_Test()
        {
            testObject.CreateScanForms(scanFormBatch, new List<ShipmentEntity>());
        }

        // Can't effectively unit test the rest of this class since it is calling into 
        // an external dependency that cannot be abstracted
    }
}
