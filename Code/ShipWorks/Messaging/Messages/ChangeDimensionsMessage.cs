using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.IO.Hardware.Scales;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Message trigerred by a user wanting to use the measurements of a dimensionalizer.
    /// </summary>
    public class ChangeDimensionsMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChangeDimensionsMessage(object sender, ScaleReadResult scaleReadResult)
        {
            Sender = sender;
            MessageId = Guid.NewGuid();
            ScaleReadResult = scaleReadResult;
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// The ScaleReadResult which prompted the message
        /// </summary>
        public ScaleReadResult ScaleReadResult { get; }
    }
}
