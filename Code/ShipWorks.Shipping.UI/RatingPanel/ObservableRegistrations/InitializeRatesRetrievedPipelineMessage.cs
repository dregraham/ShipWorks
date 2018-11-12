using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.UI.RatingPanel.ObservableRegistrations
{
    /// <summary>
    /// Initialize rating
    /// </summary>
    public struct InitializeRatesRetrievedPipelineMessage : IShipWorksMessage
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public InitializeRatesRetrievedPipelineMessage(object sender, OrderEntity order)
        {
            MessageId = Guid.NewGuid();
            Sender = sender;
            Order = order;
        }

        /// <summary>
        /// Id of the message used for tracking
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Order that was found
        /// </summary>
        public OrderEntity Order { get; }
    }
}