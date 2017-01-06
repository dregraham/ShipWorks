using System;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using Moq;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Common;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class ScannerServiceTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ScannerService testObject;

        public ScannerServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ScannerService>();
        }

        [Fact]
        public void CurrentScannerState_DelegatesToScannerIdentifier()
        {
            var result = testObject.CurrentScannerState;

            mock.Mock<IScannerIdentifier>().VerifyGet(x => x.ScannerState);
        }

        [Theory]
        [InlineData(ScannerState.Attached)]
        [InlineData(ScannerState.NotRegistered)]
        public void CurrentScannerState_ReturnsValueFromScannerIdentifier(ScannerState value)
        {
            mock.Mock<IScannerIdentifier>().SetupGet(x => x.ScannerState).Returns(value);

            Assert.Equal(value, testObject.CurrentScannerState);
        }

        [Fact]
        public void Enable_DelegatesToMessageFilterFactory()
        {
            testObject.Enable();
            mock.Mock<IScannerMessageFilterFactory>().Verify(x => x.CreateMessageFilter());
        }

        [Fact]
        public void Enable_DelegatesToMessageFilterRegistrar()
        {
            var messageFilter = mock.Create<IScannerMessageFilter>();
            mock.Mock<IScannerMessageFilterFactory>().Setup(x => x.CreateMessageFilter()).Returns(messageFilter);

            testObject.Enable();

            mock.Mock<IWindowsMessageFilterRegistrar>().Verify(x => x.AddMessageFilter(messageFilter));
        }

        [Fact]
        public void Enable_DelegatesToUser32Devices()
        {
            mock.Mock<IWin32Window>().SetupGet(x => x.Handle).Returns((IntPtr) 123);

            testObject.Enable();

            mock.Mock<IUser32Devices>().Verify(x => x.RegisterRawInputDevice(new RawInputDevice
            {
                UsagePage = 0x01,
                Usage = 0x06,
                Flags = (int) (RawInputDeviceNotificationFlags.DEFAULT | RawInputDeviceNotificationFlags.DEVNOTIFY),
                TargetHandle = (IntPtr) 123,
            }));
        }

        [Fact]
        public void Enable_DoesNotEnableTwice_WhenAlreadyEnabled()
        {
            testObject.Enable();
            testObject.Enable();

            mock.Mock<IUser32Devices>().Verify(x => x.RegisterRawInputDevice(It.IsAny<RawInputDevice>()), Times.Once);
        }

        [Fact]
        public void Disable_DelegatesToMessageFilterRegistrar()
        {
            var messageFilter = mock.Create<IScannerMessageFilter>();
            mock.Mock<IScannerMessageFilterFactory>().Setup(x => x.CreateMessageFilter()).Returns(messageFilter);

            testObject.Enable();

            testObject.Disable();

            mock.Mock<IWindowsMessageFilterRegistrar>().Verify(x => x.RemoveMessageFilter(messageFilter));
        }

        [Fact]
        public void Disable_DelegatesToUser32Devices()
        {
            testObject.Enable();

            testObject.Disable();

            var expectedDevice = new RawInputDevice
            {
                UsagePage = 0x01,
                Usage = 0x06,
                Flags = (int) RawInputDeviceNotificationFlags.REMOVE,
                TargetHandle = (IntPtr) null,
            };
            mock.Mock<IUser32Devices>().Verify(x => x.RegisterRawInputDevice(expectedDevice));
        }

        [Fact]
        public void Disable_DoesNotDisableTwice_WhenAlreadyDisabled()
        {
            testObject.Enable();

            testObject.Disable();
            testObject.Disable();

            mock.Mock<IWindowsMessageFilterRegistrar>().Verify(x => x.RemoveMessageFilter(It.IsAny<IScannerMessageFilter>()), Times.Once);
        }

        [Theory]
        [InlineData(SingleScanSettings.AutoPrint)]
        [InlineData(SingleScanSettings.Scan)]
        public void InitializeForCurrentSession_EnablesScanning_WhenSettingIsNotDisabled(SingleScanSettings setting)
        {
            var user = mock.MockRepository.Create<UserEntity>((long) 1);
            user.SetupGet(x => x.Settings)
                .Returns(new UserSettingsEntity
                {
                    SingleScanSettings = (int) setting
                });

            mock.Mock<IUserSession>().SetupGet(x => x.User).Returns(user.Object);

            testObject.InitializeForCurrentSession();

            mock.Mock<IUser32Devices>().Verify(x => x.RegisterRawInputDevice(It.IsAny<RawInputDevice>()));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
