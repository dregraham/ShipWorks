using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.PlatforInterfaces;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    public class SparkPayOrderIdentifierFactory : IOrderIdentifierFactory
    {
        public OrderNumberIdentifier Create(OrderEntity order)
        {
            return new OrderNumberIdentifier(order.OrderNumber);
        }
    }
}
