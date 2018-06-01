using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class ShortcutMessageIndicatorPipelineTest
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private readonly TestScheduler scheduler;
        private readonly ApplyProfileShortcutMessageIndicatorPipeline testObject;
        private readonly Mock<IMessageHelper> messageHelper;
        private readonly Mock<ICurrentUserSettings> currentUserSettings;

        private readonly ScanMessageBroker scanMessageBroker = new ScanMessageBroker(null, null);

        public ShortcutMessageIndicatorPipelineTest()
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
            
            testObject = mock.Create<ApplyProfileShortcutMessageIndicatorPipeline>();
        }

        [Fact]
        public void InitializeForCurrentSession_ShowsPopup_WhenProfileAppliedMessageIsNotNull()
        {
            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.ApplyProfile
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Barcode, "abcd");
            testMessenger.Send(shortcutMessage);

            ShippingProfile profile = mock.Create<ShippingProfile>();
            profile.Shortcut = shortcut;
            profile.ShippingProfileEntity = new ShippingProfileEntity() { Name = "FooBar" };

            testMessenger.Send(new ProfileAppliedMessage(profile, null, null));

            scheduler.Start();

            messageHelper.Verify(m => m.ShowBarcodePopup("Barcode: FooBar"));
        }

        [Fact]
        public void InitializeForCurrentSession_ShowsPopupWithKeyboardIcon_WhenShortcutMessageTriggerIsHotkey()
        {
            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.ApplyProfile
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Hotkey, "F5");
            testMessenger.Send(shortcutMessage);

            ShippingProfile profile = mock.Create<ShippingProfile>();
            profile.Shortcut = shortcut;
            profile.ShippingProfileEntity = new ShippingProfileEntity() { Name = "FooBar" };

            testMessenger.Send(new ProfileAppliedMessage(profile, null, null));

            scheduler.Start();

            messageHelper.Verify(m => m.ShowKeyboardPopup("F5: FooBar"));
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotShowPopupWithKeyboardIcon_WhenShortcutMessageTriggerIsHotkey()
        {
            currentUserSettings.Setup(c => c.ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator)).Returns(false);

            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.ApplyProfile
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Hotkey, "F5");
            testMessenger.Send(shortcutMessage);

            ShippingProfile profile = mock.Create<ShippingProfile>();
            profile.Shortcut = shortcut;
            profile.ShippingProfileEntity = new ShippingProfileEntity() { Name = "FooBar" };

            testMessenger.Send(new ProfileAppliedMessage(profile, null, null));

            scheduler.Start();

            messageHelper.Verify(m => m.ShowKeyboardPopup(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotShowPopup_WhenProfileAppliedMessageIsNull()
        {
            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.ApplyProfile
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Barcode, "abcd");
            testMessenger.Send(shortcutMessage);
                        
            scheduler.Start();

            messageHelper.Verify(m => m.ShowBarcodePopup(It.IsAny<string>()), Times.Never);
            messageHelper.Verify(m => m.ShowKeyboardPopup(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotShowPopup_WhenShortCutIsApplyWeight()
        {
            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.ApplyWeight
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Hotkey, "F5");
            testMessenger.Send(shortcutMessage);

            scheduler.Start();

            messageHelper.Verify(m => m.ShowBarcodePopup(It.IsAny<string>()), Times.Never);
            messageHelper.Verify(m => m.ShowKeyboardPopup(It.IsAny<string>()), Times.Never);
        }
    }
}
