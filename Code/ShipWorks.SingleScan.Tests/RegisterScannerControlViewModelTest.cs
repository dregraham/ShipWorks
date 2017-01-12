using System;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class RegisterScannerControlViewModelTest : IDisposable
    {
        private readonly RegisterScannerControlViewModel testObject;
        private readonly AutoMock mock;
        private readonly Mock<IScannerIdentifier> scannerIdentifier;
        private readonly Mock<IScannerService> scannerService;

        public RegisterScannerControlViewModelTest()
        {
            mock = AutoMock.GetLoose();
            IMessenger messenger = new TestMessenger();
            scannerIdentifier = mock.Mock<IScannerIdentifier>();
            scannerService = mock.Mock<IScannerService>();
            testObject = mock.Create<RegisterScannerControlViewModel>(new TypedParameter(typeof(IMessenger), messenger));

            messenger.Send(new ScanMessage(this, "some text", new IntPtr(123)));
        }

        [Fact]
        public void ScanDetected_SetsScanResult()
        {
            Assert.Equal("some text", testObject.ScanResult);
        }

        [Fact]
        public void ResultFound_SetToTrueWhenScanResultHasValue()
        {
            Assert.True(testObject.ResultFound);
        }

        [Fact]
        public void SaveScannerCommand_DelegatesToScannerIdentifier()
        {
            testObject.SaveScannerCommand.Execute(null);
            scannerIdentifier.Verify(s => s.Save(It.IsAny<IntPtr>()));
        }

        [Fact]
        public void Dispose_DelegatesToScannerService()
        {
            testObject.Dispose();
            scannerService.Verify(s => s.EndFindScanner());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}