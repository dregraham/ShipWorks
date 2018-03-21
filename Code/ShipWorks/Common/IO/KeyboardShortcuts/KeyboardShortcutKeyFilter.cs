using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Win32.Native;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// Key filter for keyboard shortcuts
    /// </summary>
    [Component(RegistrationType.Self)]
    public class KeyboardShortcutKeyFilter : IMessageFilter
    {
        /// <summary>
        /// Command keys and their generic equivalents
        /// </summary>
        private readonly Dictionary<VirtualKeys, KeyboardShortcutModifiers> commandKeys =
            new Dictionary<VirtualKeys, KeyboardShortcutModifiers>
            {
                { VirtualKeys.Control, KeyboardShortcutModifiers.Ctrl },
                { VirtualKeys.Shift, KeyboardShortcutModifiers.Shift },
                { VirtualKeys.Menu, KeyboardShortcutModifiers.Alt }
            };

        private readonly IMessenger messenger;
        private readonly IShortcutManager shortcutManager;
        private KeyboardShortcutModifiers modifiers = KeyboardShortcutModifiers.None;
        private readonly HashSet<VirtualKeys> pressedActionKeys = new HashSet<VirtualKeys>();

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyboardShortcutKeyFilter(IMessenger messenger, IShortcutManager shortcutManager)
        {
            this.shortcutManager = shortcutManager;
            this.messenger = messenger;
        }

        /// <summary>
        /// Listen for keyboard input
        /// </summary>
        public bool PreFilterMessage(ref Message message)
        {
            switch ((WindowsMessage) message.Msg)
            {
                case WindowsMessage.KEYFIRST:
                case WindowsMessage.SYSKEYDOWN:
                    return HandleKeyDown((VirtualKeys) message.WParam);
                case WindowsMessage.KEYLAST:
                case WindowsMessage.KEYUP:
                case WindowsMessage.SYSKEYUP:
                    HandleKeyUp((VirtualKeys) message.WParam);
                    break;
                case WindowsMessage.CHAR:
                case WindowsMessage.DEADCHAR:
                case WindowsMessage.SYSCHAR:
                case WindowsMessage.SYSDEADCHAR:
                    return pressedActionKeys.Contains((VirtualKeys) message.WParam);
            }

            return false;
        }

        /// <summary>
        /// Handle raw input message
        /// </summary>
        /// <returns>true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.</returns>
        private bool HandleKeyDown(VirtualKeys key)
        {
            if (commandKeys.ContainsKey(key))
            {
                modifiers |= commandKeys[key];

                return false;
            }

            return HandleActionKeyDown(key);
        }

        /// <summary>
        /// Handle an action key down
        /// </summary>
        private bool HandleActionKeyDown(VirtualKeys key)
        {
            ShortcutEntity command = shortcutManager.GetShortcut(key, modifiers);

            if (command != null)
            {
                messenger.Send(new KeyboardShortcutMessage(this, command));

                pressedActionKeys.Add(key);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Handle raw input message
        /// </summary>
        /// <returns>true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.</returns>
        private void HandleKeyUp(VirtualKeys key)
        {
            if (commandKeys.ContainsKey(key))
            {
                modifiers &= ~commandKeys[key];
            }
            else if (pressedActionKeys.Contains(key))
            {
                pressedActionKeys.Remove(key);
            }
        }
    }
}
