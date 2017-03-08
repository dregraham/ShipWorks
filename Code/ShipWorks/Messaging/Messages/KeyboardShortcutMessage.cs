using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Interapptive.Shared.Messaging;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shared.IO.KeyboardShortcuts;

namespace ShipWorks.Common.IO.KeyboardShortcuts.Messages
{
    /// <summary>
    /// Apply the weight in a weight control
    /// </summary>
    [KeyedComponent(typeof(IShipWorksMessage), KeyboardShortcutCommand.ApplyWeight)]
    public class KeyboardShortcutMessage : IShipWorksMessage
    {
        private readonly ImmutableHashSet<KeyboardShortcutCommand> commands;

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyboardShortcutMessage(object sender, IEnumerable<KeyboardShortcutCommand> commands)
        {
            MessageId = Guid.NewGuid();
            Sender = sender;
            this.commands = commands.ToImmutableHashSet();
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
        public bool AppliesTo(KeyboardShortcutCommand command) =>
            commands.Contains(command);
    }
}
