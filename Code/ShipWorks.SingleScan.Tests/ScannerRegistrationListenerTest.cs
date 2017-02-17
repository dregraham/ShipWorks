using System;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using Moq;
using ShipWorks.Common;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class ScannerRegistrationListenerTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ScannerRegistrationListener testObject;

        public ScannerRegistrationListenerTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ScannerRegistrationListener>();
        }

        [Fact]
        public void Start_DelegatesToMessageFilterFactory()
        {
            testObject.Start();
            mock.Mock<IScannerMessageFilterFactory>().Verify(x => x.CreateScannerRegistrationMessageFilter());
        }

        [Fact]
        public void Start_DelegatesToWindowsMessageFilterRegistrar()
        {
            testObject.Start();
            mock.Mock<IWindowsMessageFilterRegistrar>().Verify(x => x.AddMessageFilter(It.IsAny<IMessageFilter>()));
        }

        [Fact]
        public void Start_DelegatesToUser32Devices()
        {
            testObject.Start();
            mock.Mock<IUser32Devices>().Verify(x => x.RegisterRawInputDevice(It.IsAny<RawInputDevice>()));
        }

        [Fact]
        public void Stop_DelegatesToWindowsMessageFilterRegistrar()
        {
            testObject.Stop();
            mock.Mock<IWindowsMessageFilterRegistrar>().Verify(x => x.RemoveMessageFilter(It.IsAny<IMessageFilter>()));
        }

        [Fact]
        public void Stop_DelegatesToUser32Devices()
        {
            testObject.Stop();
            mock.Mock<IUser32Devices>().Verify(x => x.RegisterRawInputDevice(It.IsAny<RawInputDevice>()));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}