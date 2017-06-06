using System;
using System.Collections.Generic;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shared.IO.KeyboardShortcuts;

namespace ShipWorks.Common.IO.KeyboardShortcuts.Messages
{
    /// <summary>
    /// Apply the weight in a weight control
    /// </summary>
    [KeyedComponent(typeof(IShipWorksMessage), KeyboardShortcutCommand.ApplyWeight)]
    public class KeyboardShortcutMessage : IShipWorksMessage
    {
        private readonly KeyboardShortcutCommand command;

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyboardShortcutMessage(object sender, KeyboardShortcutCommand command)
        {
            MessageId = Guid.NewGuid();
            Sender = sender;
            this.command = command;
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
        public bool AppliesTo(KeyboardShortcutCommand command) => this.command == command;
    }
}
