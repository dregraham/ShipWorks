using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.PlatforInterfaces
{
    public interface IOrderIdentifierFactory
    {
        OrderNumberIdentifier Create(OrderEntity order);
    }
}