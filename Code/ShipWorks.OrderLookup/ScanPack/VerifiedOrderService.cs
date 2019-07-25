using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// Service for interacting with verified orders
    /// </summary>
    [Component]
    public class VerifiedOrderService : IVerifiedOrderService
    {
        private readonly IOrderRepository orderRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public VerifiedOrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        /// <summary>
        /// Save verified order data
        /// </summary>
        public void Save(OrderEntity order)
        {
            orderRepository.Save(order);
        }
    }
}
