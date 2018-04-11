using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class ShortcutMessageTelemetryPipelineTest
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private readonly TestScheduler scheduler;
        private readonly ShortcutMessageTelemetryPipeline testObject;
        private readonly Mock<ITrackedEvent> telemetryEvent;

        private readonly ScanMessageBroker scanMessageBroker = new ScanMessageBroker(null, null);

        public ShortcutMessageTelemetryPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();

            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(scheduler);
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);

            telemetryEvent = mock.Mock<ITrackedEvent>();

            Mock<Func<string, ITrackedEvent>> telemetryEventFactory = mock.MockFunc<string, ITrackedEvent>();
            telemetryEventFactory.Setup(t => t("Shortcuts.Applied")).Returns(telemetryEvent);

            testObject = mock.Create<ShortcutMessageTelemetryPipeline>();
        }

        [Fact]
        public void InitializeForCurrentSession_CollectsTelemetryData_WhenShortcutActionIsApplyWeight()
        {
            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.ApplyWeight
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Barcode,"abcd");
            testMessenger.Send(shortcutMessage);

            scheduler.Start();

            telemetryEvent.Verify(t => t.AddMetric("Shortcuts.Applied.ResponseTimeInMilliseconds", It.IsAny<double>()));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Source", "Barcode"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Value", "abcd"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Action", "ScaleReading"));
        }

        [Fact]
        public void InitializeForCurrentSession_CollectsTelemetryData_WhenShortcutActionIsApplyProfileAndProfileAppliedMessageIsSent()
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

            testMessenger.Send(new ProfileAppliedMessage(profile, null, null));

            scheduler.Start();

            telemetryEvent.Verify(t => t.AddMetric("Shortcuts.Applied.ResponseTimeInMilliseconds", It.IsAny<double>()));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Result", "Success"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Source", "Barcode"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Value", "abcd"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Action", "ShippingProfile"));
        }


        [Fact]
        public void InitializeForCurrentSession_CollectsTelemetryData_WhenShortcutActionIsApplyProfileAndProfileAppliedMessageIsNotSent()
        {
            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.ApplyProfile
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Barcode, "abcd");
            testMessenger.Send(shortcutMessage);

            scheduler.Start();

            telemetryEvent.Verify(t => t.AddMetric("Shortcuts.Applied.ResponseTimeInMilliseconds", It.IsAny<double>()));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Result", "Unknown"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Source", "Barcode"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Value", "abcd"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Action", "ShippingProfile"));
        }

        [Fact]
        public void InitializeForCurrentSession_CollectsTelemetryData_WhenShortcutActionIsApplyProfileAndProfileAppliedMessageIsSentWithDifferentShortcut()
        {
            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.ApplyProfile
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Barcode, "abcd");
            testMessenger.Send(shortcutMessage);

            ShippingProfile profile = mock.Create<ShippingProfile>();
            profile.Shortcut = new ShortcutEntity();

            testMessenger.Send(new ProfileAppliedMessage(profile, null, null));

            scheduler.Start();

            telemetryEvent.Verify(t => t.AddMetric("Shortcuts.Applied.ResponseTimeInMilliseconds", It.IsAny<double>()));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Result", "Unknown"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Source", "Barcode"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Value", "abcd"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Action", "ShippingProfile"));
        }
        
        [Fact]
        public void InitializeForCurrentSession_CollectsTelemetryData_WhenShortcutActionIsCreateLabelAndProcessShipmentMessageIsSent()
        {
            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.CreateLabel
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Barcode, "abcd");
            testMessenger.Send(shortcutMessage);

            ShippingProfile profile = mock.Create<ShippingProfile>();
            profile.Shortcut = shortcut;

            testMessenger.Send(new ProcessShipmentsMessage(this, new[] { new ShipmentEntity() }, new[] { new ShipmentEntity() }, null));

            scheduler.Start();

            telemetryEvent.Verify(t => t.AddMetric("Shortcuts.Applied.ResponseTimeInMilliseconds", It.IsAny<double>()));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Result", "Success"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Source", "Barcode"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Value", "abcd"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Action", "LabelPrinted"));
        }


        [Fact]
        public void InitializeForCurrentSession_CollectsTelemetryData_WhenShortcutActionIsCreateLabelAndProcessShipmentMessageIsNotSent()
        {
            testObject.InitializeForCurrentSession();

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Action = KeyboardShortcutCommand.CreateLabel
            };

            ShortcutMessage shortcutMessage = new ShortcutMessage(scanMessageBroker, shortcut, ShortcutTriggerType.Barcode, "abcd");
            testMessenger.Send(shortcutMessage);

            scheduler.Start();

            telemetryEvent.Verify(t => t.AddMetric("Shortcuts.Applied.ResponseTimeInMilliseconds", It.IsAny<double>()));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Result", "Unknown"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Source", "Barcode"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Value", "abcd"));
            telemetryEvent.Verify(t => t.AddProperty("Shortcuts.Applied.Action", "LabelPrinted"));
        }
    }
}
