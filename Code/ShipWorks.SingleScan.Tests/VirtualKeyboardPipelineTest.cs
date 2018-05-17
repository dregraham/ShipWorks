using Autofac.Extras.Moq;
using Interapptive.Shared.IO.Hardware;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Win32.Native;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class VirtualKeyboardPipelineTest
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private readonly TestScheduler scheduler;
        private readonly VirtualKeyboardPipeline testObject;
        private readonly Mock<IMessageHelper> messageHelper;
        private readonly Mock<ICurrentUserSettings> currentUserSettings;
        private readonly Mock<IVirtualKeyboard> virtualKeyboard;

        private readonly ScanMessageBroker scanMessageBroker = new ScanMessageBroker(null, null);

        public VirtualKeyboardPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            currentUserSettings = mock.Mock<ICurrentUserSettings>();
            currentUserSettings.Setup(c => c.ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator)).Returns(true);

            messageHelper = mock.Mock<IMessageHelper>();
            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();

            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(scheduler);
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);

            virtualKeyboard = mock.Mock<IVirtualKeyboard>();

            testObject = mock.Create<VirtualKeyboardPipeline>();
        }

        [Fact]
        public void InitializeForCurrentSession_ShowsPopup_WhenShouldNotify()
        {
            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.Tab
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Barcode, "abcd");
            testMessenger.Send(shortcutMessage);

            scheduler.Start();

            messageHelper.Verify(m => m.ShowBarcodePopup("Tab"));
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotShowPopup_WhenShouldNotifyIsFalse()
        {
            currentUserSettings.Setup(c => c.ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator)).Returns(false);

            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.Tab
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Barcode, "abcd");
            testMessenger.Send(shortcutMessage);

            scheduler.Start();

            messageHelper.Verify(m => m.ShowBarcodePopup(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void InitializeForCurrentSession_DelegatesToVirtualKeyboard_WhenActionIsTab()
        {
            currentUserSettings.Setup(c => c.ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator)).Returns(false);

            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.Tab
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Barcode, "abcd");
            testMessenger.Send(shortcutMessage);

            scheduler.Start();
            virtualKeyboard.Verify(v => v.Send(VirtualKeys.Tab));
        }

        [Fact]
        public void InitializeForCurrentSession_DelegatesToVirtualKeyboard_WhenActionIsEnter()
        {
            currentUserSettings.Setup(c => c.ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator)).Returns(false);

            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.Enter
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Barcode, "abcd");
            testMessenger.Send(shortcutMessage);

            scheduler.Start();
            virtualKeyboard.Verify(v => v.Send(VirtualKeys.Return));
        }
        
        [Fact]
        public void InitializeForCurrentSession_DelegatesToVirtualKeyboard_WhenActionIsEscape()
        {
            currentUserSettings.Setup(c => c.ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator)).Returns(false);

            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.Escape
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Barcode, "abcd");
            testMessenger.Send(shortcutMessage);

            scheduler.Start();
            virtualKeyboard.Verify(v => v.Send(VirtualKeys.Escape));
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotDelegateToVirtualKeyboard_WhenActionIsNotKey()
        {
            currentUserSettings.Setup(c => c.ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator)).Returns(false);

            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.ApplyProfile
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Barcode, "abcd");
            testMessenger.Send(shortcutMessage);

            scheduler.Start();
            virtualKeyboard.Verify(v => v.Send(It.IsAny<VirtualKeys>()), Times.Never);
        }
    }
}
