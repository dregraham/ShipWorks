using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders;

namespace ShipWorks.OrderLookup.ScanPack
{
    [Component]
    public class VerifiedOrderService : IVerifiedOrderService
    {
        private readonly IOrderRepository orderRepository;

        public VerifiedOrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public void Save(OrderEntity order)
        {
            orderRepository.Save(order);

        }
    }
}
