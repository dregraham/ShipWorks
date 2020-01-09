using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Orders;
using ShipWorks.Stores.Orders;
using ShipWorks.Users;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// Service for interacting with verified orders
    /// </summary>
    [Component]
    public class VerifiedOrderService : IVerifiedOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUserSession userSession;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public VerifiedOrderService(IOrderRepository orderRepository, IUserSession userSession, IDateTimeProvider dateTimeProvider, IMessenger messenger)
        {
            this.orderRepository = orderRepository;
            this.userSession = userSession;
            this.dateTimeProvider = dateTimeProvider;
            this.messenger = messenger;
        }

        /// <summary>
        /// Save verified order data
        /// </summary>
        public void Save(OrderEntity order, bool verified)
        {
            order.Verified = verified;
            order.VerifiedBy = verified ? (long?) userSession.User.UserID : null;
            order.VerifiedDate = verified ? (DateTime?) dateTimeProvider.UtcNow : null;

            orderRepository.Save(order);

            if (verified)
            {
                messenger.Send(new OrderVerifiedMessage(this, order));
            }
        }
    }
}
