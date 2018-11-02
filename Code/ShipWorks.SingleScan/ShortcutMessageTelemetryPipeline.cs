using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Pipeline to track shortcut telemetry
    /// </summary>
    public class ShortcutMessageTelemetryPipeline : IInitializeForCurrentUISession
    {
        private readonly IMessenger messenger;
        private readonly Func<string, ITrackedEvent> telemetryEventFactory;
        private readonly ISchedulerProvider schedulerProvider;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShortcutMessageTelemetryPipeline(IMessenger messenger,
            Func<string, ITrackedEvent> telemetryEventFactory,
            ISchedulerProvider schedulerProvider)
        {
            this.messenger = messenger;
            this.telemetryEventFactory = telemetryEventFactory;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Initialize the subscriptions
        /// </summary>
        public void InitializeForCurrentSession()
        {
            EndSession();

            subscription = new CompositeDisposable(
            messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.ApplyProfile))
                .ContinueAfter(ProfileAppliedSignal, TimeSpan.FromSeconds(5), schedulerProvider.Default,
                    (x, y) => (shortcutMessage: x, profileAppliedMessage: y))
                .Subscribe(m => CollectTelemetryWithResult(m.shortcutMessage, m.profileAppliedMessage)),

            messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.ApplyWeight) ||
                            m.AppliesTo(KeyboardShortcutCommand.Enter) ||
                            m.AppliesTo(KeyboardShortcutCommand.Tab) ||
                            m.AppliesTo(KeyboardShortcutCommand.Escape) ||
                            m.AppliesTo(KeyboardShortcutCommand.ToggleAutoPrint) ||
                            m.AppliesTo(KeyboardShortcutCommand.ClearQuickSearch) ||
                            m.AppliesTo(KeyboardShortcutCommand.FocusQuickSearch))
                .Subscribe(CollectTelemetryWithoutResult),

            messenger.OfType<ShortcutMessage>()
                     .Where(m => m.AppliesTo(KeyboardShortcutCommand.CreateLabel))
                     .ContinueAfter(LabelCreatedSignal, TimeSpan.FromSeconds(5), schedulerProvider.Default,
                                    (x, y) => (shortcutMessage: x, processShipmentMessage: y))
                     .Subscribe(m => CollectTelemetryWithResult(m.shortcutMessage, m.processShipmentMessage))
            );
        }

        /// <summary>
        /// Signal that the ShortcutMessages associated profile has been applied
        /// </summary>
        private IObservable<ProfileAppliedMessage> ProfileAppliedSignal(ShortcutMessage shortcutMessage) =>
             messenger.OfType<ProfileAppliedMessage>()
                .Where(profileAppliedMessage => ((IShippingProfile) profileAppliedMessage.Sender).Shortcut.Equals(shortcutMessage.Shortcut));

        /// <summary>
        /// Signal that the shipment is being processed
        /// </summary>
        private IObservable<ProcessShipmentsMessage> LabelCreatedSignal(ShortcutMessage shortcutMessage) => messenger.OfType<ProcessShipmentsMessage>();

        /// <summary>
        /// Handle telemetry for a profile being applied via shortcut
        /// </summary>
        private void CollectTelemetryWithResult(ShortcutMessage shortcutMessage, IShipWorksMessage actionMessage)
        {
            using (ITrackedEvent telemetryEvent = telemetryEventFactory("Shortcuts.Applied"))
            {
                CollectShortcutTelemetry(shortcutMessage, telemetryEvent);

                telemetryEvent.AddProperty("Shortcuts.Applied.Result", actionMessage == null || actionMessage.MessageId == Guid.Empty ? "Unknown" : "Success");
            }
        }

        /// <summary>
        /// Handle telemetry for a weight being applied via shortcut
        /// </summary>
        private void CollectTelemetryWithoutResult(ShortcutMessage shortcutMessage)
        {
            using (ITrackedEvent telemetryEvent = telemetryEventFactory("Shortcuts.Applied"))
            {
                CollectShortcutTelemetry(shortcutMessage, telemetryEvent);
            }
        }

        /// <summary>
        /// Collect telemetry from the shortcut message
        /// </summary>
        private void CollectShortcutTelemetry(ShortcutMessage shortcutMessage, ITrackedEvent telemetryEvent)
        {
            telemetryEvent.AddMetric("Shortcuts.Applied.ResponseTimeInMilliseconds", (DateTime.UtcNow - shortcutMessage.CreatedDate).TotalMilliseconds);
            telemetryEvent.AddProperty("Shortcuts.Applied.Source", EnumHelper.GetDescription(shortcutMessage.Trigger));
            telemetryEvent.AddProperty("Shortcuts.Applied.Value", shortcutMessage.Value);
            telemetryEvent.AddProperty("Shortcuts.Applied.Action", GetShortcutMessageAction(shortcutMessage));
        }

        /// <summary>
        /// Get the shortcutMessage action
        /// </summary>
        private string GetShortcutMessageAction(ShortcutMessage shortcutMessage)
        {
            switch (shortcutMessage.Shortcut.Action)
            {
                case KeyboardShortcutCommand.ApplyWeight:
                    return "ScaleReading";
                case KeyboardShortcutCommand.ApplyProfile:
                    return "ShippingProfile";
                case KeyboardShortcutCommand.CreateLabel:
                    return "LabelPrinted";
                case KeyboardShortcutCommand.Enter:
                case KeyboardShortcutCommand.Tab:
                case KeyboardShortcutCommand.Escape:
                    return $"Simulate {shortcutMessage.Shortcut.Action} key press";
                default:
                    return EnumHelper.GetDescription(shortcutMessage.Shortcut.Action);
            }
        }

        /// <summary>
        /// End the session
        /// </summary>
        public void Dispose() => EndSession();

        /// <summary>
        /// End the session
        /// </summary>
        public void EndSession() => subscription?.Dispose();
    }
}
