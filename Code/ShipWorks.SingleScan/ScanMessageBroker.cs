using System;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Messaging.Messages.SingleScan;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Broker ScanMessages
    /// </summary>
    public class ScanMessageBroker : IInitializeForCurrentUISession
    {
        private readonly IMessenger messenger;
        private readonly IShortcutManager shortcutManager;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanMessageBroker(IMessenger messenger, IShortcutManager shortcutManager)
        {
            this.messenger = messenger;
            this.shortcutManager = shortcutManager;
        }

        /// <summary>
        /// Initialize the subscription
        /// </summary>
        public void InitializeForCurrentSession()
        {
            subscription = messenger.OfType<ScanMessage>()
                .Subscribe(HandleScanMessage);
        }

        /// <summary>
        /// Handle the ScanMessage
        /// </summary>
        private void HandleScanMessage(ScanMessage message)
        {
            IShortcutEntity shortcut = shortcutManager.Shortcuts.FirstOrDefault(s => s.Barcode == message.ScannedText);
            if (shortcut != null)
            {
                messenger.Send(new ShortcutMessage(this, shortcut));
            }
            else
            {
                // at this point the message doesnt match any shortcut so 
                // we assume its a SingleScanMessage
                messenger.Send(new SingleScanMessage(this, message));
            }
        }

        /// <summary>
        /// Dispose the subscription
        /// </summary>
        public void EndSession() => subscription?.Dispose();

        /// <summary>
        /// Dispose the subscription
        /// </summary>
        public void Dispose() => EndSession();
    }
}
