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
using ShipWorks.Data.Model.EntityInterfaces;

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
        private IShortcutEntity shortcut;

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
                .Do(m => shortcut = m.Shortcut)
                .ContinueAfter(messenger.OfType<ProfileAppliedMessage>().Where(m => ((ShippingProfile)m.Sender).Shortcut.Equals(shortcut)), TimeSpan.FromSeconds(5), schedulerProvider.Default,
                    (x, y) => (shortcutMessage: x, profileAppliedMessage: y))
                .Subscribe(m => CollectProfileAppliedShortcutTelemetry(m.shortcutMessage, m.profileAppliedMessage)),

             messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.ApplyWeight))
                .Subscribe(CollectWeightAppliedShortcutTelemetry)
            );
        }

        /// <summary>
        /// Handle telemetry for a profile being applied via shortcut 
        /// </summary>
        private void CollectProfileAppliedShortcutTelemetry(ShortcutMessage shortcutMessage, ProfileAppliedMessage profileAppliedMessage)
        {
            TimeSpan duration = DateTime.UtcNow - shortcutMessage.CreatedDate;
            
            using (ITrackedEvent telemetryEvent = telemetryEventFactory("Shortcuts.Applied"))
            {
                if (profileAppliedMessage == null)
                {
                    telemetryEvent.AddProperty("Shortcuts.Applied.Result", "unknown");
                }
                else
                {
                    telemetryEvent.AddProperty("Shortcuts.Applied.Result", "success");
                }

                CollectShortcutTelemetry(shortcutMessage, telemetryEvent, duration);
            }
        }

        /// <summary>
        /// Handle telemetry for a weight being applied via shortcut 
        /// </summary>
        private void CollectWeightAppliedShortcutTelemetry(ShortcutMessage shortcutMessage)
        {
            TimeSpan duration = DateTime.UtcNow - shortcutMessage.CreatedDate;

            using (ITrackedEvent telemetryEvent = telemetryEventFactory("Shortcuts.Applied"))
            {
                CollectShortcutTelemetry(shortcutMessage, telemetryEvent, duration);
            }
        }

        /// <summary>
        /// Collect telemetry from the shortcut message
        /// </summary>
        private void CollectShortcutTelemetry(ShortcutMessage shortcutMessage, ITrackedEvent telemetryEvent, TimeSpan duration)
        {
            telemetryEvent.AddProperty("Shortcuts.Applied.Duration", duration.ToString());
            telemetryEvent.AddProperty("Shortcuts.Applied.Source", shortcutMessage.Source);
            telemetryEvent.AddProperty("Shortcuts.Applied.Value", shortcutMessage.Value);
            telemetryEvent.AddProperty("Shortcuts.Applied.Action", shortcutMessage.Action);
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
