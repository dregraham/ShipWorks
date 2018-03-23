using System;
using Interapptive.Shared.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Common.IO.KeyboardShortcuts.Messages
{
    /// <summary>
    /// Apply the weight in a weight control
    /// </summary>
    public struct ShortcutMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShortcutMessage(object sender, IShortcutEntity shortcut)
        {
            MessageId = Guid.NewGuid();
            Sender = sender;
            Shortcut = shortcut;
        }

        /// <summary>
        /// The shortcut 
        /// </summary>
        public IShortcutEntity Shortcut { get; }

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
        public bool AppliesTo(KeyboardShortcutCommand command) => Shortcut.Action == command;
    }
}
