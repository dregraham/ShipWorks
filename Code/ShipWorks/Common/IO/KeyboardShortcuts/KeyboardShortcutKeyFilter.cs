using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Core.Messaging;

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
        private readonly Dictionary<VirtualKeys, VirtualKeys> commandKeys = new Dictionary<VirtualKeys, VirtualKeys> {
            {VirtualKeys.LeftControl, VirtualKeys.Control },
            {VirtualKeys.RightControl, VirtualKeys.Control },
            {VirtualKeys.Control, VirtualKeys.Control },
            {VirtualKeys.LeftShift, VirtualKeys.Shift },
            {VirtualKeys.RightShift, VirtualKeys.Shift },
            {VirtualKeys.Shift, VirtualKeys.Shift },
            {VirtualKeys.LeftMenu, VirtualKeys.Menu },
            {VirtualKeys.RightMenu, VirtualKeys.Menu },
            {VirtualKeys.Menu, VirtualKeys.Menu }
        };

        private readonly IMessenger messenger;
        private readonly IKeyboardShortcutTranslator shortcutTranslator;
        private readonly IUser32Input user32Input;
        private readonly HashSet<VirtualKeys> pressedCommandKeys = new HashSet<VirtualKeys>();
        private readonly HashSet<VirtualKeys> pressedActionKeys = new HashSet<VirtualKeys>();

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyboardShortcutKeyFilter(IMessenger messenger, IUser32Input user32Input,
            IKeyboardShortcutTranslator shortcutTranslator)
        {
            this.user32Input = user32Input;
            this.shortcutTranslator = shortcutTranslator;
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
                pressedCommandKeys.Add(key);

                return false;
            }

            return HandleActionKeyDown(key);
        }

        /// <summary>
        /// Handle an action key down
        /// </summary>
        private bool HandleActionKeyDown(VirtualKeys key)
        {
            IEnumerable<Func<object, IShipWorksMessage>> commands =
                shortcutTranslator.GetCommands(pressedCommandKeys.Select(NormalizeCommandKeys).Distinct().Concat(new[] { key }));

            foreach (Func<object, IShipWorksMessage> command in commands)
            {
                messenger.Send(command(this));
            }

            if (commands.Any())
            {
                pressedActionKeys.Add(key);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Translate a command key to its side-independent version
        /// </summary>
        private VirtualKeys NormalizeCommandKeys(VirtualKeys key) =>
            commandKeys.ContainsKey(key) ? commandKeys[key] : key;

        /// <summary>
        /// Handle raw input message
        /// </summary>
        /// <returns>true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.</returns>
        private void HandleKeyUp(VirtualKeys key)
        {
            if (pressedCommandKeys.Contains(key))
            {
                pressedCommandKeys.Remove(key);
            }
            else if (pressedActionKeys.Contains(key))
            {
                pressedActionKeys.Remove(key);
            }
        }
    }
}
