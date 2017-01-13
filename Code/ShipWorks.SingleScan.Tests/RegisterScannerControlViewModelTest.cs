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
        private readonly ScannerRegistrationControlViewModel testObject;
        private readonly AutoMock mock;
        private readonly Mock<IScannerIdentifier> scannerIdentifier;
        private readonly Mock<IScannerRegistrationListener> scannerRegistrationListener;

        public RegisterScannerControlViewModelTest()
        {
            mock = AutoMock.GetLoose();
            IMessenger messenger = new TestMessenger();
            scannerIdentifier = mock.Mock<IScannerIdentifier>();
            scannerRegistrationListener = mock.Mock<IScannerRegistrationListener>();
            testObject = mock.Create<ScannerRegistrationControlViewModel>(new TypedParameter(typeof(IMessenger), messenger));

            messenger.Send(new ScanMessage(this, "some text", new IntPtr(123)));
        }

        [Fact]
        public void Constructor_StartsScannerRegistrationListener()
        {
            scannerRegistrationListener.Verify(s => s.Start());
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
        public void Dispose_StopsScannerRegistrationListener()
        {
            testObject.Dispose();
            scannerRegistrationListener.Verify(s => s.Stop());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}