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
        private readonly Mock<IScannerMessageFilter> scannerRegistrationMessageFilter;

        public ScannerRegistrationListenerTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ScannerRegistrationListener>();
            scannerRegistrationMessageFilter = mock.FromFactory<IScannerMessageFilterFactory>()
                .Mock(f => f.CreateScannerRegistrationMessageFilter());
        }

        [Fact]
        public void Start_DelegatesToMessageFilterFactory()
        {
            testObject.Start();
            mock.Mock<IScannerMessageFilterFactory>().Verify(x => x.CreateScannerRegistrationMessageFilter());
        }

        [Fact]
        public void Start_DelegatesEnableToScannerMessageFilter()
        {
            testObject.Start();

            scannerRegistrationMessageFilter.Verify(x=>x.Enable(), Times.Once);
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
            testObject.Start();
            scannerRegistrationMessageFilter.Verify(x => x.Disable(), Times.Never);
            
            testObject.Stop();
            scannerRegistrationMessageFilter.Verify(x => x.Disable(), Times.Once);
        }

        [Fact]
        public void Stop_DelegatesToUser32Devices()
        {
            testObject.Start();
            mock.Mock<IUser32Devices>()
                .Verify(x => x.RegisterRawInputDevice(It.Is<RawInputDevice>(d => d.Flags == 1)), Times.Never);

            testObject.Stop();
            mock.Mock<IUser32Devices>()
                .Verify(x => x.RegisterRawInputDevice(It.Is<RawInputDevice>(d => d.Flags == 1)), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}