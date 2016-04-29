using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Message that shipping settings have changed.
    /// </summary>
    public struct ShippingSettingsChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingSettingsChangedMessage(object sender, ShippingSettingsEntity shippingSettings)
        {
            Sender = sender;
            ShippingSettings = shippingSettings;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Shipping settings that has changed
        /// </summary>
        public ShippingSettingsEntity ShippingSettings { get; private set; }
    }
}
