using Xunit;
using ShipWorks.Shipping.ScanForms;
using Moq;
using ShipWorks.Shipping;
using System.Windows.Forms;

namespace ShipWorks.Tests.Shipping.ScanForms
{
    public class ScanFormTest
    {
        private Mock<IScanFormPrinter> printer;
        private Mock<IScanFormCarrierAccount> carrierAccount;

        private ScanForm testObject;

        public ScanFormTest()
        {
            // Setup a mocked printer that always returns true
            printer = new Mock<IScanFormPrinter>();
            printer.Setup(p => p.Print(It.IsAny<IWin32Window>(), It.IsAny<ScanForm>())).Returns(true);


            // Now we can setup our carrier account so it returns the mocked repository, gateway, and printer
            carrierAccount = new Mock<IScanFormCarrierAccount>();
            carrierAccount.Setup(c => c.Save(It.IsAny<ScanFormBatch>())).Returns(1000);
            carrierAccount.Setup(c => c.GetPrinter()).Returns(printer.Object);

            // Now we can configure our test object
            testObject = new ScanForm(carrierAccount.Object, 1000, string.Empty);
        }

        [Fact]
        public void Print_ThrowsShippingException_WhenCarrierAccountIsNull()
        {
            // Create a new test object that has a null carrier to generate the exception
            testObject = new ScanForm(null, 1000, string.Empty);
            Assert.Throws<ShippingException>(() => testObject.Print(null));
        }

        [Fact]
        public void Print_DelegatesToCarrerAccount_ForPrinter()
        {
            testObject.Print(new Form());

            carrierAccount.Verify(c => c.GetPrinter(), Times.Once());
        }

        [Fact]
        public void Print_DelegatesToScanFormPrinter()
        {
            // The owner doesn't matter since the carrier's printer is mocked
            Form owner = new Form();
            testObject.Print(owner);

            // Verify the PrintScanForm method was called and called with the correct
            // parameters
            printer.Verify(p => p.Print(owner, testObject), Times.Once());
        }

        [Fact]
        public void Print_ReturnsTrue_WhenPrintingIsSuccessful()
        {
            // The test object is already setup with the success path
            bool success = testObject.Print(new Form());

            Assert.True(success);
        }

        [Fact]
        public void Print_ReturnsFalse_WhenPrintingFails()
        {
            // Setup our mock printer to return a false value to simulate the printer failing
            printer.Setup(p => p.Print(It.IsAny<Form>(), It.IsAny<ScanForm>())).Returns(false);

            bool success = testObject.Print(new Form());

            Assert.False(success);
        }
    }
}
