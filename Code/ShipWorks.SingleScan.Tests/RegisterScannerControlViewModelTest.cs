using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.SingleScan;
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
        private readonly TestMessenger messenger;

        public RegisterScannerControlViewModelTest()
        {
            mock = AutoMock.GetLoose();
            messenger = new TestMessenger();
            scannerIdentifier = mock.Mock<IScannerIdentifier>();
            scannerIdentifier.Setup(x => x.Save(It.IsAny<IntPtr>())).Returns("Foo");

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
            var devicePtr = new IntPtr(123);
            messenger.Send(new ScanMessage(this, "Bar", new IntPtr(123)));

            testObject.SaveScannerCommand.Execute(null);
            scannerIdentifier.Verify(s => s.Save(devicePtr));
        }

        [Fact]
        public void SaveScannerCommand_HandlesError_WhenSaveFails()
        {
            var exception = new Exception("Test");
            scannerIdentifier.Setup(x => x.Save(It.IsAny<IntPtr>())).Returns(exception);

            testObject.SaveScannerCommand.Execute(null);

            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("Test"));
        }

        [Fact]
        [SuppressMessage("SonarQube", "S2952:Move this 'Dispose' call into the class' 'Dispose' method",
            Justification = "We're actually testing the 'Dispose' method")]
        public void Dispose_StopsScannerRegistrationListener()
        {
            testObject.Dispose();
            scannerRegistrationListener.Verify(s => s.Stop());
        }

        public void Dispose()
        {
            messenger.Dispose();
            mock.Dispose();
        }
    }
}