using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Message that shipping settings have changed.
    /// </summary>
    public class ShippingSettingsChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingSettingsChangedMessage(object sender, ShippingSettingsEntity shippingSettings)
        {
            Sender = sender;

            ShippingSettings = shippingSettings;
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Shipping settings that has changed
        /// </summary>
        public ShippingSettingsEntity ShippingSettings { get; private set; }
    }
}
