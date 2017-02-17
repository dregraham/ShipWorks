using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Common;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.SingleScan.ScannerServicePipelines;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class ScannerServiceTest : IDisposable
    {
        readonly AutoMock mock;

        public ScannerServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Enable_DelegatesToMessageFilterFactory()
        {
            var testObject = mock.Create<ScannerService>();
            testObject.Enable();
            mock.Mock<IScannerMessageFilterFactory>().Verify(x => x.CreateRegisteredScannerInputHandler());
        }

        [Fact]
        public void Enable_DelegatesToMessageFilterRegistrar()
        {
            var messageFilter = mock.Create<IScannerMessageFilter>();
            mock.Mock<IScannerMessageFilterFactory>().Setup(x => x.CreateRegisteredScannerInputHandler()).Returns(messageFilter);

            var testObject = mock.Create<ScannerService>();
            testObject.Enable();

            mock.Mock<IWindowsMessageFilterRegistrar>().Verify(x => x.AddMessageFilter(messageFilter));
        }

        [Fact]
        public void Enable_DelegatesToUser32Devices()
        {
            mock.Mock<IMainForm>().SetupGet(x => x.Handle).Returns((IntPtr) 123);

            var testObject = mock.Create<ScannerService>();
            testObject.Enable();

            mock.Mock<IUser32Devices>().Verify(x => x.RegisterRawInputDevice(new RawInputDevice
            {
                UsagePage = 0x01,
                Usage = 0x06,
                Flags = (int) (RawInputDeviceNotificationFlags.Default | RawInputDeviceNotificationFlags.DeviceNotify),
                TargetHandle = (IntPtr) 123,
            }));
        }

        [Fact]
        public void Enable_DoesNotEnableTwice_WhenAlreadyEnabled()
        {
            var testObject = mock.Create<ScannerService>();
            testObject.Enable();
            testObject.Enable();

            mock.Mock<IUser32Devices>().Verify(x => x.RegisterRawInputDevice(It.IsAny<RawInputDevice>()), Times.Once);
        }

        [Fact]
        public void Disable_DelegatesToMessageFilterRegistrar()
        {
            var messageFilter = mock.Create<IScannerMessageFilter>();
            mock.Mock<IScannerMessageFilterFactory>().Setup(x => x.CreateRegisteredScannerInputHandler()).Returns(messageFilter);

            var testObject = mock.Create<ScannerService>();
            testObject.Enable();

            testObject.Disable();

            mock.Mock<IWindowsMessageFilterRegistrar>().Verify(x => x.RemoveMessageFilter(messageFilter));
        }

        [Fact]
        public void Disable_DelegatesToUser32Devices()
        {
            var testObject = mock.Create<ScannerService>();
            testObject.Enable();

            testObject.Disable();

            var expectedDevice = new RawInputDevice
            {
                UsagePage = 0x01,
                Usage = 0x06,
                Flags = (int) RawInputDeviceNotificationFlags.RemoveDevice,
                TargetHandle = (IntPtr) null,
            };
            mock.Mock<IUser32Devices>().Verify(x => x.RegisterRawInputDevice(expectedDevice));
        }

        [Fact]
        public void Disable_DoesNotDisableTwice_WhenAlreadyDisabled()
        {
            var testObject = mock.Create<ScannerService>();
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
            var userSettings = mock.Mock<IUserSettingsEntity>();
            userSettings.SetupGet(x => x.SingleScanSettings)
                .Returns((int) setting);

            mock.Mock<IUserSession>().SetupGet(x => x.Settings).Returns(userSettings.Object);

            mock.Mock<IMainForm>().SetupGet(m => m.InvokeRequired).Returns(false);
            mock.Mock<IMainForm>().SetupGet(m => m.Handle).Returns(IntPtr.Zero);

            var testObject = mock.Create<ScannerService>();
            testObject.InitializeForCurrentSession();

            mock.Mock<IUser32Devices>().Verify(x => x.RegisterRawInputDevice(It.IsAny<RawInputDevice>()));
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotEnableScanning_WhenSettingIsDisabled()
        {
            var user = mock.MockRepository.Create<UserEntity>((long) 1);
            user.SetupGet(x => x.Settings)
                .Returns(new UserSettingsEntity
                {
                    SingleScanSettings = (int) SingleScanSettings.Disabled
                });

            mock.Mock<IUserSession>().SetupGet(x => x.User).Returns(user.Object);

            var testObject = mock.Create<ScannerService>();
            testObject.InitializeForCurrentSession();

            mock.Mock<IUser32Devices>().Verify(x => x.RegisterRawInputDevice(It.IsAny<RawInputDevice>()), Times.Never);
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotEnableScanning_WhenUserIsNull()
        {
            mock.Mock<IUserSession>().SetupGet(x => x.User).Returns((UserEntity) null);

            var testObject = mock.Create<ScannerService>();
            testObject.InitializeForCurrentSession();

            mock.Mock<IUser32Devices>().Verify(x => x.RegisterRawInputDevice(It.IsAny<RawInputDevice>()), Times.Never);
        }

        [Fact]
        public void Constructor_RegistersPipelines()
        {
            var pipeline1 = mock.CreateMock<IScannerServicePipeline>();
            var pipeline2 = mock.CreateMock<IScannerServicePipeline>();

            mock.Provide<IEnumerable<IScannerServicePipeline>>(new[] { pipeline1.Object, pipeline2.Object });

            var testObject = mock.Create<ScannerService>();

            pipeline1.Verify(x => x.Register(testObject));
            pipeline2.Verify(x => x.Register(testObject));
        }

        [Fact]
        public void Dispose_PipelinesAreDisposed()
        {
            var disposable1 = mock.CreateMock<IDisposable>();
            var pipeline1 = mock.CreateMock<IScannerServicePipeline>(m =>
                m.Setup(x => x.Register(It.IsAny<ScannerService>())).Returns(disposable1.Object));

            var disposable2 = mock.CreateMock<IDisposable>();
            var pipeline2 = mock.CreateMock<IScannerServicePipeline>(m =>
                m.Setup(x => x.Register(It.IsAny<ScannerService>())).Returns(disposable2.Object));

            mock.Provide<IEnumerable<IScannerServicePipeline>>(new[] { pipeline1.Object, pipeline2.Object });

            var testObject = mock.Create<ScannerService>();
            testObject.Dispose();

            disposable1.Verify(x => x.Dispose());
            disposable2.Verify(x => x.Dispose());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
