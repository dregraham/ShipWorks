using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using Moq;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class ScannerIdentifierTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ScannerIdentifier testObject;

        private const string scannerDeviceName = "ScannerName";
        private readonly IntPtr scannerDeviceHandle = (IntPtr) 5;
        private readonly IntPtr anotherScannerDeviceHandle = (IntPtr) 10;
        private const string anotherDeviceName = "AnotherDeviceName";
        private readonly IntPtr anotherDeviceHandle = (IntPtr) 42;

        public ScannerIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ScannerIdentifier>();
        }

        [Fact]
        public void HandleDeviceAdded_ScannerNotSet_WhenAddedDeviceNotScanner()
        {
            mock.Mock<IScannerConfigurationRepository>().Setup(repo => repo.GetScannerName()).Returns(GenericResult.FromSuccess(scannerDeviceName));
            MockGetDeviceName(anotherDeviceHandle, anotherDeviceName);

            testObject.HandleDeviceAdded(anotherDeviceHandle);
            Assert.False(testObject.IsRegisteredScanner(scannerDeviceHandle));
        }

        [Fact]
        public void HandleDeviceAdded_ScannerNotReset_WhenAddedDeviceIsRegisteredScanner_ButScannerAlreadyAdded()
        {
            mock.Mock<IScannerConfigurationRepository>().Setup(repo => repo.GetScannerName()).Returns(GenericResult.FromSuccess(scannerDeviceName));
            MockGetDeviceName(scannerDeviceHandle, scannerDeviceName);
            MockGetDeviceName(anotherScannerDeviceHandle, scannerDeviceName);

            testObject.HandleDeviceAdded(scannerDeviceHandle);
            Assert.True(testObject.IsRegisteredScanner(scannerDeviceHandle));

            testObject.HandleDeviceAdded(anotherScannerDeviceHandle);
            Assert.True(testObject.IsRegisteredScanner(scannerDeviceHandle));
        }

        [Fact]
        public void RemoveScanner_ScannerRemoved_WhenScannerAttached()
        {
            mock.Mock<IScannerConfigurationRepository>().Setup(repo => repo.GetScannerName()).Returns(GenericResult.FromSuccess(scannerDeviceName));
            MockGetDeviceName(scannerDeviceHandle, scannerDeviceName);
            testObject.HandleDeviceAdded(scannerDeviceHandle);

            Assert.True(testObject.IsRegisteredScanner(scannerDeviceHandle));

            testObject.HandleDeviceRemoved(scannerDeviceHandle);
            Assert.False(testObject.IsRegisteredScanner(scannerDeviceHandle));
        }

        [Fact]
        public void RemoveScanner_ScannerNotRemoved_WhenAttachedScannerNotRemovedDevice()
        {
            mock.Mock<IScannerConfigurationRepository>().Setup(repo => repo.GetScannerName()).Returns(GenericResult.FromSuccess(scannerDeviceName));
            MockGetDeviceName(scannerDeviceHandle, scannerDeviceName);

            testObject.HandleDeviceAdded(scannerDeviceHandle);

            testObject.HandleDeviceRemoved(anotherScannerDeviceHandle);
            Assert.True(testObject.IsRegisteredScanner(scannerDeviceHandle));
        }

        [Fact]
        public void RemoveScanner_ScannerStaysDetached_WhenScannerNotAttached()
        {
            mock.Mock<IScannerConfigurationRepository>().Setup(repo => repo.GetScannerName()).Returns(GenericResult.FromSuccess(scannerDeviceName));
            MockGetDeviceName(scannerDeviceHandle, scannerDeviceName);

            testObject.HandleDeviceRemoved(scannerDeviceHandle);
            Assert.False(testObject.IsRegisteredScanner(scannerDeviceHandle));
        }

        [Fact]
        public void Save_IsRegisteredScannerReturnsTrue_WhenSavedDeviceHandleIsPassedIn()
        {
            testObject.Save(anotherScannerDeviceHandle);
            Assert.True(testObject.IsRegisteredScanner(anotherScannerDeviceHandle));
        }

        [Fact]
        public void Save_DeviceNameSentToRepository()
        {
            MockGetDeviceName(scannerDeviceHandle, scannerDeviceName);
            testObject.Save(scannerDeviceHandle);

            mock.Mock<IScannerConfigurationRepository>().Verify(r=>r.SaveScannerName(scannerDeviceName), Times.Once);
        }

        private void MockGetDeviceName(IntPtr handle, string deviceName)
        {
            mock.Mock<IUser32Devices>().Setup(d => d.GetDeviceName(handle)).Returns(deviceName);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}