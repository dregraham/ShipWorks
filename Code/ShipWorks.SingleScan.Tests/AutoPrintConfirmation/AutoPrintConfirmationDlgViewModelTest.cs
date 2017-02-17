

using System;
using System.Windows.Input;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.SingleScan.AutoPrintConfirmation;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests.AutoPrintConfirmation
{
    public class AutoPrintConfirmationDlgViewModelTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly AutoPrintConfirmationDlgViewModel testObject;
        private readonly Mock<ISchedulerProvider> scheduleProvider;
        private readonly TestMessenger testMessenger;

        public AutoPrintConfirmationDlgViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            scheduleProvider = mock.WithMockImmediateScheduler();
            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            testObject = mock.Create<AutoPrintConfirmationDlgViewModel>();
            testObject.Load("barcode", "displayText", "continueText");
        }

        [Fact]
        public void Load_SetsDisplayText()
        {


            Assert.Equal("displayText", testObject.DisplayText);
        }

        [Fact]
        public void Load_SetsContinueText()
        {

            Assert.Equal("continueText", testObject.ContinueText);
        }

        [Fact]
        public void ReceiveScanMessage_CallsCloseWithTrue_WhenBarcodeMatchesBarcodeAcceptanceText()
        {
            bool? dialogResult = null;
            testObject.Close = result => dialogResult = result;

            SendScanMessage("barcode");
            
            Assert.NotNull(dialogResult);
            Assert.True(dialogResult.Value);
        }

        [Fact]
        public void ContinueClickCommand_CallsCloseWithTrue()
        {
            bool? dialogResult = null;
            testObject.Close = result => dialogResult = result;

            testObject.ContinueClickCommand.Execute(null);

            Assert.NotNull(dialogResult);
            Assert.True(dialogResult.Value);
        }

        [Fact]
        public void CancelClickCommand_CallsCloseWithFalse()
        {
            bool? dialogResult = null;
            testObject.Close = result => dialogResult = result;

            testObject.CancelClickCommand.Execute(null);

            Assert.NotNull(dialogResult);
            Assert.False(dialogResult.Value);
        }

        [Fact]
        public void ReceiveScanMessage_DoesNotCallClose_WhenBarcodeDoesNotMatchBarcodeAcceptanceText()
        {
            bool? dialogResult = null;
            testObject.Close = result => dialogResult = result;

            SendScanMessage("not the barcode");

            Assert.Null(dialogResult);
        }

        private void SendScanMessage(string scannedText)
        {
            testMessenger.Send(new ScanMessage(this, scannedText, IntPtr.Zero));
        }


        public void Dispose()
        {
            testMessenger.Dispose();
            testObject?.Dispose();
            mock.Dispose();
        }

    }
}