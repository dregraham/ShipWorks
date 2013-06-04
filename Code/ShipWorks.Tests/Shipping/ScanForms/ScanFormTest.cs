using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.ScanForms;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping;
using System.Windows.Forms;


namespace ShipWorks.Tests.Shipping.ScanForms
{
    [TestClass]    
    public class ScanFormTest
    {
        private Mock<IScanFormGateway> gateway;
        private Mock<IScanFormPrinter> printer;
        private Mock<IScanFormCarrierAccount> carrierAccount;
        
        private ScanForm testObject;

        [TestInitialize]
        public void Initialize()
        {
            // Setup a mocked gateway that we're going to use with the carrier account; the actual type
            // of entity it returns doesn't matter for our tests, but we're just going to have it return 
            // a stamps scan form entity
            gateway = new Mock<IScanFormGateway>();
            gateway.Setup(g => g.FetchScanForm(It.IsAny<ScanForm>(), It.IsAny<IEnumerable<ShipmentEntity>>()))
                .Returns(new StampsScanFormEntity());
            
            // Setup a mocked printer that always returns true
            printer = new Mock<IScanFormPrinter>();
            printer.Setup(p => p.Print(It.IsAny<IWin32Window>(), It.IsAny<ScanForm>())).Returns(true);


            // Now we can setup our carrier account so it returns the mocked repository, gateway, and printer
            carrierAccount = new Mock<IScanFormCarrierAccount>();
            carrierAccount.Setup(c => c.Save(It.IsAny<ScanFormBatch>())).Returns(1000);
            carrierAccount.Setup(c => c.GetGateway()).Returns(gateway.Object);
            carrierAccount.Setup(c => c.GetPrinter()).Returns(printer.Object);

            // Now we can configure our test object
            testObject = new ScanForm(carrierAccount.Object, 1000, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ShippingException))]
        public void Generate_ThrowsShippingException_WhenShipmentsIsNull_Test()
        {
            testObject = new ScanForm(carrierAccount.Object, 1000, string.Empty, null);
            testObject.Generate();
        }

        [TestMethod]
        [ExpectedException(typeof(ShippingException))]
        public void Generate_ThrowsShippingException_WhenZeroShipments_Test()
        {
            testObject = new ScanForm(carrierAccount.Object, 1000, string.Empty, new List<ShipmentEntity>());
            testObject.Generate();
        }

        [TestMethod]
        public void Generate_GetsGatewayOfCarrierAccount_Test()
        {
            // Our setup has already been done in the Initialize method so we can just jump right into the test
            List<ShipmentEntity> shipments = new List<ShipmentEntity>
            {
                new ShipmentEntity(),
                new ShipmentEntity()
            };

            testObject = new ScanForm(carrierAccount.Object, 1000, string.Empty, shipments);
            testObject.Generate();

            carrierAccount.Verify(c => c.GetGateway(), Times.Once());
        }

        [TestMethod]
        public void Generate_DelegatesToGateway_Test()
        {
            // We're going to provide a list of shipments so we can verify that this
            // specific shipment list is sent to the gateway
            List<ShipmentEntity> shipments = new List<ShipmentEntity>
            {
                new ShipmentEntity(),
                new ShipmentEntity()
            };

            testObject = new ScanForm(carrierAccount.Object, 1000, string.Empty, shipments);
            testObject.Generate();

            // Check that the gateway was used and the correct arguments were provided to it
            gateway.Verify(g => g.FetchScanForm(testObject, shipments), Times.Once());
        }

        
        [TestMethod]
        [ExpectedException(typeof(ShippingException))]
        public void Generate_ThrowsStampsException_WhenScanFormEntityIsNull_Test()
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>
            {
                new ShipmentEntity(),
                new ShipmentEntity()
            };

            testObject = new ScanForm(carrierAccount.Object, 1000, string.Empty, shipments);

            // Override the gateway to return a null entity
            gateway.Setup(g => g.FetchScanForm(It.IsAny<ScanForm>(), It.IsAny<IEnumerable<ShipmentEntity>>())).Returns((IEntity2)null);


            testObject.Generate();
        }

        [TestMethod]
        [ExpectedException(typeof(ShippingException))]
        public void Print_ThrowsShippingException_WhenCarrierAccountIsNull_Test()
        {
            // Create a new test object that has a null carrier to generate the exception
            testObject = new ScanForm(null, 1000, string.Empty);
            testObject.Print(null);
        }

        [TestMethod]
        public void Print_DelegatesToCarrerAccount_ForPrinter_Test()
        {
            testObject.Print(new Form());

            carrierAccount.Verify(c => c.GetPrinter(), Times.Once());
        }

        [TestMethod]
        public void Print_DelegatesToScanFormPrinter_Test()
        {
            // The owner doesn't matter since the carrier's printer is mocked
            Form owner = new Form();
            testObject.Print(owner);

            // Verify the PrintScanForm method was called and called with the correct
            // parameters
            printer.Verify(p => p.Print(owner, testObject), Times.Once());
        }

        [TestMethod]
        public void Print_ReturnsTrue_WhenPrintingIsSuccessful_Test()
        {
            // The test object is already setup with the success path
            bool success = testObject.Print(new Form());

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void Print_ReturnsFalse_WhenPrintingFails_Test()
        {
            // Setup our mock printer to return a false value to simulate the printer failing
            printer.Setup(p => p.Print(It.IsAny<Form>(), It.IsAny<ScanForm>())).Returns(false);

            bool success = testObject.Print(new Form());

            Assert.IsFalse(success);
        }
    }
}
