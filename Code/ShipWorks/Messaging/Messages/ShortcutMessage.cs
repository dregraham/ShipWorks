using System;
using Interapptive.Shared.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.SingleScan;

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
        public ShortcutMessage(object sender, IShortcutEntity shortcut, string value)
        {
            MessageId = Guid.NewGuid();
            Sender = sender;
            Shortcut = shortcut;
            Value = value;

            if (Sender is KeyboardShortcutKeyFilter)
            {
                Source = "Keyboard";
            }
            else if (Sender is ScanMessageBroker)
            {
                Source = "Barcode";
            }
            else
            {
                throw new ArgumentException("Unknown Shortcut sender");
            }
            
            CreatedDate = DateTime.UtcNow;
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
        /// The date this message was created
        /// </summary>
        public DateTime CreatedDate { get; }

        /// <summary>
        /// The value that triggered the shortcut
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// The source of the ShortcutMessage
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// The action of the shortcut
        /// </summary>
        public string Action
        {
            get
            {
                switch (Shortcut.Action)
                {
                    case KeyboardShortcutCommand.ApplyWeight:
                        return "ScaleReading";
                    case KeyboardShortcutCommand.ApplyProfile:
                        return "ShippingProfile";
                    default:
                        throw new ArgumentException("Unknown Shortcut command");
                }
            }
        }

        /// <summary>
        /// Checks whether this message applies to the given command
        /// </summary>
        public bool AppliesTo(KeyboardShortcutCommand command) => Shortcut.Action == command;
    }
}
