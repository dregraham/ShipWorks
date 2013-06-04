using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ScanForms;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps
{
    [TestClass]
    public class StampsScanFormGatewayTest
    {
        private ScanForm scanForm;
        private Mock<IScanFormCarrierAccount> carrierAccount;
        
        private StampsScanFormGateway testObject;

        [TestInitialize]
        public void Initialize()
        {
            carrierAccount = new Mock<IScanFormCarrierAccount>();
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns(new StampsAccountEntity());

            scanForm = new ScanForm(carrierAccount.Object, 1000, string.Empty);

            testObject = new StampsScanFormGateway();
        }

        [TestMethod]
        [ExpectedException(typeof(StampsException))]
        public void FetchScanForm_ThrowsStampsException_WhenAccountEntityIsNull_Test()
        {
            // Setup the GetAccountEntity method to return a null value
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns((IEntity2)null);

            testObject.FetchScanForm(scanForm, new List<ShipmentEntity>());
        }

        [TestMethod]
        [ExpectedException(typeof(StampsException))]
        public void FetchScanForm_ThrowsStampsException_WhenShipmentsContainNonStampsShipment_Test()
        {
            // Create an Endicia shipment to get the gateway to throw an exception
            List<ShipmentEntity> shipments = new List<ShipmentEntity>()
            {
                new ShipmentEntity()
                {
                    Postal = new PostalShipmentEntity() { Endicia = new EndiciaShipmentEntity() }
                }
            };

            testObject.FetchScanForm(scanForm, shipments);
        }

        [TestMethod]
        [ExpectedException(typeof(StampsException))]
        public void FetchScanForm_ThrowsStampsException_WhenShipmentsIsNull_Test()
        {
            testObject.FetchScanForm(scanForm, null);
        }

        [TestMethod]
        [ExpectedException(typeof(StampsException))]
        public void FetchScanForm_ThrowsStampsException_WhenShipmentsIsEmpty_Test()
        {
            testObject.FetchScanForm(scanForm, new List<ShipmentEntity>());
        }

        // Can't effectively unit test the rest of this class since it is calling into 
        // an external dependency that cannot be abstracted
    }
}
