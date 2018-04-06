using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Metrics;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using Interapptive.Shared.Collections;
using ShipWorks.Shipping.Profiles;
using Interapptive.Shared.Threading;
using System.Reactive.Disposables;
using ShipWorks.Common.IO.KeyboardShortcuts;
using Interapptive.Shared.Utility;
using ShipWorks.Messaging.Messages.SingleScan;

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
                .Subscribe(m => CollectProfileAppliedShortcutTelemetry(m.shortcutMessage, m.profileAppliedMessage)),

             messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.ApplyWeight))
                .Subscribe(CollectWeightAppliedShortcutTelemetry)
            );
        }

        /// <summary>
        /// Signal that the ShortcutMessages associated profile has been applied
        /// </summary>
        private IObservable<ProfileAppliedMessage> ProfileAppliedSignal(ShortcutMessage shortcutMessage) =>
             messenger.OfType<ProfileAppliedMessage>()
                .Where(profileAppliedMessage => ((ShippingProfile) profileAppliedMessage.Sender).Shortcut.Equals(shortcutMessage.Shortcut));
        
        /// <summary>
        /// Handle telemetry for a profile being applied via shortcut 
        /// </summary>
        private void CollectProfileAppliedShortcutTelemetry(ShortcutMessage shortcutMessage, ProfileAppliedMessage profileAppliedMessage)
        {
            using (ITrackedEvent telemetryEvent = telemetryEventFactory("Shortcuts.Applied"))
            {
                CollectShortcutTelemetry(shortcutMessage, telemetryEvent);

                if (profileAppliedMessage == null)
                {
                    telemetryEvent.AddProperty("Shortcuts.Applied.Result", "Unknown");
                }
                else
                {
                    telemetryEvent.AddProperty("Shortcuts.Applied.Result", "Success");
                }
            }
        }

        /// <summary>
        /// Handle telemetry for a weight being applied via shortcut 
        /// </summary>
        private void CollectWeightAppliedShortcutTelemetry(ShortcutMessage shortcutMessage)
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
            telemetryEvent.AddProperty("Shortcuts.Applied.Source", GetShortcutMessageSource(shortcutMessage));
            telemetryEvent.AddProperty("Shortcuts.Applied.Value", shortcutMessage.Value);
            telemetryEvent.AddProperty("Shortcuts.Applied.Action", GetShortcutMessageAction(shortcutMessage));
        }
        
        /// <summary>
        /// Get the shortcutMessage source
        /// </summary>
        private string GetShortcutMessageSource(ShortcutMessage shortcutMessage)
        {
            if (shortcutMessage.Sender is KeyboardShortcutKeyFilter)
            {
                return "Keyboard";
            }
            else if (shortcutMessage.Sender is ScanMessageBroker)
            {
                return "Barcode";
            }

            return shortcutMessage.Sender.GetType().ToString();
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
