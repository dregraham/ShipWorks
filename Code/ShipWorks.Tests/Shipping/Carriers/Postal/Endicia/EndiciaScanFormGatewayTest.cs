using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using Moq;
using ShipWorks.Shipping.ScanForms;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia
{
    [TestClass]
    public class EndiciaScanFormGatewayTest
    {
        private ScanForm scanForm;
        private Mock<IScanFormCarrierAccount> carrierAccount;
        
        private EndiciaScanFormGateway testObject;

        [TestInitialize]
        public void Initialize()
        {
            carrierAccount = new Mock<IScanFormCarrierAccount>();
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns(new EndiciaAccountEntity());

            scanForm = new ScanForm(carrierAccount.Object, 1000, string.Empty);

            testObject = new EndiciaScanFormGateway();
        }

        [TestMethod]
        [ExpectedException(typeof(EndiciaException))]
        public void FetchScanForm_ThrowsEndiciaException_WhenAccountEntityIsNull_Test()
        {
            // Setup the GetAccountEntity method to return a null value
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns((IEntity2)null);

            testObject.FetchScanForm(scanForm, new List<ShipmentEntity>());
        }

        [TestMethod]
        [ExpectedException(typeof(EndiciaException))]
        public void FetchScanForm_ThrowsEndiciaException_WhenShipmentsIsNull_Test()
        {
            testObject.FetchScanForm(scanForm, null);
        }

        [TestMethod]
        [ExpectedException(typeof(EndiciaException))]
        public void FetchScanForm_ThrowsEndiciaException_WhenShipmentsIsEmpty_Test()
        {
            testObject.FetchScanForm(scanForm, new List<ShipmentEntity>());
        }

        // Can't effectively unit test the rest of this class since it is calling into 
        // an external dependency that cannot be abstracted
    }
}
