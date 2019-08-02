using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.SingleScan;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupAutoPrintServiceTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IAutoPrintService> autoPrintService;
        private readonly OrderLookupAutoPrintService testObject;

        public OrderLookupAutoPrintServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            autoPrintService = mock.Mock<IAutoPrintService>();

            testObject = mock.Create<OrderLookupAutoPrintService>();
        }

        [Fact]
        public async Task AutoPrintShipment_DelegatesToAutoPrintServiceAllowAutoPrint()
        {
            var message = new SingleScanMessage(this, new ScanMessage(this, "123", IntPtr.Zero));

            await testObject.AutoPrintShipment(123, message.ScannedText);

            autoPrintService.Verify(a => a.AllowAutoPrint(message.ScannedText));
        }

        [Fact]
        public async Task AutoPrintShipment_DelegatesToAutoPrintServicePrint()
        {
            SingleScanMessage message = new SingleScanMessage(this, new ScanMessage(this, "123", IntPtr.Zero));
            autoPrintService.Setup(a => a.AllowAutoPrint(message.ScannedText)).Returns(true);

            await testObject.AutoPrintShipment(123, message.ScannedText);

            autoPrintService.Verify(a => a.Print(new AutoPrintServiceDto() { OrderID = 123, MatchedOrderCount = 1, ScannedBarcode = message.ScannedText}));
        }

        [Fact]
        public async Task AutoPrintShipment_ReturnsEmptyAutoPrintCompletionResult_WhenAutoPrintIsDisabled()
        {
            SingleScanMessage message = new SingleScanMessage(this, new ScanMessage(this, "123", IntPtr.Zero));
            autoPrintService.Setup(a => a.AllowAutoPrint(message.ScannedText)).Returns(false);

            AutoPrintCompletionResult result = await testObject.AutoPrintShipment(123, message.ScannedText);

            Assert.Empty(result.ProcessShipmentResults);
        }
    }
}
