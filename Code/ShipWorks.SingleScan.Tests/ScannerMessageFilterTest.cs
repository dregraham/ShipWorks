using System;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.Win32.Native;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class ScannerMessageFilterTest : IDisposable
    {
        readonly AutoMock mock;
        private ScannerMessageFilter testObject;

        public ScannerMessageFilterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ScannerMessageFilter>();
        }

        [Fact]
        public void PreFilterMessage_ReturnsFalse_WhenInputIsNotScanner()
        {
            var message = new Message();
            var result = testObject.PreFilterMessage(ref message);
            Assert.False(result);
        }

        [Fact]
        public void PreFilterMessage_DelegatesToScannerIdentifierAdded_WhenInputChangeIsRecieved()
        {
            var message = new Message
            {
                Msg = (int) WindowsMessage.INPUT_DEVICE_CHANGE,
                WParam = (IntPtr) 1,
                LParam = (IntPtr) 1234,
            };

            testObject.PreFilterMessage(ref message);

            mock.Mock<IScannerIdentifier>().Verify(x => x.HandleDeviceAdded(1234));
        }

        [Fact]
        public void PreFilterMessage_DelegatesToScannerIdentifierRemoved_WhenInputChangeIsRecieved()
        {
            var message = new Message
            {
                Msg = (int) WindowsMessage.INPUT_DEVICE_CHANGE,
                WParam = (IntPtr) 0,
                LParam = (IntPtr) 1234,
            };

            testObject.PreFilterMessage(ref message);

            mock.Mock<IScannerIdentifier>().Verify(x => x.HandleDeviceRemoved(1234));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void PreFilterMessage_ReturnsFalse_WhenInputChangeIsRecieved(int input)
        {
            var message = new Message
            {
                Msg = (int) WindowsMessage.INPUT_DEVICE_CHANGE,
                WParam = (IntPtr) input,
                LParam = (IntPtr) 1234,
            };

            var result = testObject.PreFilterMessage(ref message);

            Assert.False(result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
