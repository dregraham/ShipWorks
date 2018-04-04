using System;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using Interapptive.Shared.Collections;
using ShipWorks.Shipping.Profiles;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using ShipWorks.Common.IO.KeyboardShortcuts;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Pipeline to show indicator for shortcuts or barcodes being applied
    /// </summary>
    public class ShortcutMessageIndicatorPipeline : IInitializeForCurrentUISession
    {
        private readonly IMessenger messenger;
        private readonly IMessageHelper messageHelper;
        private readonly ISchedulerProvider schedulerProvider;
        private IDisposable subscription;

        private const char BarcodeIconText = (char) 0xf02a;
        private const char KeyboardIconText = (char) 0xf11c;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShortcutMessageIndicatorPipeline(IMessenger messenger, 
            IMessageHelper messageHelper,
            ISchedulerProvider schedulerProvider)
        {
            this.messenger = messenger;
            this.messageHelper = messageHelper;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Initialize the subscriptions
        /// </summary>
        public void InitializeForCurrentSession()
        {
            EndSession();

            subscription = messenger.OfType<ShortcutMessage>()
                            .Where(m => m.AppliesTo(KeyboardShortcutCommand.ApplyProfile))
                            .ContinueAfter(ProfileAppliedSignal, TimeSpan.FromSeconds(1), schedulerProvider.Default,
                                (x, y) => (shortcutMessage: x, profileAppliedMessage: y))
                            .Subscribe(m => ShowProfileAppliedIndicator(m.shortcutMessage, m.profileAppliedMessage));
        }

        /// <summary>
        /// Signal that the ShortcutMessages associated profile has been applied
        /// </summary>
        private IObservable<ProfileAppliedMessage> ProfileAppliedSignal(ShortcutMessage shortcutMessage) =>
             messenger.OfType<ProfileAppliedMessage>()
                .Where(profileAppliedMessage => ((ShippingProfile) profileAppliedMessage.Sender).Shortcut.Equals(shortcutMessage.Shortcut));
        
        /// <summary>
        /// Show indicator
        /// </summary>
        private void ShowProfileAppliedIndicator(ShortcutMessage shortcutMessage, ProfileAppliedMessage profileAppliedMessage)
        {
            if (profileAppliedMessage != null)
            {
                string action = shortcutMessage.Trigger == ShortcutTriggerType.Hotkey ? shortcutMessage.Value : "Barcode";
                string name = (profileAppliedMessage?.Sender as IShippingProfile)?.ShippingProfileEntity?.Name ?? string.Empty;
                char fontAwesomeIcon = shortcutMessage.Trigger == ShortcutTriggerType.Hotkey ? KeyboardIconText : BarcodeIconText;

                messageHelper.ShowPopup($"{action}: {name}", fontAwesomeIcon);
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
