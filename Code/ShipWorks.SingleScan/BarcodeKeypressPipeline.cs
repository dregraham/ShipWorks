using System;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using Interapptive.Shared.IO.Hardware;
using Interapptive.Shared.Win32.Native;
using Interapptive.Shared.UI;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Pipeline to simulate key presses
    /// </summary>
    public class BarcodeKeypressPipeline : IInitializeForCurrentUISession
    {
        private readonly IMessenger messenger;
        private readonly IVirtualKeyboard virtualKeyboard;
        private readonly IMessageHelper messageHelper;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public BarcodeKeypressPipeline(IMessenger messenger, IVirtualKeyboard virtualKeyboard, IMessageHelper messageHelper)
        {
            this.messenger = messenger;
            this.virtualKeyboard = virtualKeyboard;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Initialize the subscriptions
        /// </summary>
        public void InitializeForCurrentSession()
        {
            EndSession();

            subscription = messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.Tab) ||
                            m.AppliesTo(KeyboardShortcutCommand.Enter) ||
                            m.AppliesTo(KeyboardShortcutCommand.Escape))
                .Subscribe(HandleKeypressShortcutMessage);
        }

        /// <summary>
        /// Handle a keypress ShortcutMessage
        /// </summary>
        public void HandleKeypressShortcutMessage(ShortcutMessage shortcutMessage)
        {
            virtualKeyboard.Send(GetVirtualKey(shortcutMessage));
            messageHelper.ShowBarcodePopup($"{shortcutMessage.Shortcut.Action}");
        }

        /// <summary>
        /// Get the corrisponding virtual key
        /// </summary>
        private static VirtualKeys GetVirtualKey(ShortcutMessage shortcutMessage)
        {
            switch (shortcutMessage.Shortcut.Action)
            {
                case KeyboardShortcutCommand.Tab:
                    return VirtualKeys.Tab;
                case KeyboardShortcutCommand.Escape:
                    return VirtualKeys.Escape;
                case KeyboardShortcutCommand.Enter:
                    return VirtualKeys.Return;
                default:
                    throw new InvalidOperationException($"{shortcutMessage.Shortcut.Action} does not have a corresponding key.");
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
