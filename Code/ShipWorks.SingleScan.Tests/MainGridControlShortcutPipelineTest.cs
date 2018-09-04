using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Settings;
using ShipWorks.Users;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.SingleScan.Tests
{
    public class MainGridControlShortcutPipelineTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private readonly Mock<IMainGridControl> mainGridControl;
        private readonly MainGridControlShortcutPipeline testObject;
        private readonly TestScheduler scheduler;
        
        public MainGridControlShortcutPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);
            mainGridControl = mock.CreateMock<IMainGridControl>();

            testObject = mock.Create<MainGridControlShortcutPipeline>();

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();
            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(scheduler);
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);

            testObject.Register(mainGridControl.Object);

            SetUIMode(UIMode.Batch);
        }

        [Fact]
        public void ClearQuickSearch_ClearsQuickSearch()
        {
            var shortcut = new ShortcutEntity() { Action = KeyboardShortcutCommand.ClearQuickSearch };
            var message = new ShortcutMessage(this, shortcut, ShortcutTriggerType.Barcode, string.Empty);
            testMessenger.Send(message);

            scheduler.Start();

            mainGridControl.Verify(m => m.ClearSearch(), Times.Once());
        }

        [Fact]
        public void SetQuickSearchFocus_SetsQuickSearchFocus()
        {
            var shortcut = new ShortcutEntity() { Action = KeyboardShortcutCommand.FocusQuickSearch };
            var message = new ShortcutMessage(this, shortcut, ShortcutTriggerType.Barcode, string.Empty);
            testMessenger.Send(message);

            scheduler.Start();

            mainGridControl.Verify(m => m.FocusSearch(), Times.Once());
        }

        [Theory]
        [InlineData(KeyboardShortcutCommand.ClearQuickSearch, "Barcode: Clear Quick Search")]
        [InlineData(KeyboardShortcutCommand.FocusQuickSearch, "Barcode: Focus Quick Search")]
        public void BarcodeMessage_ShowsMessageIndicator_WhenShouldShowNotificationIsTrue(KeyboardShortcutCommand action, string expectedIndicatorText)
        {
            mock.Mock<ICurrentUserSettings>()
                .Setup(s => s.ShouldShowNotification(It.IsAny<UserConditionalNotificationType>()))
                .Returns(true);

            var shortcut = new ShortcutEntity() { Action = action };
            var message = new ShortcutMessage(this, shortcut, ShortcutTriggerType.Barcode, string.Empty);
            testMessenger.Send(message);

            scheduler.Start();

            mock.Mock<IMessageHelper>().Verify(m => m.ShowBarcodePopup(expectedIndicatorText), Times.Once);
        }

        [Fact]
        public void BarcodeMessage_DoesNotShowIndicator_WhenNotInBatchUIMode()
        {
            SetUIMode(UIMode.OrderLookup);

            mock.Mock<ICurrentUserSettings>()
                .Setup(s => s.ShouldShowNotification(It.IsAny<UserConditionalNotificationType>()))
                .Returns(true);

            var shortcut = new ShortcutEntity() { Action = KeyboardShortcutCommand.FocusQuickSearch };
            var message = new ShortcutMessage(this, shortcut, ShortcutTriggerType.Barcode, string.Empty);
            testMessenger.Send(message);

            scheduler.Start();

            mock.Mock<IMessageHelper>().Verify(m => m.ShowBarcodePopup(AnyString), Times.Never);
        }

        [Theory]
        [InlineData(KeyboardShortcutCommand.ClearQuickSearch, "The Hotkey: Clear Quick Search")]
        [InlineData(KeyboardShortcutCommand.FocusQuickSearch, "The Hotkey: Focus Quick Search")]
        public void KeyboardShortcutMessage_ShowsMessageIndicator_WhenShouldShowNotificationIsTrue(KeyboardShortcutCommand action, string expectedIndicatorText)
        {
            mock.Mock<ICurrentUserSettings>()
                .Setup(s => s.ShouldShowNotification(It.IsAny<UserConditionalNotificationType>()))
                .Returns(true);

            var shortcut = new ShortcutEntity() { Action = action };
            var message = new ShortcutMessage(this, shortcut, ShortcutTriggerType.Hotkey, "The Hotkey");
            testMessenger.Send(message);

            scheduler.Start();

            mock.Mock<IMessageHelper>().Verify(m => m.ShowKeyboardPopup(expectedIndicatorText), Times.Once);
        }

        [Fact]
        public void ShortcutMessage_ShowsNoMessageIndicator_WhenShouldShowNotificationIsFalse()
        {
            mock.Mock<ICurrentUserSettings>()
                .Setup(s => s.ShouldShowNotification(It.IsAny<UserConditionalNotificationType>()))
                .Returns(false);

            var shortcut = new ShortcutEntity() { Action = KeyboardShortcutCommand.FocusQuickSearch };
            var message = new ShortcutMessage(this, shortcut, ShortcutTriggerType.Hotkey, "The Hotkey");
            testMessenger.Send(message);

            scheduler.Start();

            mock.Mock<IMessageHelper>().Verify(m => m.ShowKeyboardPopup(AnyString), Times.Never);
        }

        private void SetUIMode(UIMode uiMode)
        {
            mock.Mock<IUserSession>()
                .SetupGet(s => s.User)
                .Returns(new UserEntity
                {
                    Settings = new UserSettingsEntity
                    {
                        UIMode = uiMode
                    }
                });
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}