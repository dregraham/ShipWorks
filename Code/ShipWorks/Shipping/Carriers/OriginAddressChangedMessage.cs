using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Message that shipping accounts have changed for a carrier.
    /// </summary>
    public class OriginAddressChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OriginAddressChangedMessage(object sender)
        {
            Sender = sender;
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }
    }
}
