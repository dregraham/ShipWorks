using System;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Common.IO.KeyboardShortcuts.Messages
{
    /// <summary>
    /// Apply the weight in a weight control
    /// </summary>
    [KeyedComponent(typeof(IShipWorksMessage), KeyboardShortcutCommand.ApplyWeight)]
    public class KeyboardShortcutMessage : IShipWorksMessage
    {
        private readonly ShortcutEntity shortcut;

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyboardShortcutMessage(object sender, ShortcutEntity shortcut)
        {
            MessageId = Guid.NewGuid();
            Sender = sender;
            this.shortcut = shortcut;
        }

        /// <summary>
        /// Id of the message, used for tracking
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Checks whether this message applies to the given command
        /// </summary>
        public bool AppliesTo(KeyboardShortcutCommand command) => (KeyboardShortcutCommand) this.shortcut.Action == command;

        /// <summary>
        /// Checks whether this message applies to the given shortcut
        /// </summary>
        public bool AppliesTo(ShortcutEntity shortcut) => AppliesTo((KeyboardShortcutCommand) shortcut.Action);
    }
}
