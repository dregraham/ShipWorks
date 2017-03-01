using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using log4net;
using Moq;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Tests.Shared;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class LoggableAutoPrintServiceTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Mock<IAutoPrintService> autoPrintService;
        readonly Mock<ILog> logger;
        readonly LoggableAutoPrintService testObject;

        public LoggableAutoPrintServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            autoPrintService = mock.Mock<IAutoPrintService>();
            logger = mock.Mock<ILog>();

            testObject = mock.Create<LoggableAutoPrintService>();
        }

        [Fact]
        public void AllowAutoPrint_DelegatesToAutoPrintService_AllowAutoPrint()
        {
            ScanMessage scanMessage = new ScanMessage();
            testObject.AllowAutoPrint(scanMessage);

            autoPrintService.Verify(a => a.AllowAutoPrint(scanMessage));
        }

        [Fact]
        public async void Print_DelegatesToAutoPrintService_Prit()
        {
            AutoPrintServiceDto autoprintdto = new AutoPrintServiceDto();
            await testObject.Print(new AutoPrintServiceDto());

            autoPrintService.Verify(a => a.Print(autoprintdto));
        }

        [Fact]
        public async void Print_LogsError_WhenAutoPrintServiceDoesNotSendProcessShipmentsMessage()
        {
            string error = "something went wrong";
            SetupAutoPrintService("foo", 42, error, false);
            
            await testObject.Print(new AutoPrintServiceDto());

            logger.Verify(l => l.Error(error));
        }

        private void SetupAutoPrintService(string scannedBarcode, int orderID, string errorMessage, bool success)
        {
            var result = success ?
                GenericResult.FromSuccess(new AutoPrintResult(scannedBarcode, orderID)) :
                GenericResult.FromError(errorMessage, new AutoPrintResult(scannedBarcode, orderID));

            mock.Mock<IAutoPrintService>()
                .Setup(a => a.Print(It.IsAny<AutoPrintServiceDto>()))
                .ReturnsAsync(result);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
