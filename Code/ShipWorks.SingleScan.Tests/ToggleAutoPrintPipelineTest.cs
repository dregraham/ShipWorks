using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Users;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.SingleScan.Tests
{
    public class ToggleAutoPrintPipelineTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private readonly TestScheduler scheduler;

        public ToggleAutoPrintPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();
            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(scheduler);
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);
        }

        [Theory]
        [InlineData(SingleScanSettings.AutoPrint, "Ctrl+Shift+A: Auto Print OFF")]
        [InlineData(SingleScanSettings.Scan, "Ctrl+Shift+A: Auto Print ON")]
        public void ToggleAutoPrint_ShowHotKeyIndicator_WhenSingleScanIsEnabled(SingleScanSettings setting, string expectedText)
        {
            mock.Mock<IUserSession>().SetupGet(s => s.User)
                .Returns(new UserEntity()
                {
                    Settings = new UserSettingsEntity()
                    {
                        SingleScanSettings = (int) setting
                    }
                });

            mock.Mock<ICurrentUserSettings>()
                .Setup(s => s.ShouldShowNotification(It.IsAny<UserConditionalNotificationType>()))
                .Returns(true);

            var testObject = mock.Create<ToggleAutoPrintPipeline>();

            testObject.InitializeForCurrentSession();

            var shortcut = new ShortcutEntity() { Action = KeyboardShortcutCommand.ToggleAutoPrint };

            var message = new ShortcutMessage(this, shortcut, ShortcutTriggerType.Hotkey, "Ctrl+Shift+A");
            testMessenger.Send(message);

            scheduler.Start();

            mock.Mock<IMessageHelper>().Verify(m => m.ShowKeyboardPopup(expectedText));
        }

        [Theory]
        [InlineData(SingleScanSettings.AutoPrint, SingleScanSettings.Scan)]
        [InlineData(SingleScanSettings.Scan, SingleScanSettings.AutoPrint)]
        [InlineData(SingleScanSettings.Disabled, SingleScanSettings.Disabled)]
        public void ToggleAutoPrint_SetsSingleScanSettingCorrectly(SingleScanSettings setting, SingleScanSettings expectedResult)
        {
            var userSettingsEntity = new UserSettingsEntity()
            {
                SingleScanSettings = (int) setting
            };

            mock.Mock<IUserSession>().SetupGet(s => s.User)
                .Returns(new UserEntity()
                {
                    Settings = userSettingsEntity
                });

            var testObject = mock.Create<ToggleAutoPrintPipeline>();

            testObject.InitializeForCurrentSession();

            var shortcut = new ShortcutEntity() { Action = KeyboardShortcutCommand.ToggleAutoPrint };

            var message = new ShortcutMessage(this, shortcut, ShortcutTriggerType.Hotkey, "Ctrl+Shift+A");
            testMessenger.Send(message);

            scheduler.Start();

            Assert.Equal(expectedResult, (SingleScanSettings) userSettingsEntity.SingleScanSettings);
        }

        [Theory]
        [InlineData(SingleScanSettings.AutoPrint, "Barcode: Auto Print OFF")]
        [InlineData(SingleScanSettings.Scan, "Barcode: Auto Print ON")]
        public void ToggleAutoPrint_ShowsBarcodeIndicator_WhenSingleScanEnabled(SingleScanSettings setting, string expectedText)
        {
            mock.Mock<IUserSession>().SetupGet(s => s.User)
                .Returns(new UserEntity()
                {
                    Settings = new UserSettingsEntity()
                    {
                        SingleScanSettings = (int) setting
                    }
                });

            mock.Mock<ICurrentUserSettings>()
                .Setup(s => s.ShouldShowNotification(It.IsAny<UserConditionalNotificationType>()))
                .Returns(true);

            var testObject = mock.Create<ToggleAutoPrintPipeline>();

            testObject.InitializeForCurrentSession();

            var shortcut = new ShortcutEntity() { Action = KeyboardShortcutCommand.ToggleAutoPrint };

            var message = new ShortcutMessage(this, shortcut, ShortcutTriggerType.Barcode, string.Empty);
            testMessenger.Send(message);

            scheduler.Start();

            mock.Mock<IMessageHelper>().Verify(m => m.ShowBarcodePopup(expectedText));
        }

        [Theory]
        [InlineData(ShortcutTriggerType.Hotkey)]
        [InlineData(ShortcutTriggerType.Barcode)]
        public void ToggleAutoPrint_NoIndicatorShown_WhenShouldShowNotificationReturnsFalse(ShortcutTriggerType triggerType)
        {
            mock.Mock<IUserSession>().SetupGet(s => s.User)
                .Returns(new UserEntity()
                {
                    Settings = new UserSettingsEntity()
                    {
                        SingleScanSettings = (int) SingleScanSettings.AutoPrint
                    }
                });

            mock.Mock<ICurrentUserSettings>()
                .Setup(s => s.ShouldShowNotification(It.IsAny<UserConditionalNotificationType>()))
                .Returns(false);

            var testObject = mock.Create<ToggleAutoPrintPipeline>();

            testObject.InitializeForCurrentSession();

            var shortcut = new ShortcutEntity() { Action = KeyboardShortcutCommand.ToggleAutoPrint };

            var message = new ShortcutMessage(this, shortcut, triggerType, string.Empty);
            testMessenger.Send(message);

            scheduler.Start();

            mock.Mock<IMessageHelper>().Verify(m => m.ShowBarcodePopup(AnyString), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}