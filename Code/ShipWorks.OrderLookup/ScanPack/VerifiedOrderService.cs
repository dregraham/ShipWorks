using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public VerifiedOrderService(IOrderRepository orderRepository, IUserSession userSession)
        {
            this.orderRepository = orderRepository;
            this.userSession = userSession;
        }

        /// <summary>
        /// Save verified order data
        /// </summary>
        public void Save(OrderEntity order)
        {
            order.Verified = true;
            order.VerifiedBy = userSession.User.UserID;
            order.VerifiedDate = DateTime.UtcNow;

            orderRepository.Save(order);
        }
    }
}
